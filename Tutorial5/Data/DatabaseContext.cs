using Microsoft.EntityFrameworkCore;
using Tutorial5.Models;

namespace Tutorial5.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Medicament> Medicaments => Set<Medicament>();
    public DbSet<Prescription> Prescriptions => Set<Prescription>();
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments => Set<PrescriptionMedicament>();

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PrescriptionMedicament>()
            .HasKey(pm => new { pm.IdPrescription, pm.IdMedicament });

        modelBuilder.Entity<PrescriptionMedicament>()
            .HasOne(pm => pm.Prescription)
            .WithMany(p => p.PrescriptionMedicaments)
            .HasForeignKey(pm => pm.IdPrescription);

        modelBuilder.Entity<PrescriptionMedicament>()
            .HasOne(pm => pm.Medicament)
            .WithMany(m => m.PrescriptionMedicaments)
            .HasForeignKey(pm => pm.IdMedicament);

        // Seed: Doktorzy
        modelBuilder.Entity<Doctor>().HasData(
            new Doctor { IdDoctor = 1, FirstName = "Jan", LastName = "Kowalski", Email = "jan.kowalski@hospital.com" },
            new Doctor { IdDoctor = 2, FirstName = "Anna", LastName = "Nowak", Email = "anna.nowak@hospital.com" }
        );

        // Seed: Leki
        modelBuilder.Entity<Medicament>().HasData(
            new Medicament { IdMedicament = 1, Name = "Apap", Description = "Painkiller", Type = "Tablet" },
            new Medicament { IdMedicament = 2, Name = "Ibuprom", Description = "Anti-inflammatory", Type = "Capsule" }
        );
    }
}