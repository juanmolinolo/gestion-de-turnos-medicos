using Domain;
using Domain.Consults;

namespace Persistence;

public class InMemoryDatabase
{
    public readonly List<Doctor> Doctors = [];
    public readonly List<Patient> Patients = [];
    public readonly List<ConsultBase> PendingConsults = [];
    public readonly List<ConsultBase> HandledConsults = [];

    public InMemoryDatabase()
    {
        LoadDoctors();
        LoadPatients();
    }

    private void LoadDoctors()
    {
        var fer = new Doctor()
        {
            Name = "Fernando",
            Specialty = Specialty.Cardiology,
            Rate = 15m
        };

        var juan = new Doctor
        {
            Name = "Juan",
            Specialty = Specialty.Dermatology,
            Rate = 20m
        };

        var gaston = new Doctor
        {
            Name = "Gaston",
            Rate = 35m
        };
        
        Doctors.Add(fer);
        Doctors.Add(juan);   
        Doctors.Add(gaston);
    }

    private void LoadPatients()
    {
        var nico = new Patient
        {
            Name = "Nico",
        };

        var alex = new Patient
        {
            Name = "Alex",
        };
        
        Patients.Add(nico);
        Patients.Add(alex);
    }
}