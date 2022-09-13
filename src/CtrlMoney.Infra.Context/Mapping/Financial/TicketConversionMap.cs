using CtrlMoney.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CtrlMoney.Infra.Context.Mapping.Financial
{
    internal class TicketConversionMap : EntityConfigurationBase<TicketConversion>
    {
        protected override void Initialize(EntityTypeBuilder<TicketConversion> builder)
        {
            base.Initialize(builder);

            builder.ToTable("TicketsConversions");
            builder.Property(x => x.Id).HasColumnName("TicketConversionID");
            builder.HasKey(b => b.Id).HasName("TicketConversionID");

            builder.Property(e => e.TicketInput)
                    .IsRequired()
                    .HasColumnType("varchar")
                    .HasMaxLength(15);

            builder.Property(e => e.TicketOutput)
                    .IsRequired()
                    .HasColumnType("varchar")
                    .HasMaxLength(15);

            builder.Property(e => e.StockBroker)
                    .IsRequired()
                    .HasColumnType("varchar");

            builder.Property(e => e.UnitPrice)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

            builder.Property(e => e.Date)
                    .IsRequired();

            builder.Property(e => e.Quantity)   
                    .IsRequired();
        }
    }
}
