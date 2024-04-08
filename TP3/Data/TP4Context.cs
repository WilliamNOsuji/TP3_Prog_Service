using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TP4.Models;

namespace TP4.Data
{
    public class TP4Context : IdentityDbContext<User>
    {
        public TP4Context (DbContextOptions<TP4Context> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        base.OnModelCreating(builder);

        PasswordHasher<User> hasher = new PasswordHasher<User>();
        User u1 = new User
        {
            Id = "11111111-1111-1111-1111-111111111111", // Format GUID
            UserName = "User1",
            Email = "user1@gmail.com",
            NormalizedEmail = "USER1@GMAIL.COM",
            NormalizedUserName = "USER1"
        };
        u1.PasswordHash = hasher.HashPassword(u1, "user@123");

        User u2 = new User
        {
            Id = "11111111-1111-1111-1111-111111111112", // Format GUID
            UserName = "User2",
            Email = "user2@gmail.com",
            NormalizedEmail = "USER2@GMAIL.COM",
            NormalizedUserName = "USER2"
        };

        u2.PasswordHash = hasher.HashPassword(u2, "user@123");

        builder.Entity<User>().HasData(u1, u2);

        var todayDateString = DateTime.Today.ToString("yyyy-MM-dd");


            builder.Entity<Score>().HasData(
            new
            {
                Id = 1,
                Pseudo = "user1",
                Date = todayDateString,
                Temps = 40.00f,
                ScoreValue = 45,
                IsPublic = true,
                UserId = "11111111-1111-1111-1111-111111111111"
            },
            new
            {
                Id = 2,
                Pseudo = "user1",
                Date = todayDateString,
                Temps = 80.00f,
                ScoreValue = 75,
                IsPublic = false,
                UserId = "11111111-1111-1111-1111-111111111111"
            },
            new
            {
                Id = 3,
                Pseudo = "user2",
                Date = todayDateString,
                Temps = 30.00f,
                ScoreValue = 45,
                IsPublic = true,
                UserId = "11111111-1111-1111-1111-111111111112"
            },
            new
            {
                Id = 4,
                Pseudo = "user2",
                Date = todayDateString,
                Temps = 10.00f,
                ScoreValue = 8,
                IsPublic = false,
                UserId = "11111111-1111-1111-1111-111111111112"
            });
        }

        public DbSet<TP4.Models.Score> Score { get; set; } = default!;
    }
}
