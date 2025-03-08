using EntryTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntryTask.Infrastructure.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.TeacherId)
                .IsRequired();

            // 1:N with Teacher
            builder.HasOne(c => c.Teacher)
                   .WithMany(t => t.Courses)
                   .HasForeignKey(c => c.TeacherId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Courses");
        }
    }
}
