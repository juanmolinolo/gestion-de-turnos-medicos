using BusinessLogic.Services;
using Domain;
using Domain.Consults;
using Persistence;

var database = new InMemoryDatabase();
var doctorsService = new DoctorsService(new DoctorsRepository(database));
var patientsService = new PatientsService(new PatientsRepository(database));
var consultsService = new ConsultsService(new ConsultsRepository(database));

while (true)
{
	Console.Clear();
	Console.WriteLine("=== Sistema de Gestion de Turnos Medicos ===");
	Console.WriteLine("1) Registrar turno");
	Console.WriteLine("2) Listar turnos del dia");
	Console.WriteLine("3) Listar turnos por medico");
	Console.WriteLine("4) Atender proximo turno");
	Console.WriteLine("5) Ver historial");
	Console.WriteLine("6) Recaudacion del dia");
	Console.WriteLine("0) Salir");

	var option = ReadIntInRange("Seleccione una opcion: ", 0, 6);
	Console.WriteLine();

	try
	{
		switch (option)
		{
			case 1:
				RegisterConsult(doctorsService, patientsService, consultsService);
				break;
			case 2:
				ListTodayConsults(consultsService);
				break;
			case 3:
				ListConsultsByDoctor(doctorsService, consultsService);
				break;
			case 4:
				HandleNextConsult(consultsService);
				break;
			case 5:
				ShowHistory(consultsService);
				break;
			case 6:
				ShowDailyRevenue(consultsService);
				break;
			case 0:
				return;
		}
	}
	catch (InvalidOperationException ex)
	{
		Console.WriteLine($"Error de negocio: {ex.Message}");
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Error inesperado: {ex.Message}");
	}

	Pause();
}

static void RegisterConsult(DoctorsService doctorsService, PatientsService patientsService, ConsultsService consultsService)
{
	Console.WriteLine("== Registrar turno ==");

	var type = ReadIntInRange("Tipo (1: General, 2: Urgencia, 3: Telemedicina): ", 1, 3);
	var doctor = SelectDoctor(doctorsService.GetDoctors());
	var patient = SelectOrCreatePatient(patientsService);
	var consultDateTime = ReadDateTime("Fecha y hora (ej: 2026-04-07 14:30): ");

	ConsultBase consult = type switch
	{
		1 => new GeneralConsult
		{
			Doctor = doctor,
			Patient = patient,
			DateTime = consultDateTime
		},
		2 => new UrgentConsult
		{
			Doctor = doctor,
			Patient = patient,
			DateTime = consultDateTime,
			Priority = ReadPriority()
		},
		3 => new RemoteConsult
		{
			Doctor = doctor,
			Patient = patient,
			DateTime = consultDateTime,
			Url = ReadUrl("Link de videollamada: ")
		},
		_ => throw new InvalidOperationException("Tipo de consulta invalido.")
	};

	consultsService.AddPendingConsult(consult);
	Console.WriteLine("Turno registrado correctamente.");
}

static void ListTodayConsults(ConsultsService consultsService)
{
	Console.WriteLine("== Turnos del dia ==");
	var consults = consultsService.GetPendingConsultsFromToday()
		.OrderBy(consult => consult.DateTime)
		.ToList();

	PrintConsults(consults);
}

static void ListConsultsByDoctor(DoctorsService doctorsService, ConsultsService consultsService)
{
	Console.WriteLine("== Turnos por medico ==");
	var doctor = SelectDoctor(doctorsService.GetDoctors());
	var consults = consultsService.GetPendingConsultsByDoctor(doctor)
		.OrderBy(consult => consult.DateTime)
		.ToList();

	PrintConsults(consults);
}

static void HandleNextConsult(ConsultsService consultsService)
{
	Console.WriteLine("== Atender proximo turno ==");
	consultsService.HandleNextPendingConsult();
	Console.WriteLine("Se atendio el proximo turno pendiente.");
}

static void ShowHistory(ConsultsService consultsService)
{
	Console.WriteLine("== Historial de turnos atendidos ==");
	var handled = consultsService.GetHandledConsults()
		.OrderBy(consult => consult.DateTime)
		.ToList();

	PrintConsults(handled);
}

static void ShowDailyRevenue(ConsultsService consultsService)
{
	Console.WriteLine("== Recaudacion del dia ==");
	var total = consultsService.GetTotalEarningsFromToday();
	Console.WriteLine($"Total cobrado hoy: {total:0.00}");
}

static Doctor SelectDoctor(List<Doctor> doctors)
{
	if (doctors.Count == 0)
	{
		throw new InvalidOperationException("No hay medicos cargados.");
	}

	Console.WriteLine("Medicos disponibles:");
	for (var i = 0; i < doctors.Count; i++)
	{
		var doctor = doctors[i];
		Console.WriteLine($"{i + 1}) {doctor.Name} - {doctor.Specialty} - Base: {doctor.Rate:0.00}");
	}

	var selectedIndex = ReadIntInRange("Seleccione medico: ", 1, doctors.Count) - 1;
	return doctors[selectedIndex];
}

