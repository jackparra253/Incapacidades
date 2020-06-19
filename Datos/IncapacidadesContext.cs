﻿using System.Collections.Generic;
using IDatos;
using Microsoft.EntityFrameworkCore;
using Modelos.Constantes;
using Modelos.Entidades;
using Modelos.ValueObjects;
using Modelos.Enumeracion;
using System;

namespace Datos
{
    public partial class IncapacidadesContext : DbContext, IServicioDatos
    {
        public IncapacidadesContext(DbContextOptions<IncapacidadesContext> options) : base(options)
        {

        }

        public DbSet<Incapacidad> Incapacidades { get; set; }
        public DbSet<ReconocimientoEconomico> ReconocimientosEconomicos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("GTH");

            modelBuilder.Entity<Incapacidad>(entidad =>
            {
                entidad.ToTable("Incapacidades");
                entidad.HasKey(i => i.Id);
                entidad.Property(i => i.Observaciones).HasColumnType("varchar(200)").HasMaxLength(200);
                entidad.Property(i => i.FechaIncial).HasColumnType("smalldatetime").IsRequired();
                entidad.Property(i => i.FechaFinal).HasColumnType("smalldatetime").IsRequired();
                entidad.Property(i => i.CantidadDias).IsRequired();
                entidad.HasMany(i => i.ReconocimientosEconomicos).WithOne();

            });

            modelBuilder.Entity<ReconocimientoEconomico>(entidad =>
            {
                entidad.ToTable("ReconocimientosEconomicos");
                entidad.HasKey(re => re.Id);
                entidad.HasOne(re => re.Incapacidad)
                       .WithMany(i => i.ReconocimientosEconomicos)
                       .HasForeignKey(re => re.IdIncapacidad);

                entidad.OwnsOne(re => re.ValorAPagar)
                    .Property(p => p.Cantidad)
                    .HasColumnType("decimal(5, 2)")
                    .HasColumnName("DineroCantidad")
                    .IsRequired();


                entidad.OwnsOne(re => re.ValorAPagar)
                    .Property(p => p.Moneda)
                    .HasColumnType("varchar(3)")
                    .HasMaxLength(3)
                    .HasColumnName("DineroMoneda")
                    .IsRequired();
            });
        }
    }
}