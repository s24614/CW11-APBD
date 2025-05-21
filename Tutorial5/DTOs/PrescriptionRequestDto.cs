namespace Tutorial5.DTOs;

public class PrescriptionRequestDto
{
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }

    public PatientDto Patient { get; set; } = null!;
    public int IdDoctor { get; set; }
    public List<PrescriptionMedicamentDto> Medicaments { get; set; } = new();
}

public class PatientDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime BirthDate { get; set; }
}

public class PrescriptionMedicamentDto
{
    public int IdMedicament { get; set; }
    public int Dose { get; set; }
    public string Description { get; set; } = null!;
}
