namespace Domain.Consults;

public class RemoteConsult : ConsultBase
{
    public required string Url { get; set; }
    
    protected override decimal BaseCost => Doctor.Rate * 0.8m;
}