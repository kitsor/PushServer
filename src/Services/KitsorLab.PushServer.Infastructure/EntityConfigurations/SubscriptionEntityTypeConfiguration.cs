namespace KitsorLab.PushServer.Infastructure.EntityConfigurations
{
	using KitsorLab.PushServer.Kernel.Models.Subscription;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	internal class SubscriptionEntityTypeConfiguration : IEntityTypeConfiguration<Subscription>
	{
		public void Configure(EntityTypeBuilder<Subscription> builder)
		{
			builder.ToTable("Subscriptions", PushServerDbContext.DEFAULT_SCHEMA);
			builder.HasKey(x => x.SubscriptionKey);

			builder.Property(x => x.UserId).HasMaxLength(32).IsRequired();
			builder.Property(x => x.Endpoint).IsRequired(false);
			builder.Property(x => x.PublicKey).HasMaxLength(90).IsRequired(false);
			builder.Property(x => x.Token).HasMaxLength(25).IsRequired(false);
			builder.Property(x => x.DeviceToken).HasMaxLength(255).IsRequired(false);
			builder.Property(x => x.Type).IsRequired();

			builder.Property(x => x.IP).IsRequired(false);

			builder.Ignore(x => x.IPAddress);
			builder.Ignore(x => x.DomainEvents);

			builder.HasIndex(x => x.UserId).IsUnique();
		}
	}
}
