﻿using BattleSimulator.Entities.DB;
using Microsoft.EntityFrameworkCore;

namespace BattleSimulator.DAL.Contexts
{
    public class NonTrackingContext : DbContext
    {
        public NonTrackingContext(DbContextOptions<NonTrackingContext> options) : base(options)
        {

        }
        public virtual DbSet<Battle> Battles { get; set; }
        public virtual DbSet<Army> Armies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Battle>().HasKey(x => x.Id);
            builder.Entity<Army>().HasKey(x => new { x.Name, x.BattleId });
            builder.Entity<Army>().HasOne(a => a.Battle).WithMany(b => b.Armies).IsRequired();
        }
    }
}
