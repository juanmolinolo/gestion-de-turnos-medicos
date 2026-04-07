namespace Domain.Consults;

public class UrgentConsult : ConsultBase
{
    public Priority Priority { get; set; }
    
    protected override decimal BaseCost => Doctor.Rate * 1.5m;
}