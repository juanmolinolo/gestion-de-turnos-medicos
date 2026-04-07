namespace Domain;

public class Patient
{
    public required string Name { get; init; }

    /// <summary>
    /// Refiere al punto sobre obra social.
    /// </summary>
    public bool HasInsurance { get; set; } = false;
}