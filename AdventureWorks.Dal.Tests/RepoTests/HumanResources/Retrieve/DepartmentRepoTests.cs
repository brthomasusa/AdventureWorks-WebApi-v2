using System;
using System.Collections.Generic;
using System.Linq;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.HumanResources;
using AdventureWorks.Models.CustomTypes;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.HumanResources.Retrieve
{
    [Collection("AdventureWorks.Dal")]
    public class DepartmentRepoTests : RepoTestsBase
    {
        private readonly IDepartmentRepo _deptRepo;

        public DepartmentRepoTests()
        {
            _deptRepo = new DepartmentRepo(ctx);
        }

        public override void Dispose()
        {
            _deptRepo.Dispose();
        }

        [Fact]
        public void ShouldGetAllDepartments()
        {
            var departments = _deptRepo.GetAll();
            Assert.NotNull(departments);

            int count = departments.Count();
            Assert.Equal(16, count);
        }

        [Fact]
        public void ShouldGetOneDepartmentByID()
        {
            short deptID = 5;
            var dept = _deptRepo.Find(deptID);

            Assert.NotNull(dept);
            Assert.Equal("Information Services", dept.Name);
        }

        [Fact]
        public void ShouldGetOneDepartmentByMiscCriteria()
        {
            var dept = _deptRepo.FindAsNoTracking(d => d.Name == "Information Services");
            Assert.NotNull(dept);
        }
    }
}