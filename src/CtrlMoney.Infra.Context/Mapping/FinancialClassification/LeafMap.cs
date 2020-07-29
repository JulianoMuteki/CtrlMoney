using CtrlMoney.Domain.Entities.FinancialClassification;
using CtrlMoney.Infra.Context.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CtrlMoney.Infra.Context.Mapping.FinancialClassification
{
    /// <summary>
    /// The 'Leaf' class
    /// </summary>
    public class LeafMap : EntityConfigurationBase<Leaf>
    {
        protected override void Initialize(EntityTypeBuilder<Leaf> builder)
        {
            base.Initialize(builder);

            builder.ToTable("Leaves");
            builder.Property(x => x.Id).HasColumnName("LeafID");
            builder.HasKey(b => b.Id).HasName("LeafID");

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(250);

        }
    }
}