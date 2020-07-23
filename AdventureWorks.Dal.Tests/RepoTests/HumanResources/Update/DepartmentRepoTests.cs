using System;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.HumanResources;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.HumanResources.Update
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
        public void ShouldUpdateDepartmentName()
        {
            short deptID = 5;
            var dept = _deptRepo.Find(deptID);

            Assert.NotNull(dept);
            Assert.Equal("Information Services", dept.Name);
            Assert.Equal("Executive General and Administration", dept.GroupName);

            dept.Name = "I.T. Department";
            dept.GroupName = "Sales and Marketing";
            _deptRepo.Update(dept);

            var result = _deptRepo.Find(deptID);
            Assert.Equal(dept.Name, result.Name);
            Assert.Equal(dept.GroupName, result.GroupName);
        }

        [Fact]
        public void ShouldRaiseExceptionAndNotUpdateDepartment()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                short deptID = 5;
                var dept = _deptRepo.Find(deptID);
                Assert.NotNull(dept);
                Assert.Equal("Information Services", dept.Name);

                dept.Name = "Engineering";

                Action testCode = () =>
                {
                    _deptRepo.Update(dept);
                };

                var ex = Record.Exception(testCode);

                Assert.NotNull(ex);
                Assert.IsType<AdventureWorksUniqueIndexException>(ex);
                Assert.Equal("Error: This operation would result in a duplicate department name!", ex.Message);
            }
        }
    }
}