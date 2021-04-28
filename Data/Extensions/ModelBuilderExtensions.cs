using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using Data.Entities;

namespace Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void SeedAdminUser(this ModelBuilder modelBuilder)
        {
            //Seed Admin User
            var roleId = new Guid("8D04DCE2-969A-435D-BBA4-DF3F325983DC");
            var adminId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00DE");
            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = roleId,
                Name = "Admin",
                NormalizedName = "ADMIN",
                Description = "Quản trị viên"
            });
            var hasher = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = adminId,
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "trungthuongvo109@gmail.com",
                NormalizedEmail = "TRUNGTHUONGVO109@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Thuong0165@"),
                SecurityStamp = string.Empty,
                PhoneNumber = "0375274267",
                PhoneNumberConfirmed = true,
                FirstName = "Võ",
                LastName = "Trung Thường",
                Gender = Enums.Gender.Male,
                Address = "KTX Khu B, Đại Học Quốc Gia TPHCM",
                City = "Thành phố Hồ Chí Minh",
                Country = "Việt Nam",
                ProfilePicture = "https://res.cloudinary.com/dglgzh0aj/image/upload/v1617718785/upload/img/user/IMG_20200910_134644_flstzn.jpg",
                PublicId = "upload/img/user/IMG_20200910_134644_flstzn"
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleId,
                UserId = adminId
            });
        }
    }
}
