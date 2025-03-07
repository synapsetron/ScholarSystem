using EntryTask.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            // 1: N 
            builder.HasOne(c => c.Teacher)
                   .WithMany(t => t.Courses)
                   .HasForeignKey(c => c.TeacherId)
                   .OnDelete(DeleteBehavior.Restrict);

            // many to many 
            builder.HasMany(c => c.Students)
                   .WithMany(s => s.Courses);

            builder.ToTable("Courses");
        }
    }
}
