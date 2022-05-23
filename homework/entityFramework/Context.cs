using homework.Models;
using homework.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homework.entityFramework
{
    public class Context : DbContext
    {


        public DbSet<FileModel> File { get; set; }
        public DbSet<ServType> ServType{ get; set; }
        public IQueryable<ReportModel> get_statistics() => FromExpression(() => get_statistics());
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }
        public Context()
            
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql(@"Server=localhost;Port=5432;Database=task;User Id=postgres;Password=sys123;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDbFunction(() => get_statistics());
        }

    }
}
