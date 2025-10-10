using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Doc_Patient_Backend.Models
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        // Add DbSets for new models
        public DbSet<PatientInfo> PatientInfos { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentLog> AppointmentLogs { get; set; }
        public DbSet<AvailabilityHour> AvailabilityHours { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure PatientInfo one-to-one relationship with ApplicationUser
            builder.Entity<PatientInfo>()
                .HasOne(pi => pi.User)
                .WithOne() // A user can have one patient info record
                .HasForeignKey<PatientInfo>(pi => pi.UserId);

            // Configure unique constraint on PhoneNumber in PatientInfo
            builder.Entity<PatientInfo>()
                .HasIndex(p => p.PhoneNumber)
                .IsUnique();

            // Configure Appointment relationship with PatientInfo
            builder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany() // A patient can have many appointments
                .HasForeignKey(a => a.PatientId);

            // Configure AppointmentLog relationship with Appointment
            builder.Entity<AppointmentLog>()
                .HasOne(al => al.Appointment)
                .WithMany() // An appointment can have many logs
                .HasForeignKey(al => al.AppointmentId);

            // Configure AvailabilityHour relationship with ApplicationUser (Doctor)
            builder.Entity<AvailabilityHour>()
                .HasOne(ah => ah.Doctor)
                .WithMany() // A doctor can have many availability slots
                .HasForeignKey(ah => ah.DoctorId);

            // Configure Payment one-to-one relationship with Appointment
            builder.Entity<Payment>()
                .HasOne(p => p.Appointment)
                .WithOne() // An appointment has one payment
                .HasForeignKey<Payment>(p => p.AppointmentId);
        }
    }
}