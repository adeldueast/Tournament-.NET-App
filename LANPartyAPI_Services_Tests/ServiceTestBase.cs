using LANPartyAPI_Core.Enums;
using LANPartyAPI_Core.Models;
using LANPartyAPI_DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANPartyAPI_Services_Tests
{
    public abstract class ServiceTestBase<T> where T : class
    {
        public T _sut;

        public readonly ApplicationDbContext _context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
            .Options
            );

        [OneTimeSetUp]
        public void Setup()
        {
            if (!_context.Database.IsInMemory()) return;
            _context.Database.EnsureCreated();
            SeedDatabase();
			_context.SaveChanges();
            _sut = Activator.CreateInstance(typeof(T), new object[] { _context }) as T;
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            if (!_context.Database.IsInMemory()) return;
            _context.Database.EnsureDeleted();
        }

		public abstract void SeedDatabase();
        
    }
}
