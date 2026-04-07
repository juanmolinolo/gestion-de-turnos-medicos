using Domain;
using Domain.Consults;

namespace Persistence;

public class InMemoryDatabase
{
    public readonly List<Doctor> Doctors = [];
    public readonly List<Patient> Patients = [];
    public readonly List<ConsultBase> Consults = [];

    public InMemoryDatabase()
    {
        LoadDoctors();
    }

    private void LoadDoctors()
    {
        var fer = new Doctor()
        {
            Name = "Fernando",
            Rate = 15m
        };

        var juan = new Doctor()
        {
            Name = "Juan",
            Rate = 20m
        };

        var gaston = new Doctor()
        {
            Name = "Gaston",
            Rate = 35m
        };
        
        Doctors.Add(fer);
        Doctors.Add(juan);   
        Doctors.Add(gaston);
    }
}