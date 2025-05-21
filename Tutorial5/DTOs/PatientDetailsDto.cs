namespace Tutorial5.DTOs;

public class PatientDetailsDto
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime BirthDate { get; set; }

    public List<PrescriptionResponseDto> Prescriptions { get; set; } = new();
}

public class MedicamentDetailsDto
{
    public int IdMedicament { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Dose { get; set; }
}
