using CtrlMoney.Domain.Entities.FinancialClassification;
using CtrlMoney.Infra.Context.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CtrlMoney.Infra.Context.Mapping.FinancialClassification
{
    /// <summary>
    /// The 'Composite' class
    /// </summary>
    public class ParentTreeMap : EntityConfigurationBase<ParentTree>
    {
        protected override void Initialize(EntityTypeBuilder<ParentTree> builder)
        {
            base.Initialize(builder);

            builder.ToTable("ParentsTrees");
            builder.Property(x => x.Id).HasColumnName("ParentTreeID");
            builder.HasKey(b => b.Id).HasName("ParentTreeID");

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(250);

            builder.HasMany(x => x.Children)
                 .WithOne(x => x.ParentNode)
                 .HasForeignKey(x => x.ParentNodeID)
                 .IsRequired(false);

        }
    }
}