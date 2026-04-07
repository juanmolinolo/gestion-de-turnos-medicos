using Domain;
using Persistence;

namespace BusinessLogic.Services;

public class DoctorsService(DoctorsRepository doctorsRepository)
{
    public List<Doctor> GetDoctors()
    {
        return doctorsRepository.GetDoctors().ToList();
    }
}