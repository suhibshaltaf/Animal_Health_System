using Animal_Health_System.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection.Emit;

namespace Animal_Health_System.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Animal> animals { get; set; }


        public DbSet<Birth> births { get; set; }
        public DbSet<Farm> farms { get; set; }

        public DbSet<Mating> matings { get; set; }
        public DbSet<MedicalExamination> medicalExaminations { get; set; }
        public DbSet<MedicalRecord> medicalRecords { get; set; }
        public DbSet<Medication> medications { get; set; }
        public DbSet<Owner> owners { get; set; }
        public DbSet<Pregnancy> pregnancies { get; set; }
        public DbSet<Vaccine> vaccines { get; set; }
        public DbSet<VaccineHistory> vaccineHistories { get; set; }
        public DbSet<Veterinarian> veterinarians { get; set; }

        public DbSet<FarmStaff> farmStaff { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Animal -> Farm (Many-to-One)
            modelBuilder.Entity<Animal>()
                .HasOne(a => a.Farm)  // Animal has one Farm
                .WithMany(f => f.Animals) // Farm has many Animals
                .HasForeignKey(a => a.FarmId).OnDelete(DeleteBehavior.Restrict); // Foreign key in Animal



            // AnimalHealthHistory -> Animal (Many-to-One)
          
            // Birth -> Pregnancy (Many-to-One)
            modelBuilder.Entity<Birth>()
                .HasOne(b => b.Pregnancy)  // Birth has one Pregnancy
                .WithMany(p => p.Births) // Pregnancy has many Births
                .HasForeignKey(b => b.PregnancyId).OnDelete(DeleteBehavior.Restrict); // Foreign key in Birth

            // Birth -> Animal (Many-to-One)


            // FarmStaff -> Farm (Many-to-One)
            modelBuilder.Entity<FarmStaff>()
                .HasOne(fs => fs.Farm)  // FarmStaff has one Farm
                .WithMany(f => f.FarmStaffs) // Farm has many FarmStaffs
                .HasForeignKey(fs => fs.FarmId).OnDelete(DeleteBehavior.Restrict); // Foreign key in FarmStaff





            // MedicalExamination -> Animal (Many-to-One)
            modelBuilder.Entity<MedicalExamination>()
                .HasOne(me => me.Animal)  // MedicalExamination has one Animal
                .WithMany(a => a.MedicalExaminations) // Animal has many MedicalExaminations
                .HasForeignKey(me => me.AnimalId).OnDelete(DeleteBehavior.Restrict); // Foreign key in MedicalExamination

            // MedicalExamination -> FarmStaff (Many-to-One)


            // MedicalExamination -> Veterinarian (Many-to-One)
            modelBuilder.Entity<MedicalExamination>()
                .HasOne(me => me.Veterinarian)  // MedicalExamination has one Veterinarian
                .WithMany(v => v.MedicalExaminations) // Veterinarian has many MedicalExaminations
                .HasForeignKey(me => me.VeterinarianId).OnDelete(DeleteBehavior.Restrict); // Foreign key in MedicalExamination

            // MedicalRecord -> Animal (One-to-One)
            modelBuilder.Entity<MedicalRecord>()
                .HasOne(mr => mr.Animal)  // MedicalRecord has one Animal
                .WithOne(a => a.MedicalRecord) // Animal has one MedicalRecord
                .HasForeignKey<MedicalRecord>(mr => mr.AnimalId).OnDelete(DeleteBehavior.Restrict);// Foreign key in MedicalRecord

            // VaccineHistory -> Animal (Many-to-One)


            // VaccineHistory -> Vaccine (Many-to-One)
            modelBuilder.Entity<VaccineHistory>()
                .HasOne(vh => vh.vaccine)  // VaccineHistory has one Vaccine
                .WithMany(v => v.VaccineHistories) // Vaccine has many VaccineHistories
                .HasForeignKey(vh => vh.VaccineId).OnDelete(DeleteBehavior.Restrict); // Foreign key in VaccineHistory

            // VaccineHistory -> Veterinarian (Many-to-One)
            modelBuilder.Entity<VaccineHistory>()
                .HasOne(vh => vh.veterinarian)  // VaccineHistory has one Veterinarian
                .WithMany(v => v.VaccineHistories) // Veterinarian has many VaccineHistories
                .HasForeignKey(vh => vh.VeterinarianId).OnDelete(DeleteBehavior.Restrict); // Foreign key in VaccineHistory


            modelBuilder.Entity<VaccineHistory>()
    .HasOne(vh => vh.medicalRecord)
    .WithMany(mr => mr.vaccineHistories)
    .HasForeignKey(vh => vh.medicalRecordId)
    .OnDelete(DeleteBehavior.Restrict);





            modelBuilder.Entity<FarmStaff>()
        .HasOne(fs => fs.ApplicationUser)
        .WithOne(u => u.FarmStaff)
        .HasForeignKey<FarmStaff>(fs => fs.ApplicationUserId)
        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Owner>()
        .HasOne(fs => fs.ApplicationUser)
        .WithOne(u => u.Owner)
        .HasForeignKey<Owner>(fs => fs.ApplicationUserId)
        .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Veterinarian>()
        .HasOne(fs => fs.ApplicationUser)
        .WithOne(u => u.Veterinarian)
        .HasForeignKey<Veterinarian>(fs => fs.ApplicationUserId)
        .OnDelete(DeleteBehavior.Restrict);

            // PregnancyNotification -> Animal (Many-to-One)
         

            // Owner -> Farm (One-to-Many)
            modelBuilder.Entity<Owner>()
                .HasMany(o => o.Farms)
                .WithOne(f => f.Owner)
                .HasForeignKey(f => f.OwnerId).OnDelete(DeleteBehavior.Restrict);

            // Owner -> Notification (One-to-Many)
        
           

            // Notification -> Veterinarian (Many-to-One)
         



            // Medication -> MedicalExamination (Many-to-One)
            modelBuilder.Entity<MedicalExamination>()
      .HasMany(me => me.Medications)  // الفحص الطبي يمكن أن يحتوي على أكثر من دواء
      .WithMany(m => m.MedicalExaminations)  // الدواء يمكن أن يكون مرتبطًا بعدة فحوصات طبية
      .UsingEntity(j => j.ToTable("MedicalExamination_Medication"));

            // FarmHealthSummary -> Farm (Many-to-One)
         
            modelBuilder.Entity<Mating>()
        .HasOne(m => m.MaleAnimal)
        .WithMany()
        .HasForeignKey(m => m.MaleAnimalId)
        .OnDelete(DeleteBehavior.Restrict); // منع الحذف التتابعي

            modelBuilder.Entity<Mating>()
                .HasOne(m => m.FemaleAnimal)
                .WithMany()
                .HasForeignKey(m => m.FemaleAnimalId)
                .OnDelete(DeleteBehavior.Restrict);


            //------------unique  --------------


            modelBuilder.Entity<Animal>()
                .HasIndex(a => a.Name)
                .IsUnique();

            modelBuilder.Entity<Animal>()
                .HasIndex(a => new { a.FarmId, a.Name })
                .IsUnique();

          

            modelBuilder.Entity<Farm>()
                .HasIndex(f => new { f.Name, f.OwnerId })
                .IsUnique();

            modelBuilder.Entity<FarmStaff>()
                .HasIndex(fs => new { fs.FarmId, fs.FullName })
                .IsUnique();


            modelBuilder.Entity<Mating>()
                .HasIndex(m => new { m.MaleAnimalId, m.FemaleAnimalId, m.MatingDate })
                .IsUnique();

            modelBuilder.Entity<MedicalExamination>()
                .HasIndex(me => new { me.AnimalId, me.ExaminationDate })
                .IsUnique();

            modelBuilder.Entity<MedicalRecord>()
                .HasIndex(mr => mr.AnimalId)
                .IsUnique();

            modelBuilder.Entity<MedicalRecord>()
                .HasIndex(mr => mr.Name)
                .IsUnique();
            modelBuilder.Entity<Medication>()
                .HasIndex(m => m.Name)
                .IsUnique();

            modelBuilder.Entity<Vaccine>()
               .HasIndex(m => m.Name)
               .IsUnique();

         

            modelBuilder.Entity<Owner>()
                .HasIndex(o => o.Email)
                .IsUnique();

            modelBuilder.Entity<Pregnancy>()
                .HasIndex(p => new { p.AnimalId, p.MatingDate })
                .IsUnique();











            modelBuilder.Entity<Veterinarian>()
                .HasIndex(v => v.Email)
                .IsUnique();
            // ----------soft delet ----------
           
            modelBuilder.Entity<Animal>()
                .HasQueryFilter(d => !d.IsDeleted);

         

          

            modelBuilder.Entity<Birth>()
                .HasQueryFilter(d => !d.IsDeleted);



            modelBuilder.Entity<Farm>()
                .HasQueryFilter(d => !d.IsDeleted);

            

            modelBuilder.Entity<FarmStaff>()
                .HasQueryFilter(d => !d.IsDeleted);



            modelBuilder.Entity<Mating>()
                .HasQueryFilter(m => !m.IsDeleted);

            modelBuilder.Entity<MedicalExamination>()
                .HasQueryFilter(me => !me.IsDeleted);

            modelBuilder.Entity<MedicalRecord>()
                .HasQueryFilter(mr => !mr.IsDeleted);

            modelBuilder.Entity<Medication>()
                .HasQueryFilter(m => !m.IsDeleted);



         

            modelBuilder.Entity<Owner>()
                .HasQueryFilter(o => !o.IsDeleted);

            modelBuilder.Entity<Pregnancy>()
                .HasQueryFilter(p => !p.IsDeleted);







            modelBuilder.Entity<Vaccine>()
                .HasQueryFilter(v => !v.IsDeleted);

            modelBuilder.Entity<VaccineHistory>()
                .HasQueryFilter(vh => !vh.IsDeleted);



            modelBuilder.Entity<Veterinarian>()
                .HasQueryFilter(v => !v.IsDeleted);



          // تعريف المتغيرات للأدوار
            var roleAdminId = Guid.NewGuid().ToString();
            var ownerRoleId = Guid.NewGuid().ToString();
            var veterinarianRoleId = Guid.NewGuid().ToString();
            var farmStaffRoleId = Guid.NewGuid().ToString();

            // إضافة الأدوار
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = roleAdminId, Name = "Admin", NormalizedName = "ADMIN" }, // إضافة دور Admin دون ربطه بمستخدم
                new IdentityRole { Id = ownerRoleId, Name = "Owner", NormalizedName = "OWNER" },
                new IdentityRole { Id = veterinarianRoleId, Name = "Veterinarian", NormalizedName = "VETERINARIAN" },
                new IdentityRole { Id = farmStaffRoleId, Name = "FarmStaff", NormalizedName = "FARMSTAFF" }
            );

            var hasher = new PasswordHasher<ApplicationUser>();

            // تعريف مستخدم Admin
            var adminUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "admin@medc.com",
                NormalizedUserName = "ADMIN@MEDC.COM",
                Email = "admin@medc.com",
                NormalizedEmail = "ADMIN@MEDC.COM",
                EmailConfirmed = true,
                Role ="Admin" // تعيين قيمة للـ Role
            };
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Sohaib@18");

            // تعريف مستخدم Owner
            var ownerUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "owner@medc.com",
                NormalizedUserName = "OWNER@MEDC.COM",
                Email = "owner@medc.com",
                NormalizedEmail = "OWNER@MEDC.COM",
                EmailConfirmed = true,
                Role = "Owner" // تعيين قيمة للـ Role
            };
            ownerUser.PasswordHash = hasher.HashPassword(ownerUser, "Sohaib@18");

            // إضافة بيانات الـ Owner إلى جدول Owner
            var owner = new Owner
            {
                Id = 1,
                FullName = "John Doe",
                PhoneNumber = "123-456-7890",
                Email = "owner@medc.com",
                ApplicationUserId = ownerUser.Id // ربط المستخدم بـ ApplicationUser
            };

            // تعريف مستخدم Veterinarian
            var veterinarianUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "veterinarian@medc.com",
                NormalizedUserName = "VETERINARIAN@MEDC.COM",
                Email = "veterinarian@medc.com",
                NormalizedEmail = "VETERINARIAN@MEDC.COM",
                EmailConfirmed = true,
                Role = "Veterinarian" // تعيين قيمة للـ Role
            };
            veterinarianUser.PasswordHash = hasher.HashPassword(veterinarianUser, "Sohaib@18");

            // إضافة بيانات الـ Veterinarian إلى جدول Veterinarian
            var veterinarian = new Veterinarian
            {Id=1,
                FullName = "Dr. Smith",
                Specialty = "Small Animal",
                PhoneNumber = "123-456-7890",
                Email = "veterinarian@medc.com",
                YearsOfExperience = 5,
                salary = 50000,
                ApplicationUserId = veterinarianUser.Id // ربط المستخدم بـ ApplicationUser
            };

            // تعريف مستخدم FarmStaff
            var farmStaffUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "farmstaff@medc.com",
                NormalizedUserName = "FARMSTAFF@MEDC.COM",
                Email = "farmstaff@medc.com",
                NormalizedEmail = "FARMSTAFF@MEDC.COM",
                EmailConfirmed = true,
                Role = "FarmStaff" // تعيين قيمة للـ Role
            };
            farmStaffUser.PasswordHash = hasher.HashPassword(farmStaffUser, "Sohaib@18");

            // إضافة بيانات الـ FarmStaff إلى جدول FarmStaff
         /*  var farmStaff = new FarmStaff
              {
                  FullName = "Alex Johnson",
                  JobTitle = "Farm Worker",
                  PhoneNumber = "123-456-7890",
                  Email = "farmstaff@medc.com",
                  salary = 30000,
                  FarmId = null, // ربط مع مزرعة معينة
                  ApplicationUserId = farmStaffUser.Id // ربط المستخدم بـ ApplicationUser
              };
            */
            // إضافة المستخدمين إلى قاعدة البيانات
            modelBuilder.Entity<ApplicationUser>().HasData(adminUser, ownerUser, veterinarianUser, farmStaffUser);
            modelBuilder.Entity<Owner>().HasData(owner);
            modelBuilder.Entity<Veterinarian>().HasData(veterinarian);
            //  modelBuilder.Entity<FarmStaff>().HasData(farmStaff);

            // ربط المستخدمين بالأدوار الصحيحة
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = ownerRoleId, UserId = ownerUser.Id },
                new IdentityUserRole<string> { RoleId = veterinarianRoleId, UserId = veterinarianUser.Id },
                new IdentityUserRole<string> { RoleId = farmStaffRoleId, UserId = farmStaffUser.Id }
            );
           
        }



    }
}
