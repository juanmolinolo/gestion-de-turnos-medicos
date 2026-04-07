namespace Domain.Consults;

public class UrgentConsult : ConsultBase
{
    public Priority Priority { get; set; }
    
    public  override decimal Cost => Doctor.Rate * 1.5m;
}