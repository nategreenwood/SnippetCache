using SnippetCache.Data.Configuration;
using SnippetCache.Data.SampleData;
using SnippetCache.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnippetCache.Data
{
    public class SnippetCacheDbContext : DbContext
    {
        public SnippetCacheDbContext()
            : base("snippetCache")
        {}

        public DbSet<Snippet> Snippets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Hyperlink> Hyperlinks { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }

        static SnippetCacheDbContext()
        {
            Database.SetInitializer(new SnippetCacheDatabaseInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new SnippetConfiguration());
        }
    }
}
