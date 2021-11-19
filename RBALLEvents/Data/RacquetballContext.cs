using Microsoft.EntityFrameworkCore;
using RBALLEvents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBALLEvents.Data
{
    public class RacquetballContext : DbContext 
    {
        public RacquetballContext(DbContextOptions<RacquetballContext> options) : base(options)
        {

        }
        public DbSet<Event> Events { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<EventCoordinator> EventCoordinators { get; set; }
        public DbSet<EventAssignment> EventAssignments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<EventLocation> EventLocations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().ToTable("Event");
            modelBuilder.Entity<Registration>().ToTable("Registration");
            modelBuilder.Entity<Member>().ToTable("Member");
            modelBuilder.Entity<Club>().ToTable("Club");
            modelBuilder.Entity<EventCoordinator>().ToTable("EventCoordinator");
            modelBuilder.Entity<EventAssignment>().ToTable("EventAssignment");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<EventLocation>().ToTable("EventLocation");

            modelBuilder.Entity<EventAssignment>()
                .HasKey(c => new { c.EventID, c.EventCoordinatorID });
        }
    }
}
