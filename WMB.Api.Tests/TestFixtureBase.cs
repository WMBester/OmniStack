using NUnit.Framework;
using WMB.Api.DbContext;
using WMB.Api.Services;
using EntityFrameworkCore.Testing.Moq;

namespace WMB.Api.Tests
{
    [TestFixture]
    public abstract class TestFixtureBase
    {
        protected ApplicationDbContext _mockContext;
        protected CrudService _crudService;

        [SetUp]
        public virtual void Setup()
        {
            _mockContext = Create.MockedDbContextFor<ApplicationDbContext>();
            _crudService = new CrudService(_mockContext);
        }

        [TearDown]
        public virtual void TearDown()
        {
            _mockContext?.Dispose();
        }
    }
}
