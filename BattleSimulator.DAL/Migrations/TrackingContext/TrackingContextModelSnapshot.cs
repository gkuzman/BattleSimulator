﻿// <auto-generated />
using BattleSimulator.DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BattleSimulator.DAL.Migrations.TrackingContext
{
    [DbContext(typeof(BattleSimulator.DAL.Contexts.TrackingContext))]
    partial class TrackingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BattleSimulator.Entities.DB.Army", b =>
                {
                    b.Property<string>("Name");

                    b.Property<int>("BattleId");

                    b.Property<int>("AttackStrategy");

                    b.Property<int>("Units");

                    b.HasKey("Name", "BattleId");

                    b.HasIndex("BattleId");

                    b.ToTable("Armies");
                });

            modelBuilder.Entity("BattleSimulator.Entities.DB.Battle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BattleStatus");

                    b.HasKey("Id");

                    b.ToTable("Battles");
                });

            modelBuilder.Entity("BattleSimulator.Entities.DB.Army", b =>
                {
                    b.HasOne("BattleSimulator.Entities.DB.Battle", "Battle")
                        .WithMany("Armies")
                        .HasForeignKey("BattleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
