using Domain;
using Persistence;

namespace BusinessLogic.Services;

public class PatientsService(PatientsRepository patientsRepository)
{
    public List<Patient> GetPatients()
    {
        return patientsRepository.GetPatients().ToList();
    }
    
    public void AddPatient(Patient patient)
    {
        if (IsPatientNameUnique(patient.Name))
        {
            patientsRepository.AddPatient(patient);
        }
        else throw new InvalidOperationException($"A patient with the name '{patient.Name}' already exists.");
    }

    private bool IsPatientNameUnique(string name)
    {
        return !patientsRepository.GetPatients().Any(patient => patient.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}