static Patient SelectOrCreatePatient(PatientsService patientsService)
{
	while (true)
	{
		var patients = patientsService.GetPatients();

		Console.WriteLine();
		Console.WriteLine("Pacientes:");
		for (var i = 0; i < patients.Count; i++)
		{
			var patient = patients[i];
			var insuranceText = patient.HasInsurance ? "Con obra social" : "Sin obra social";
			Console.WriteLine($"{i + 1}) {patient.Name} ({insuranceText})");
		}
		Console.WriteLine("N) Crear nuevo paciente");

		Console.Write("Seleccione paciente o N: ");
		var input = (Console.ReadLine() ?? string.Empty).Trim();

		if (input.Equals("N", StringComparison.OrdinalIgnoreCase))
		{
			var name = ReadRequiredText("Nombre del paciente: ");
			var hasInsurance = ReadYesNo("Tiene obra social? (S/N): ");
			var newPatient = new Patient
			{
				Name = name,
				HasInsurance = hasInsurance
			};

			patientsService.AddPatient(newPatient);
			return newPatient;
		}

		if (int.TryParse(input, out var patientOption) && patientOption >= 1 && patientOption <= patients.Count)
		{
			return patients[patientOption - 1];
		}

		Console.WriteLine("Entrada invalida. Intente nuevamente.");
	}
}

static Priority ReadPriority()
{
	Console.WriteLine("Prioridad de urgencia:");
	Console.WriteLine("1) Alta");
	Console.WriteLine("2) Media");
	Console.WriteLine("3) Baja");

	var option = ReadIntInRange("Seleccione prioridad: ", 1, 3);
	return option switch
	{
		1 => Priority.High,
		2 => Priority.Medium,
		3 => Priority.Low,
		_ => Priority.Low
	};
}

static string ReadUrl(string message)
{
	while (true)
	{
		var url = ReadRequiredText(message);
		if (Uri.TryCreate(url, UriKind.Absolute, out _))
		{
			return url;
		}

		Console.WriteLine("El link no es valido. Intente de nuevo (ej: https://meet.example.com/abc). ");
	}
}

static void PrintConsults(List<ConsultBase> consults)
{
	if (consults.Count == 0)
	{
		Console.WriteLine("No hay turnos para mostrar.");
		return;
	}

	foreach (var consult in consults)
	{
		Console.WriteLine(FormatConsultLine(consult));
	}
}

static string FormatConsultLine(ConsultBase consult)
{
	var typeLabel = consult switch
	{
		UrgentConsult urgent => $"Urgencia ({MapPriority(urgent.Priority)})",
		RemoteConsult remote => $"Telemedicina ({remote.Url})",
		_ => "General"
	};

	return $"[{consult.DateTime:yyyy-MM-dd HH:mm}] {typeLabel} | Paciente: {consult.Patient.Name} | Medico: {consult.Doctor.Name} | Costo: {consult.Cost:0.00}";
}

static string MapPriority(Priority priority)
{
	return priority switch
	{
		Priority.High => "Alta",
		Priority.Medium => "Media",
		Priority.Low => "Baja",
		_ => priority.ToString()
	};
}

static DateTime ReadDateTime(string message)
{
	while (true)
	{
		Console.Write(message);
		var text = Console.ReadLine();

		if (!string.IsNullOrWhiteSpace(text) && DateTime.TryParse(text, out var dateTime))
		{
			return dateTime;
		}

		Console.WriteLine("Fecha y hora invalida. Use un formato como 2026-04-07 14:30.");
	}
}

static string ReadRequiredText(string message)
{
	while (true)
	{
		Console.Write(message);
		var text = Console.ReadLine();
		if (!string.IsNullOrWhiteSpace(text))
		{
			return text.Trim();
		}

		Console.WriteLine("El valor no puede estar vacio.");
	}
}

static bool ReadYesNo(string message)
{
	while (true)
	{
		Console.Write(message);
		var text = (Console.ReadLine() ?? string.Empty).Trim();
		if (text.Equals("S", StringComparison.OrdinalIgnoreCase) || text.Equals("SI", StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}

		if (text.Equals("N", StringComparison.OrdinalIgnoreCase) || text.Equals("NO", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}

		Console.WriteLine("Ingrese S/Si o N/No.");
	}
}

static int ReadIntInRange(string message, int min, int max)
{
	while (true)
	{
		Console.Write(message);
		var text = Console.ReadLine();

		if (int.TryParse(text, out var value) && value >= min && value <= max)
		{
			return value;
		}

		Console.WriteLine($"Ingrese un numero entre {min} y {max}.");
	}
}

static void Pause()
{
	Console.WriteLine();
	Console.WriteLine("Presione Enter para continuar...");
	Console.ReadLine();
}
