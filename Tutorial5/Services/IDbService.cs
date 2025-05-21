using Tutorial5.DTOs;

namespace Tutorial5.Services;

public interface IDbService
{
    Task<int> AddPrescriptionAsync(PrescriptionRequestDto dto);
    Task<PatientDetailsDto?> GetPatientDetailsAsync(int idPatient);
}