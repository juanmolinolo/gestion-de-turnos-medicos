namespace Domain.Consults;

public abstract class ConsultBase
{
    public required Doctor Doctor { get; set; }
    
    public required Patient Patient { get; set; }

    public DateTime DateTime { get; set; } = DateTime.Now;

    protected virtual decimal BaseCost => Doctor.Rate;

    public decimal Cost => Patient.HasInsurance ? BaseCost * 0.85m : BaseCost;
}