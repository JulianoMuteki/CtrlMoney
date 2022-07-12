using CtrlMoney.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CtrlMoney.Infra.Context.Mapping.Financial
{
    internal class EarningMap : EntityConfigurationBase<Earning>
    {
        protected override void Initialize(EntityTypeBuilder<Earning> builder)
        {
            base.Initialize(builder);

            builder.ToTable("Earnings");
            builder.Property(x => x.Id).HasColumnName("EarningID");
            builder.HasKey(b => b.Id).HasName("EarningID");

            builder.Property(e => e.TicketCode)
                    .IsRequired()
                    .HasColumnType("varchar")
                    .HasMaxLength(50);

            builder.Property(e => e.StockBroker)
                    .IsRequired()
                    .HasColumnType("varchar")
                    .HasMaxLength(50);

            builder.Property(e => e.PaymentDate)
                    .IsRequired()
                    .HasColumnType("varchar")
                    .HasMaxLength(50);

            builder.Property(e => e.EventType)
                    .IsRequired()
                    .HasColumnType("varchar")
                    .HasMaxLength(50);

            builder.Property(e => e.StockBroker)
                    .IsRequired()
                    .HasColumnType("varchar")
                    .HasMaxLength(50);

            builder.Property(e => e.Quantity)
                    .IsRequired();

            builder.Property(e => e.UnitPrice)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

            builder.Property(e => e.NetValue)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();
        }
    }
}
