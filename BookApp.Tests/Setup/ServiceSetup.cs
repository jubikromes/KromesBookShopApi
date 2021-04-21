using BookApp.Core.Models;
using BookApp.Core.Services;
using BookApp.Core.Services.Interfaces;
using Mailing.Core.Context;
using Mailing.Core.Data;
using Mailing.Core.Data.Repositories;
using Mailing.Core.Models;
using Mailing.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookApp.Tests.Setup
{
    public  class ServiceSetup
    {
        public ServiceSetup()
        {

            var options = new DbContextOptionsBuilder<BookShopDbContext>().UseSqlite("DataSource=:memory:").Options;


            Context = new BookShopDbContext(options);
            UnitofWork = new UnitOfWork(Context);
            MemoryCache = new MemoryCache(new MemoryOptions());
            BookService = new BookService(UnitofWork, MemoryCache);

            Context.Database.OpenConnection();

            Context.Database.EnsureCreated();

            //Context.AddRange(new List<Author> { new Author { FirstName = "Oldberg", LastName = "James", Id = Guid.NewGuid() } });

            //Context.AddRange(new List<Category> { new Category { Title = "CIA Chimp" } });

        }

        public BookShopDbContext Context { get; set; }
        public IUnitOfWork UnitofWork { get; set; }

        public IBookService BookService { get; set; }

        public IMemoryCache MemoryCache { get; set; }




        public void Dispose()
        {
            Context.Database.EnsureDeleted();
        }



        public class MemoryOptions : IOptions<MemoryCacheOptions>
        {
            public MemoryCacheOptions Value => new MemoryCacheOptions { SizeLimit = 1040};
        }
    }
}
