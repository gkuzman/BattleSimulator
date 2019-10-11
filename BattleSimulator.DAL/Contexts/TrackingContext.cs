using BattleSimulator.Entities.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleSimulator.DAL.Contexts
{
    public class TrackingContext : DbContext
    {
        public virtual DbSet<Battle> Battles { get; set; }
        public virtual DbSet<Army> Armies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Battle>().HasKey(x => x.Id);
            builder.Entity<Army>().HasOne(a => a.Battle).WithMany(b => b.Armies).HasForeignKey(x => new { x.Name, x.BattleId });
        }
    }
}
