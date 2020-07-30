using CtrlMoney.Domain.Entities.FinancialClassification;
using CtrlMoney.Infra.Context.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CtrlMoney.Infra.Context.Mapping.FinancialClassification
{
    /// <summary>
    /// The 'Composite' class
    /// </summary>
    public class CompositeMap : EntityConfigurationBase<Composite>
    {
        protected override void Initialize(EntityTypeBuilder<Composite> builder)
        {
            base.Initialize(builder);

            builder.ToTable("Composites");
            builder.Property(x => x.Id).HasColumnName("CompositeID");
            builder.HasKey(b => b.Id).HasName("CompositeID");

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(250);

            builder.HasMany(x => x.Leaves)
                 .WithOne(x => x.Composite)
                 .HasForeignKey(x => x.CompositeID)
                 .IsRequired(true);


        }
    }
}