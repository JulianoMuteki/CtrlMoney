using CtrlMoney.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CtrlMoney.Infra.Context.Mapping.Financial
{
    internal class BrokerageHistoryMap : EntityConfigurationBase<BrokerageHistory>
    {
        protected override void Initialize(EntityTypeBuilder<BrokerageHistory> builder)
        {
            base.Initialize(builder);

            builder.ToTable("BrokeragesHistories");
            builder.Property(x => x.Id).HasColumnName("BrokerageHistoryID");
            builder.HasKey(b => b.Id).HasName("BrokerageHistoryID");

            builder.Property(e => e.TicketCode)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(50);

            builder.Property(e => e.StockBroker)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(50);

            builder.Property(e => e.TransactionType)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(50);

            builder.Property(e => e.TotalPrice)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

            builder.Property(e => e.Price)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

            builder.Property(e => e.Quantity)
                    .IsRequired();

            builder.Property(e => e.ExpireDate)                  
                    .IsRequired();

            builder.Property(e => e.TransactionDate)                   
                    .IsRequired();

            builder.Property(e => e.Category)
                    .IsRequired();
            builder.Property(e => e.Brokerage)
                    .IsRequired();
            builder.Property(e => e.Fees)
                    .IsRequired();
            builder.Property(e => e.Taxes)
                    .IsRequired();
            builder.Property(e => e.IRRF)
                    .IsRequired();
        }
    }
}
