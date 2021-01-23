using CtrlMoney.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CtrlMoney.Infra.Context.Mapping.Financial
{
   public class FinancialTransactionMap : EntityConfigurationBase<FinancialTransaction>
    {
        protected override void Initialize(EntityTypeBuilder<FinancialTransaction> builder)
        {
            base.Initialize(builder);

            builder.ToTable("FinancialTransactions");
            builder.Property(x => x.Id).HasColumnName("FinancialTransactionID");
            builder.HasKey(b => b.Id).HasName("FinancialTransactionID");

            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(e => e.Value)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

            builder.Property(e => e.IsOperationDone)
                    .IsRequired();

            builder.Property(e => e.PaymentDate);

            builder.Property(e => e.ETransactionType)
                  .HasConversion<int>();

            builder.Property(e => e.EPaymentMethod)
                  .HasConversion<int>();

            builder.HasOne(tk => tk.Bank)
                .WithMany(k => k.FinancialTransactions)
                .HasForeignKey(tk => tk.BankID);

            builder.HasOne(tk => tk.ChildTree)
                .WithMany(k => k.FinancialTransactions)
                .HasForeignKey(tk => tk.ChildTreeID);

            builder.HasOne(tk => tk.ParentTree)
                .WithMany(k => k.FinancialTransactions)
                .HasForeignKey(tk => tk.ParentTreeID);

            builder.HasOne(tk => tk.GrandChildTree)
                .WithMany(k => k.FinancialTransactions)
                .HasForeignKey(tk => tk.GrandChildTreeID);
        }
    }
}
