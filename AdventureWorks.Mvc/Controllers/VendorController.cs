using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.DomainModels;

namespace AdventureWorks.Mvc.Controllers
{
    public class VendorController : Controller
    {
        private IRepositoryCollection _repository;

        public VendorController(IRepositoryCollection repository)
        {
            _repository = repository;
        }

        public ViewResult List(int? pageNumber, int? pageSize)
        {
            var pagingParams = new VendorParameters {
                PageNumber = pageNumber ?? 1,
                PageSize = pageSize ?? 10
            };
            var pagedList = _repository.Vendor.GetVendors(pagingParams);
            return View(pagedList);
        }

        public ViewResult Details(int id)
        {
            var vendor = _repository.Vendor.GetVendorByID(id);
            return View(vendor);
        }
            
        public ViewResult Create()
        {
            var vendor = new VendorDomainObj { };
            return View(vendor);
        }

        public ViewResult Edit()
        {
            return View();
        }

        public ViewResult Delete()
        {
            return View();
        }
    }
}
