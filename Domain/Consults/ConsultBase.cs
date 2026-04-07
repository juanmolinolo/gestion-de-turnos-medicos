namespace Domain.Consults;

public abstract class ConsultBase
{
    public required Doctor Doctor { get; set; }
    
    public required Patient Patient { get; set; }

    public DateTime DateTime { get; set; } = DateTime.Now;

    public virtual decimal Cost => Doctor.Rate;
}