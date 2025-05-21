using Microsoft.EntityFrameworkCore;
using Tutorial5.Data;
using Tutorial5.DTOs;
using Tutorial5.Models;

namespace Tutorial5.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _context;

    public DbService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> AddPrescriptionAsync(PrescriptionRequestDto dto)
    {
        if (dto.Medicaments.Count > 10)
            throw new ArgumentException("Recepta może zawierać maksymalnie 10 leków.");

        if (dto.DueDate < dto.Date)
            throw new ArgumentException("DueDate nie może być wcześniejszy niż Date.");

        var doctor = await _context.Doctors.FindAsync(dto.IdDoctor)
            ?? throw new ArgumentException("Podany lekarz nie istnieje.");

        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.FirstName == dto.Patient.FirstName
                                   && p.LastName == dto.Patient.LastName
                                   && p.BirthDate == dto.Patient.BirthDate);

        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = dto.Patient.FirstName,
                LastName = dto.Patient.LastName,
                BirthDate = dto.Patient.BirthDate
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        var prescription = new Prescription
        {
            Date = dto.Date,
            DueDate = dto.DueDate,
            IdDoctor = doctor.IdDoctor,
            IdPatient = patient.IdPatient,
            PrescriptionMedicaments = new List<PrescriptionMedicament>()
        };

        foreach (var medDto in dto.Medicaments)
        {
            var medicament = await _context.Medicaments.FindAsync(medDto.IdMedicament);
            if (medicament == null)
                throw new ArgumentException($"Lek z ID {medDto.IdMedicament} nie istnieje.");

            prescription.PrescriptionMedicaments.Add(new PrescriptionMedicament
            {
                IdMedicament = medicament.IdMedicament,
                Dose = medDto.Dose,
                Description = medDto.Description
            });
        }

        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();

        return prescription.IdPrescription;
    }

    public async Task<PatientDetailsDto?> GetPatientDetailsAsync(int idPatient)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.PrescriptionMedicaments)
                    .ThenInclude(pm => pm.Medicament)
            .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.Doctor)
            .FirstOrDefaultAsync(p => p.IdPatient == idPatient);

        if (patient == null)
            return null;

        return new PatientDetailsDto
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            Prescriptions = patient.Prescriptions
                .OrderBy(p => p.DueDate)
                .Select(p => new PrescriptionResponseDto
                {
                    IdPrescription = p.IdPrescription,
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Doctor = new DoctorSimpleDto
                    {
                        IdDoctor = p.Doctor.IdDoctor,
                        FirstName = p.Doctor.FirstName,
                        LastName = p.Doctor.LastName
                    },
                    Medicaments = p.PrescriptionMedicaments.Select(pm => new MedicamentDetailsDto
                    {
                        IdMedicament = pm.Medicament.IdMedicament,
                        Name = pm.Medicament.Name,
                        Description = pm.Medicament.Description,
                        Dose = pm.Dose
                    }).ToList()
                }).ToList()
        };
    }
}