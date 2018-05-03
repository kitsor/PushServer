namespace KitsorLab.PushServer.Infastructure
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using KitsorLab.PushServer.Infastructure.EntityConfigurations;
	using KitsorLab.PushServer.Kernel.Models.Delivery;
	using KitsorLab.PushServer.Kernel.Models.Notification;
	using KitsorLab.PushServer.Kernel.Models.Subscription;
	using KitsorLab.PushServer.Kernel.SeedWork;
	using MediatR;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.ChangeTracking;
	using Microsoft.EntityFrameworkCore.Design;

	public class PushServerDbContext : DbContext, IUnitOfWork
	{
		public const string DEFAULT_SCHEMA = "pushServer";
		public DbSet<Subscription> Subscriptions { get; set; }
		public DbSet<Notification> Notifications { get; set; }
		public DbSet<Delivery> Deliveries { get; set; }

		private readonly IMediator _mediator;

		/// <param name="options"></param>
		private PushServerDbContext(DbContextOptions<PushServerDbContext> options)
			: base(options)
		{ }

		/// <param name="options"></param>
		/// <param name="mediator"></param>
		public PushServerDbContext(DbContextOptions<PushServerDbContext> options, IMediator mediator) : base(options)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		}

		/// <param name="modelBuilder"></param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new SubscriptionEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new NotificationEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new DeliveryEntityTypeConfiguration());
		}

		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			IEnumerable<EntityEntry<Entity>> domainEntities = ChangeTracker
								.Entries<Entity>()
								.Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

			IList<INotification> domainEvents = domainEntities
					.SelectMany(x => x.Entity.DomainEvents)
					.ToList();

			domainEntities.ToList()
					.ForEach(entity => entity.Entity.DomainEvents.Clear());

			IEnumerable<Task> tasks = domainEvents
					.Select(async (domainEvent) =>
					{
						await _mediator.Publish(domainEvent);
					});

			await Task.WhenAll(tasks);

			int result = await SaveChangesAsync();
			return true;
		}
	}

	public class PushServerDbContexDesignFactory : IDesignTimeDbContextFactory<PushServerDbContext>
	{
		public PushServerDbContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<PushServerDbContext>()
					.UseSqlServer("Server=.;Initial Catalog=PushServer;Integrated Security=true");

			return new PushServerDbContext(optionsBuilder.Options, new NoMediator());
		}

		class NoMediator : IMediator
		{
			public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default(CancellationToken)) where TNotification : INotification
			{
				return Task.CompletedTask;
			}

			public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default(CancellationToken))
			{
				return Task.FromResult<TResponse>(default(TResponse));
			}

			public Task Send(IRequest request, CancellationToken cancellationToken = default(CancellationToken))
			{
				return Task.CompletedTask;
			}
		}
	}
}
