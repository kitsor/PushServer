namespace KitsorLab.PushServer.Infastructure.Repositories
{
	using KitsorLab.PushServer.Kernel.SeedWork;
	using Microsoft.EntityFrameworkCore;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	public abstract class RepositoryBase<TC, TE>
		where TC : DbContext, IUnitOfWork
		where TE : Entity
	{
		protected readonly TC _context;
		protected TC Context => _context;
		public IUnitOfWork UnitOfWork => _context;

		public RepositoryBase(TC context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		/// <param name="notification"></param>
		/// <returns></returns>
		public Task<TE> AddAsync(TE entity)
		{
			return _context.Set<TE>().AddAsync(entity)
							.ContinueWith(s => s.Result.Entity);
		}

		/// <param name="entity"></param>
		/// <returns></returns>
		public Task UpdateAsync(TE entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
			return Task.CompletedTask;
		}

		/// <param name="entity"></param>
		/// <returns></returns>
		public Task DeleteAsync(TE entity)
		{
			_context.Entry(entity).State = EntityState.Deleted;
			return Task.CompletedTask;
		}

		/// <param name="entities"></param>
		/// <param name="detachEntities"></param>
		/// <returns></returns>
		public Task SaveRange(IEnumerable<TE> entities, bool detachEntities = true)
		{
			// store autodetect changes settings
			bool autoDetectChanges = _context.ChangeTracker.AutoDetectChangesEnabled;
			_context.ChangeTracker.AutoDetectChangesEnabled = !detachEntities;

			Task task = _context.AddRangeAsync(entities)
				.ContinueWith(t =>
				{
					_context.SaveChangesAsync();

					// restore autodetect changes settings
					_context.ChangeTracker.AutoDetectChangesEnabled = autoDetectChanges;

					if (detachEntities)
					{
						_context.ChangeTracker.Entries<TE>()
							.Where(x => entities.Any(y => y.PrimaryKey == x.Entity.PrimaryKey))
							.ToList()
							.ForEach(x => x.State = EntityState.Detached);
					}
				});

			return task;
		}

		/// <typeparam name="T"></typeparam>
		/// <param name="dbSet"></param>
		/// <param name="isReadOnly"></param>
		/// <returns></returns>
		protected IQueryable<TE> CreateQuery(IQueryable<TE> queryable, bool isReadOnly)
		{
			return isReadOnly
				? queryable.AsNoTracking()
				: queryable;
		}
	}
}
