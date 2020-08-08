using CtrlMoney.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CtrlMoney.Infra.Context.Mapping
{
    public class BankMap : EntityConfigurationBase<Bank>
    {
        protected override void Initialize(EntityTypeBuilder<Bank> builder)
        {
            base.Initialize(builder);

            builder.ToTable("Banks");
            builder.Property(x => x.Id).HasColumnName("BankID");
            builder.HasKey(b => b.Id).HasName("BankID");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(e => e.BankCode)
             .IsRequired();

            builder.Property(e => e.InitialBalance)
             .HasColumnType("decimal(10,2)")
             .IsRequired();

            builder.Property(e => e.BankBalance)
              .HasColumnType("decimal(10,2)")
               .IsRequired();
        }
    }
}
