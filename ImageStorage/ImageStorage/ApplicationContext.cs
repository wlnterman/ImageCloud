using Microsoft.EntityFrameworkCore;

namespace ImageStorage
{
    public class ApplicationContext : DbContext
    {
        public DbSet<SerializedDataPath> SerializedDataPaths { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }
    }
}