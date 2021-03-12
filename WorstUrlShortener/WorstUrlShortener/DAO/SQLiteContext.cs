using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using com.xyroh.lib;
using Microsoft.EntityFrameworkCore;
using WorstUrlShortener.Interfaces;
using WorstUrlShortener.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WorstUrlShortener.DAO
{
	public class SQLiteContext : DbContext
	{
		public DbSet<ShortenedUrl> ShortenedUrl { get; set; }


		public SQLiteContext()
		{
			SQLitePCL.Batteries_V2.Init();

			this.Database.EnsureCreated();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			XyrohLib.Log("Configuring DB: ");
			var dbFolder =  DependencyService.Get<IDBPath>().GetDBPath();
			XyrohLib.Log("Using Folder: " + dbFolder);
			var dbPath = Path.Combine(dbFolder, BaseConfig.dbName);
			XyrohLib.Log("Using DB: " + dbPath);

			optionsBuilder
				.UseSqlite($"Filename={dbPath}");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Performance Indexes
            modelBuilder.Entity<ShortenedUrl>().HasIndex(p => p.ShortenService);

        }


		public override int SaveChanges()
		{
			this.AddBaseModelValues();
			return base.SaveChanges();
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
		{
			this.AddBaseModelValues();
			return base.SaveChangesAsync(cancellationToken);
		}

		private void AddBaseModelValues()
		{
			var entities = this.ChangeTracker.Entries().Where(x => x.Entity is BaseModel && (x.State == EntityState.Added || x.State == EntityState.Modified));

			foreach (var entity in entities)
			{
				if (entity.State == EntityState.Added)
				{
					((BaseModel)entity.Entity).CreatedAt = DateTime.Now;
				}

				((BaseModel)entity.Entity).UpdatedAt = DateTime.Now;
			}
		}
	}
}
