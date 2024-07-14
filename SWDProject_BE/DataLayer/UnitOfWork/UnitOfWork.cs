using DataLayer.Model;
using DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly SWD392_DBContext _context;

		public UnitOfWork(SWD392_DBContext context)
		{
			_context = context;
		}

		private readonly Dictionary<Type, object> reposotories = new Dictionary<Type, object>();

		public IGenericRepository<T> Repository<T>()
			where T : class
		{
			Type type = typeof(T);
			if (!reposotories.TryGetValue(type, out object value))
			{
				var genericRepos = new GenericRepository<T>(_context);
				reposotories.Add(type, genericRepos);
				return genericRepos;
			}
			return value as GenericRepository<T>;
		}

		private bool disposed = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					_context.Dispose();
				}
			}
			this.disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public int Commit()
		{
			return _context.SaveChanges();
		}

		public Task<int> CommitAsync()
		{
			TrackChanges();
			return _context.SaveChangesAsync();
		}

		private void TrackChanges()
		{
			var validationErrors = _context.ChangeTracker.Entries<IValidatableObject>()
				.SelectMany(e => e.Entity.Validate(null))
				.Where(e => e != ValidationResult.Success)
				.ToArray();
			if (validationErrors.Any())
			{
				var exceptionMessage = string.Join(Environment.NewLine,
					validationErrors.Select(error => $"Properties {error.MemberNames} Error: {error.ErrorMessage}"));
				throw new Exception(exceptionMessage);
			}
		}
	}
}
