using Domain;

namespace Persistence;

public class DoctorsRepository(InMemoryDatabase database)
{
    public ICollection<Doctor> GetDoctors()
    {
        return database.Doctors;
    }
}