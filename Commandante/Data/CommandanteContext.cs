using Commandante.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Commandante.Data
{
    public class CommandanteContext : IdentityDbContext<CommandanteUser, IdentityRole, string>
    {
        private static bool _created = false;
        private static readonly string databaseFilePath = Path.Combine(Program.AppData.FullName, @"sqlite.db");
        public CommandanteContext()
        {
            if (!_created)
            {
                _created = true;
                Database.Migrate();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={databaseFilePath}");
        }

        // Any time this goes to production and the database schema isn't the way it was before, run the following command before deploying:
        // dotnet ef migrations add {DescribeChangeToDbSchema}
        public DbSet<Project> Projects { get; set; }

        public DbSet<Instance> Instances { get; set; }
    }
}
