namespace Tutorial5.DTOs;

public class PrescriptionResponseDto
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }

    public DoctorSimpleDto Doctor { get; set; } = null!;
    public List<MedicamentDetailsDto> Medicaments { get; set; } = new();
}

public class DoctorSimpleDto
{
    public int IdDoctor { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}
