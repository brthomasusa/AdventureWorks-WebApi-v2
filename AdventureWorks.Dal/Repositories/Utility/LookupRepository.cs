using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;
using System.Threading.Tasks;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Models.Base;
using LoggerService;

namespace AdventureWorks.Dal.Repositories.Utility
{
    public class LookupRepository : ILookupRepository
    {
        private const string CLASSNAME = "LookupRepository";
        protected AdventureWorksContext DbContext { get; }
        protected ILoggerManager RepoLogger { get; }

        private LookupItem _firstItem = new LookupItem("0", "-- Not Selected --");

        private readonly string[] _regions = { "AS", "AU", "CA", "DE", "FM", "FR", "GB", "MH", "MP", "PW", "US", "VI" };

        public LookupRepository(AdventureWorksContext context, ILoggerManager logger)
        {
            DbContext = context;
            RepoLogger = logger;
        }

        public async Task<List<LookupItem>> GetDepartmentLookupItems()
        {
            var lookupItems = await DbContext.Department.Select(dept => new LookupItem(
                dept.DepartmentID.ToString(), dept.Name
            )).OrderBy(lkup => lkup.Text).ToListAsync();

            lookupItems.Insert(0, _firstItem);
            return lookupItems;
        }

        public async Task<List<LookupItem>> GetAddressTypeLookupItems()
        {
            var lookupItems = await DbContext.AddressType.Select(atype => new LookupItem(
                atype.AddressTypeID.ToString(), atype.Name
            )).OrderBy(lkup => lkup.Text).ToListAsync();

            lookupItems.Insert(0, _firstItem);
            return lookupItems;
        }

        public async Task<List<LookupItem>> GetContactTypeLookupItems()
        {
            var lookupItems = await DbContext.ContactType.Select(ctype => new LookupItem(
                ctype.ContactTypeID.ToString(), ctype.Name
            )).OrderBy(lkup => lkup.Text).ToListAsync();

            lookupItems.Insert(0, _firstItem);
            return lookupItems;
        }

        public async Task<List<LookupItem>> GetStateProvinceIdByRegionLookupItems(string regionCode)
        {
            if (!string.IsNullOrWhiteSpace(regionCode) && _regions.Contains(regionCode))
            {
                var lookupItems = await DbContext.StateProvince
                    .Where(st => st.CountryRegionCode.Trim() == regionCode.Trim())
                    .Select(st => new LookupItem(st.StateProvinceID.ToString(), st.StateProvinceCode))
                    .OrderBy(lkup => lkup.Text).ToListAsync();

                lookupItems.Insert(0, _firstItem);
                return lookupItems;
            }

            string msg = $"Error: Invalid country code: '{regionCode}', unable to filter state code list.";
            RepoLogger.LogError(CLASSNAME + ".GetStateProvinceIdByRegionLookupItems " + msg);
            throw new AdventureWorksNullEntityObjectException(msg);

        }
    }
}