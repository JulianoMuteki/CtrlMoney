using CtrlMoney.Domain.Entities.FinancialClassification;
using CtrlMoney.Infra.Context.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CtrlMoney.Infra.Context.Mapping.FinancialClassification
{
    /// <summary>
    /// The 'Composite' class
    /// </summary>
    public class GrandChildTreeMap : EntityConfigurationBase<GrandChildTree>
    {
        protected override void Initialize(EntityTypeBuilder<GrandChildTree> builder)
        {
            base.Initialize(builder);

            builder.ToTable("GrandChildrenTrees");
            builder.Property(x => x.Id).HasColumnName("GrandChildTreeID");
            builder.HasKey(b => b.Id).HasName("GrandChildTreeID");

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(250);

            builder.HasMany(x => x.Children)
                 .WithOne(x => x.ParentNode)
                 .HasForeignKey(x => x.ParentNodeID)
                 .IsRequired(true);

        }
    }
}