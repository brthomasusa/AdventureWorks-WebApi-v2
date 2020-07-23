using AdventureWorks.Dal.Initialization;
using AdventureWorks.Dal.Tests.Base;

namespace AdventureWorks.Dal.Tests.RepoTests.Base
{
    public class RepoTestsBase : TestBase
    {
        public RepoTestsBase()
        {
            SampleDataInitialization.InitializeData(ctx);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}