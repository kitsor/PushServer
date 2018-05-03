namespace KitsorLab.PushServer.Infastructure.EntityConfigurations
{
	using KitsorLab.PushServer.Kernel.Models.Notification;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	internal class NotificationEntityTypeConfiguration : IEntityTypeConfiguration<Notification>
	{
		public void Configure(EntityTypeBuilder<Notification> builder)
		{
			builder.ToTable("Notifications", PushServerDbContext.DEFAULT_SCHEMA);
			builder.HasKey(x => x.NotificationKey);

			builder.Property(x => x.NotificationId).IsRequired();
			builder.Property(x => x.Title).HasMaxLength(50).IsRequired();
			builder.Property(x => x.Message).HasMaxLength(100).IsRequired();
			builder.Property(x => x.Url).HasMaxLength(255);
			builder.Property(x => x.IconUrl).HasMaxLength(150);
			builder.Property(x => x.ImageUrl).HasMaxLength(150);

			builder.HasIndex(x => x.NotificationId).IsUnique();
		}
	}
}
