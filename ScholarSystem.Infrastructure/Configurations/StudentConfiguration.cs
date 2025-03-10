using ScholarSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ScholarSystem.Infrastructure.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Email)
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(s => s.EnrollmentDate)
                .IsRequired();

            builder.ToTable("Students");

        }
    }
}
