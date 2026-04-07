namespace Domain;

public class Doctor
{
    public required string Name { get; set; }

    public Specialty Specialty { get; set; } = Specialty.General;
    
    public required decimal Rate { get; init; } 
}