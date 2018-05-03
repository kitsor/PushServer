namespace KitsorLab.PushServer.Kernel.SeedWork
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface IRepository<T> where T : IAggregateRoot
	{
		IUnitOfWork UnitOfWork { get; }

		Task<T> AddAsync(T entity);
		Task<T> GetByKeyAsync(long key, bool isReadOnly = true);
		Task UpdateAsync(T entity);
		Task DeleteAsync(T entity);
		Task SaveRange(IEnumerable<T> entities, bool detachEntities = true);
	}
}
