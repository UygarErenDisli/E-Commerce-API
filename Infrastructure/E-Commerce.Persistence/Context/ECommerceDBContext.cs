using E_Commerce.Application.Enums;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.Common;
using E_Commerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Endpoint = E_Commerce.Domain.Entities.Endpoint;
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
		public DbSet<CompletedOrder> CompletedOrders { get; set; }
		public DbSet<CanceledOrder> CanceledOrders { get; set; }
		public DbSet<Notification> Notifications { get; set; }
		public DbSet<Menu> Menus { get; set; }
		public DbSet<Endpoint> Endpoints { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Order>()
				.HasKey(o => o.Id);

			modelBuilder.Entity<Basket>()
				.HasOne(b => b.Order)
				.WithOne(o => o.Basket)
				.HasForeignKey<Order>(b => b.Id);

			modelBuilder.Entity<Order>().OwnsOne(x => x.Address, a => a.WithOwner());

			modelBuilder.Entity<Order>()
				.HasIndex(o => o.OrderCode)
				.IsUnique();

			modelBuilder.Entity<Order>()
				.HasOne(o => o.CompletedOrder)
				.WithOne(co => co.Order)
				.HasForeignKey<CompletedOrder>();

			modelBuilder.Entity<Order>()
				.HasOne(o => o.CanceledOrder)
				.WithOne(co => co.Order)
				.HasForeignKey<CanceledOrder>();

			modelBuilder.Entity<AppUser>()
				.HasMany(u => u.Notifications)
				.WithOne(n => n.User)
				.HasForeignKey(n => n.UserId)
				.IsRequired();

			#region RoleSeeding
			string adminRoleId = Guid.NewGuid().ToString();
			var adminRole = new AppRole()
			{
				Id = adminRoleId,
				Name = "Admin",
				NormalizedName = "ADMIN"
			};
			var customerRole = new AppRole()
			{
				Id = Guid.NewGuid().ToString(),
				Name = "Customer",
				NormalizedName = "CUSTOMER"
			};
			modelBuilder.Entity<AppRole>().HasData(adminRole, customerRole);
			#endregion

			#region UserSeeding

			string adminId = Guid.NewGuid().ToString();
			var adminUser = new AppUser()
			{
				Id = adminId,
				UserName = "Admin",
				Email = "admin@gmail.com",
				NameSurname = "admin",
				NormalizedUserName = "ADMIN",
				NormalizedEmail = "ADMIN@GMAIL.COM".ToUpper(),
				PhoneNumber = "XXXXXXXXXXXXX",
				EmailConfirmed = true,
				PhoneNumberConfirmed = true,
				SecurityStamp = new Guid().ToString(),
			};

			PasswordHasher<AppUser> passwordHasher = new();
			adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "admin");

			modelBuilder.Entity<AppUser>().HasData(adminUser);

			#endregion

			#region IdentityUserRoleSeeding
			modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>()
			{
				RoleId = adminRoleId,
				UserId = adminId
			});
			#endregion

			#region MenuSeeding
			var basketMenu = new Menu()
			{
				Name = "Baskets",
				Id = Guid.NewGuid(),
				CreatedDate = DateTime.UtcNow

			};
			var ordersMenu = new Menu()
			{
				Name = "Orders",
				Id = Guid.NewGuid(),
				CreatedDate = DateTime.UtcNow
			};

			var notificationMenu = new Menu()
			{
				Name = "Notifications",
				Id = Guid.NewGuid(),
				CreatedDate = DateTime.UtcNow

			};

			modelBuilder.Entity<Menu>().HasData(basketMenu, notificationMenu, ordersMenu);
			#endregion

			#region EndpointSeeding


			Guid getBasketId = Guid.NewGuid();
			Guid updateBasketId = Guid.NewGuid();
			Guid putBasketId = Guid.NewGuid();
			Guid deleteBasketId = Guid.NewGuid();
			Guid getNotificationId = Guid.NewGuid();
			Guid deleteNotificationId = Guid.NewGuid();
			Guid completeOrderId = Guid.NewGuid();

			modelBuilder.Entity<Endpoint>().HasData(new()
			{
				Id = getBasketId,
				ActionType = ActionType.Reading.ToString(),
				HttpType = HttpMethods.Get,
				Definition = "Get Basket Items",
				Code = "GET.Reading.GetBasketItems",
				MenuId = basketMenu.Id,
				CreatedDate = DateTime.UtcNow
			}, new()
			{
				Id = updateBasketId,
				ActionType = ActionType.Writing.ToString(),
				HttpType = HttpMethods.Post,
				Definition = "Add Item To Basket",
				Code = "POST.Writing.AddItemToBasket",
				MenuId = basketMenu.Id,
				CreatedDate = DateTime.UtcNow
			}, new()
			{
				Id = putBasketId,
				ActionType = ActionType.Updating.ToString(),
				HttpType = HttpMethods.Put,
				Definition = "Update Item Quantity",
				Code = "PUT.Updating.UpdateItemQuantity",
				MenuId = basketMenu.Id,
				CreatedDate = DateTime.UtcNow
			}, new()
			{
				Id = deleteBasketId,
				ActionType = ActionType.Deleting.ToString(),
				HttpType = HttpMethods.Delete,
				Definition = "Deleting Basket Item",
				Code = "DELETE.Deleting.DeletingBasketItem",
				MenuId = basketMenu.Id,
				CreatedDate = DateTime.UtcNow
			}, new()
			{
				Id = getNotificationId,
				ActionType = ActionType.Reading.ToString(),
				HttpType = HttpMethods.Get,
				Definition = "Get User Notifications",
				Code = "GET.Reading.GetUserNotifications",
				MenuId = notificationMenu.Id,
				CreatedDate = DateTime.UtcNow
			}, new()
			{
				Id = deleteNotificationId,
				ActionType = ActionType.Deleting.ToString(),
				HttpType = HttpMethods.Delete,
				Definition = "Delete Notification",
				Code = "DELETE.Deleting.DeleteNotification",
				MenuId = notificationMenu.Id,
				CreatedDate = DateTime.UtcNow
			}, new()
			{
				Id = completeOrderId,
				ActionType = ActionType.Writing.ToString(),
				HttpType = HttpMethods.Post,
				Definition = "Create Order",
				Code = "POST.Writing.CreateOrder",
				MenuId = ordersMenu.Id,
				CreatedDate = DateTime.UtcNow
			});

			modelBuilder.Entity<Endpoint>()
			  .HasMany(p => p.Roles)
			  .WithMany(t => t.Endpoints)
			  .UsingEntity<Dictionary<string, object>>(
				  l => l.HasOne<AppRole>().WithMany().HasForeignKey("RolesId"),
				  r => r.HasOne<Endpoint>().WithMany().HasForeignKey("EndpointsId"),
				  je =>
				  {
					  je.HasKey("RolesId", "EndpointsId");
					  je.HasData(
						  new { RolesId = customerRole.Id, EndpointsId = getBasketId },
						  new { RolesId = customerRole.Id, EndpointsId = updateBasketId },
						  new { RolesId = customerRole.Id, EndpointsId = putBasketId },
						  new { RolesId = customerRole.Id, EndpointsId = deleteBasketId },
						  new { RolesId = customerRole.Id, EndpointsId = getNotificationId },
						  new { RolesId = customerRole.Id, EndpointsId = deleteNotificationId },
						  new { RolesId = customerRole.Id, EndpointsId = completeOrderId }
						  );
				  });
			#endregion

			#region ProductSeeding
			var products = new List<Product>();
			for (var i = 1; i < 13; i++)
			{
				products.Add(new()
				{
					Id = Guid.NewGuid(),
					Name = $"Test Product {i}",
					Price = i * 10,
					Stock = i * 10,
					CreatedDate = DateTime.UtcNow
				});
			}
			modelBuilder.Entity<Product>().HasData(products);
			#endregion

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