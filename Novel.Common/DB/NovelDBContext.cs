using Microsoft.EntityFrameworkCore;
using Novel.Common.DB.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Novel.Common.DB
{
    public class NovelDBContext : DbContext
    {
        public NovelDBContext(DbContextOptions<NovelDBContext> dbContextOptions) : base(dbContextOptions)
        {
        }
        public DbSet<BookShelf> BookShelve { get; set; }
        public DbSet<ReadRecord> ReadRecord { get; set; }
        public DbSet<BookCatalog> BookCatalog { get; set; }
        public DbSet<NovelBook> NovelBook { get; set; }
        public DbSet<NovelContent> NovelContent { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //}
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseMySql("");
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
