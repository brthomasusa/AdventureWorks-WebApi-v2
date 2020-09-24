using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        public async Task<IActionResult> List([FromQuery] int? pageNumber, int? pageSize)
        {
            var pagingParams = new VendorParameters
            {
                PageNumber = pageNumber ?? 1,
                PageSize = pageSize ?? 10
            };
            var pagedList = await _repository.Vendor.GetVendors(pagingParams);
            return View(pagedList);
        }

        public async Task<IActionResult> Details(int id)
        {
            var vendor = await _repository.Vendor.GetVendorByID(id);
            if (vendor == null)
            {
                return NotFound();
            }

            return View(vendor);
        }

        public IActionResult Create()
        {
            var creditRatingLookup = CreditRatingLookupCollection.CreditRatingStatuses();
            ViewBag.CreditRatingLookup = creditRatingLookup;

            return View(new VendorDomainObj { });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vendor = await _repository.Vendor.GetVendorByID(id);

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
        public async Task<IActionResult> Edit([FromForm] VendorDomainObj vendor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.Vendor.UpdateVendor(vendor);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(VendorDomainObj), ex.Message);
                    var creditRatingLookup = CreditRatingLookupCollection.CreditRatingStatuses();
                    ViewBag.CreditRatingLookup = creditRatingLookup;
                    return View(vendor);
                }

            }
            else
            {
                return View(vendor);
            }

            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var vendor = await _repository.Vendor.GetVendorByID(id);
            if (vendor == null)
            {
                return NotFound();
            }

            return View(vendor);
        }
    }
}
