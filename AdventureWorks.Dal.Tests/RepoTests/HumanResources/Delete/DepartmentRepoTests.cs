using System;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.HumanResources;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.HumanResources.Delete
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
        public void ShouldDeleteOneDepartment()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                short deptID = 5;
                var dept = _deptRepo.Find(deptID);

                Assert.NotNull(dept);

                _deptRepo.Delete(dept);

                dept = _deptRepo.Find(deptID);
                Assert.Null(dept);
            }
        }

        [Fact]
        public void ShouldRaiseConcurrencyException()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                short deptID = 5;
                var dept = _deptRepo.Find(deptID);

                Assert.NotNull(dept);

                _deptRepo.Delete(dept);

                Action testCode = () =>
                {
                    _deptRepo.Delete(dept);
                };

                var ex = Record.Exception(testCode);

                Assert.NotNull(ex);
                Assert.IsType<AdventureWorksConcurrencyExeception>(ex);
                Assert.Equal("Error: The department you are trying to delete does not exist. Try refreshing your screen.", ex.Message);
            }
        }
    }
}