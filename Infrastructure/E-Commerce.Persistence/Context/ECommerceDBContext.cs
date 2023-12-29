﻿using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Persistence.Context
{
	public class ECommerceDBContext : DbContext
	{
		public ECommerceDBContext(DbContextOptions options) : base(options)
		{
		}
		public DbSet<Product> Products { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Order> Orders { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Order>().OwnsOne(x => x.Address, a => a.WithOwner());
			base.OnModelCreating(modelBuilder);
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			var datas = ChangeTracker.Entries<BaseEntity>();

			foreach (var data in datas)
			{
				_ = data.State switch
				{
					EntityState.Added => data.Entity.CreatedDate = DateTime.UtcNow,
					EntityState.Modified => data.Entity.UpdatedDate = DateTime.UtcNow,
				};
			}
			return base.SaveChangesAsync(cancellationToken);
		}
	}
}