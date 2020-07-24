using System;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.HumanResources;
using AdventureWorks.Models.CustomTypes;
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Models.Person;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.HumanResources.Update
{
    [Collection("AdventureWorks.Dal")]
    public class EmployeeRepoTests : RepoTestsBase
    {
        private readonly IEmployeeRepo _employeeRepo;

        public EmployeeRepoTests()
        {
            _employeeRepo = new EmployeeRepo(ctx);
        }

        public override void Dispose()
        {
            _employeeRepo.Dispose();
        }
    }
}