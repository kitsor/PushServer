namespace KitsorLab.PushServer.Infastructure.EntityConfigurations
{
	using KitsorLab.PushServer.Kernel.Models.Delivery;
	using KitsorLab.PushServer.Kernel.Models.Notification;
	using KitsorLab.PushServer.Kernel.Models.Subscription;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using System;

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
			builder.Property<DateTime?>("ScheduledOn").IsRequired(false);

			builder.Ignore(x => x.DomainEvents);

			builder.HasIndex(x => new { x.Status, x.ScheduledOn });

			builder.HasOne<Notification>()
				.WithMany()
				.IsRequired()
				.HasForeignKey("NotificationKey")
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne<Subscription>()
				.WithMany()
				.IsRequired()
				.HasForeignKey("SubscriptionKey")
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
