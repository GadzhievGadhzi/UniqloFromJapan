using Microsoft.EntityFrameworkCore;
using UniqloFromJapan.Models;

namespace UniqloFromJapan.Data {
    public class DataRepository : DbContext {
        public DbSet<Product> Products { get; set; }

        public DataRepository(DbContextOptions<DataRepository> options) : base(options) {
            Database.EnsureCreated();
        }
    }
}