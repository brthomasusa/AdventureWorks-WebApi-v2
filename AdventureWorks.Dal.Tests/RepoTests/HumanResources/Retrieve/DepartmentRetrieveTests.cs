using System;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.HumanResources;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using AdventureWorks.Models.HumanResources;
using Xunit;
using LoggerService;
using LoggingLevel = NLog.LogLevel;

namespace AdventureWorks.Dal.Tests.RepoTests.HumanResources.Retrieve
{
    [Collection("AdventureWorks.Dal")]
    public class DepartmentRetrieveTests : RepoTestsBase
    {
        private readonly IDepartmentRepository _deptRepo;

        public DepartmentRetrieveTests()
        {
            _deptRepo = new DepartmentRepository(ctx, new MockLoggerManager());
        }

        [Fact]
        public void ShouldGetAllDepartments()
        {
            var departments = _deptRepo.GetDepartments(new DepartmentParameters { PageNumber = 1, PageSize = 16 });
            var count = departments.Count;
            Assert.Equal(16, count);
        }
    }
}