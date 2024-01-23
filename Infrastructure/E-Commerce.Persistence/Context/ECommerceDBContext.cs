using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.Common;
using E_Commerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using File = E_Commerce.Domain.Entities.File;

namespace E_Commerce.Persistence.Context
{
	public class ECommerceDBContext : IdentityDbContext<AppUser, AppRole, string>
	{
		public ECommerceDBContext(DbContextOptions options) : base(options)
		{
		}
		public DbSet<Product> Products { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Order> Orders { get; set; }

		public DbSet<File> Files { get; set; }
		public DbSet<ProductImageFile> ProductImageFiles { get; set; }
		public DbSet<InvoiceFile> InvoiceFiles { get; set; }
		public DbSet<Basket> Baskets { get; set; }
		public DbSet<BasketItem> BasketItems { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.Entity<Order>()
				.HasKey(o => o.Id);

			modelBuilder.Entity<Basket>()
				.HasOne(b => b.Order)
				.WithOne(o => o.Basket)
				.HasForeignKey<Order>(b => b.Id);

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
					_ => DateTime.UtcNow
				};
			}
			return base.SaveChangesAsync(cancellationToken);
		}
	}
}