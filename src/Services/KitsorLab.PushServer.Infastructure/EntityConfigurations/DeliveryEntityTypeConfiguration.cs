namespace KitsorLab.PushServer.Infastructure.EntityConfigurations
{
	using KitsorLab.PushServer.Kernel.Models.Delivery;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	internal class DeliveryEntityTypeConfiguration : IEntityTypeConfiguration<Delivery>
	{
		public void Configure(EntityTypeBuilder<Delivery> builder)
		{
			builder.ToTable("Deliveries", PushServerDbContext.DEFAULT_SCHEMA);
			builder.HasKey(x => x.DeliveryKey);

			builder.Property<long>("NotificationKey").IsRequired();
			builder.Property<long>("SubscriptionKey").IsRequired();
			builder.Property(x => x.Status).IsRequired()
				.HasDefaultValue(DeliveryStatus.New);

			builder.Ignore(x => x.DomainEvents);

			builder.HasIndex(x => new { x.Status, x.CreatedOn });
		}

	}
}
