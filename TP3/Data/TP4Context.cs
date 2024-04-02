using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public DbSet<TP4.Models.Score> Score { get; set; } = default!;
    }
}
