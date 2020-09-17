using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.DomainModels;
using AdventureWorks.Mvc.Views.Shared.LookupData;
using AdventureWorks.Mvc.Infrastructure;
using LoggerService;

namespace AdventureWorks.Mvc.Controllers
{
    public class VendorController : Controller
    {
        private IRepositoryCollection _repository;

        public VendorController(IRepositoryCollection repository)
        {
            _repository = repository;
        }

        public IActionResult List([FromQuery] int? pageNumber, int? pageSize)
        {
            var pagingParams = new VendorParameters
            {
                PageNumber = pageNumber ?? 1,
                PageSize = pageSize ?? 10
            };
            var pagedList = _repository.Vendor.GetVendors(pagingParams);
            return View(pagedList);
        }

        public IActionResult Details(int id)
        {
            var vendor = _repository.Vendor.GetVendorByID(id);
            if (vendor == null)
            {
                return NotFound();
            }

            return View(vendor);
        }

        public ViewResult Create()
        {
            var creditRatingLookup = CreditRatingLookupCollection.CreditRatingStatuses();
            ViewBag.CreditRatingLookup = creditRatingLookup;

            return View(new VendorDomainObj { });
        }

        public IActionResult Edit(int id)
        {
            var vendor = _repository.Vendor.GetVendorByID(id);

            if (vendor == null)
            {
                return NotFound();
            }

            var creditRatingLookup = CreditRatingLookupCollection.CreditRatingStatuses();
            ViewBag.CreditRatingLookup = creditRatingLookup;

            return View(vendor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromForm] VendorDomainObj vendor)
        {
            if (ModelState.IsValid)
            {
                _repository.Vendor.UpdateVendor(vendor);
            }
            else
            {
                return View(vendor);
            }

            return RedirectToAction(nameof(List));
        }

        public IActionResult Delete(int id)
        {
            var vendor = _repository.Vendor.GetVendorByID(id);
            if (vendor == null)
            {
                return NotFound();
            }

            return View(vendor);
        }
    }
}
