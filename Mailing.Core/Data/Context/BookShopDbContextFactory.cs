using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mailing.Core.Context
{

    public class GigDbContextFactory<T> : IDesignTimeDbContextFactory<T> where T : DbContext
    {

        public T CreateDbContext(string[] args)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile("appsettings.Development.json", optional: true)
               .Build();

            var builder = new DbContextOptionsBuilder<T>();
            builder.EnableSensitiveDataLogging(true);

            var connectionString = configuration["ConnectionStrings:Default"];
            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly(this.GetType().Assembly.FullName));
            var dbContext = (T)Activator.CreateInstance(typeof(T), builder.Options);
            return dbContext;
        }
    }
}
