using System;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.HumanResources;
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.HumanResources.Create
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
        public void ShouldCreateNewDepartment()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var dept = new Department
                {
                    Name = "Testing 1 2 3",
                    GroupName = "Research and Development"
                };

                _deptRepo.Add(dept);

                var testResult = _deptRepo.Find(dept.DepartmentID);
                Assert.NotNull(testResult);
            }
        }

        [Fact]
        public void ShouldRaiseExceptionAndNotAddNewDepartment()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                Action testCode = () =>
                {
                    _deptRepo.Add
                    (
                        new Department
                        {
                            Name = "Engineering",
                            GroupName = "Research and Development"
                        }
                    );
                };

                var ex = Record.Exception(testCode);

                Assert.NotNull(ex);
                Assert.IsType<AdventureWorksUniqueIndexException>(ex);
                Assert.Equal("Error: This operation would result in a duplicate department name!", ex.Message);
            }
        }
    }
}