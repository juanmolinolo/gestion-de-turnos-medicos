using Domain.Consults;

namespace Persistence;

public class ConsultsRepository(InMemoryDatabase database)
{
    public ICollection<ConsultBase> GetPendingConsults()
    {
        return database.PendingConsults;
    }

    public ICollection<ConsultBase> GetHandledConsults()
    {
        return database.HandledConsults;
    }
    
    public void AddPendingConsult(ConsultBase consult)
    {
        database.PendingConsults.Add(consult);
    }

    public void RemovePendingConsult(ConsultBase consult)
    {
        database.PendingConsults.Remove(consult);
    }
    
    public void HandlePendingConsult(ConsultBase consult)
    {
        database.PendingConsults.Remove(consult);
        database.HandledConsults.Add(consult);
    }
}