namespace Domain.Consults;

public class RemoteConsult : ConsultBase
{
    public required string Url { get; set; }
    
    public override decimal Cost => Doctor.Rate * 0.8m;
}