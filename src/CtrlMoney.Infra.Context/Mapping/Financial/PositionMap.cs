using CtrlMoney.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CtrlMoney.Infra.Context.Mapping.Financial
{
    internal class PositionMap : EntityConfigurationBase<Position>
    {
        protected override void Initialize(EntityTypeBuilder<Position> builder)
        {
            base.Initialize(builder);

            builder.ToTable("Positions");
            builder.Property(x => x.Id).HasColumnName("PositionID");
            builder.HasKey(b => b.Id).HasName("PositionID");

            builder.Property(e => e.PositionDate)
                    .IsRequired();

            builder.Property(e => e.EInvestmentType)
                  .HasConversion<int>();

            builder.Property(e => e.TicketCode)
                    .IsRequired()
                    .HasColumnType("varchar")
                    .HasMaxLength(50);

            builder.Property(e => e.StockBroker)
                    .IsRequired()
                    .HasColumnType("varchar")
                    .HasMaxLength(50);

            builder.Property(e => e.ISINCode)
                    .IsRequired()
                    .HasColumnType("varchar")
                    .HasMaxLength(50);

            builder.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnType("varchar")
                    .HasMaxLength(50);

            builder.Property(e => e.Bookkeeping)
                    .IsRequired()
                    .HasColumnType("varchar")
                    .HasMaxLength(50);

            builder.Property(e => e.Quantity)
                    .IsRequired();

            builder.Property(e => e.QuantityAvailable)
                    .IsRequired();

            builder.Property(e => e.QuantityUnavailable)
                    .IsRequired();

            builder.Property(e => e.Reason)
                    .IsRequired()
                    .HasColumnType("varchar")
                    .HasMaxLength(50);

            builder.Property(e => e.ClosingPrice)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

            builder.Property(e => e.ValueUpdated)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();
        }
    }
}
