using Domain;
using Persistence;

namespace Service;

public class DoctorsService(DoctorsRepository doctorsRepository)
{
    public List<Doctor> GetDoctors()
    {
        return doctorsRepository.GetDoctors().ToList();
    }
}