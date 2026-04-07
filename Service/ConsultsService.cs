using Domain;
using Domain.Consults;
using Persistence;

namespace Service;

public class ConsultsService(ConsultsRepository consultsRepository)
{
    public void AddPendingConsult(ConsultBase consultBase)
    {
        if (IsTheDoctorFree(consultBase.Doctor, consultBase.DateTime))
        {
            consultsRepository.AddPendingConsult(consultBase);
        }
        else throw new InvalidOperationException("The doctor is not available at that time.");
    }
    
    private bool IsTheDoctorFree(Doctor doctor, DateTime dateTime)
    {
        return !consultsRepository.GetPendingConsults()
            .Any(consult => consult.Doctor.Equals(doctor) && consult.DateTime == dateTime);
    }
    
    public List<ConsultBase> GetPendingConsultsFromToday()
    {
        return consultsRepository.GetPendingConsults().Where(consult => consult.DateTime.Date == DateTime.Today).ToList();
    }
    
    public List<ConsultBase> GetPendingConsultsByDoctor(Doctor doctor)
    {
        return consultsRepository.GetPendingConsults().Where(consult => consult.Doctor.Equals(doctor)).ToList();
    }
    
    public void HandleNextPendingConsult()
    {
        var nextConsult = GetNextPendingUrgentConsult() ?? GetNextPendingConsult();
        consultsRepository.HandlePendingConsult(nextConsult);
    }
    
    private UrgentConsult? GetNextPendingUrgentConsult()
    {
        return consultsRepository.GetPendingConsults()
            .OfType<UrgentConsult>()
            .OrderBy(consult => consult.DateTime)
            .FirstOrDefault(consult => consult.DateTime.Date == DateTime.Today && consult.Priority == Priority.High);
    }

    private ConsultBase GetNextPendingConsult()
    {
        return consultsRepository.GetPendingConsults()
            .OrderBy(consult => consult.DateTime)
            .FirstOrDefault(consult => consult.DateTime.Date == DateTime.Today) ?? throw new InvalidOperationException("No pending consults for today.");
    }
    
    public List<ConsultBase> GetHandledConsults()
    {
        return consultsRepository.GetHandledConsults().ToList();
    }
    
    public decimal GetTotalEarningsFromToday()
    {
        return consultsRepository.GetHandledConsults()
            .Where(consult => consult.DateTime.Date == DateTime.Today)
            .Sum(consult => consult.Cost);
    }
}