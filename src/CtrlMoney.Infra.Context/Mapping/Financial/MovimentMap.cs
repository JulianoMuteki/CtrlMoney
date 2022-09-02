using CtrlMoney.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CtrlMoney.Infra.Context.Mapping.Financial
{
    public class MovimentMap:EntityConfigurationBase<Moviment>
    {
        protected override void Initialize(EntityTypeBuilder<Moviment> builder)
        {
            base.Initialize(builder);

            builder.ToTable("Moviments");
            builder.Property(x => x.Id).HasColumnName("MovimentsID");
            builder.HasKey(b => b.Id).HasName("MovimentsID");

            builder.Property(e => e.TicketCode)
                    .IsRequired()
                    .HasColumnType("varchar")
                    .HasMaxLength(50);

            builder.Property(e => e.InputOutput)
                    .IsRequired();

            builder.Property(e => e.Date)
                    .IsRequired();

            builder.Property(e => e.StockBroker)
                    .IsRequired()
                    .HasColumnType("varchar")
                    .HasMaxLength(50);

            builder.Property(e => e.Quantity)
                    .IsRequired();

            builder.Property(e => e.UnitPrice)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

            builder.Property(e => e.TransactionValue)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();
        }
    }
}
