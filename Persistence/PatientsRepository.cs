using Domain;

namespace Persistence;

public class PatientsRepository(InMemoryDatabase database)
{
    public ICollection<Patient> GetPatients()
    {
        return database.Patients;
    }

    public void AddPatient(Patient patient)
    {
        database.Patients.Add(patient);
    }
    
    public void RemovePatient(Patient patient)
    {
        database.Patients.Remove(patient);
        database.PendingConsults.RemoveAll(consult => consult.Patient.Equals(patient));
    }
}