using BulkyBook.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		public DbSet<Category> Categories { get; set; } //Creating db table. Type Category is pulled from Models folder. 
		public DbSet<CoverType> CoverTypes { get; set; }
	}
}

