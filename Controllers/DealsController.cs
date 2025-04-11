using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab5.Data;
using Lab5.Models;
using Lab5.Models.ViewModels;
using System.Diagnostics;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Lab5.Controllers
{
    public class DealsController : Controller
    {
        private readonly DealsFinderDbContext _context;

        public DealsController(DealsFinderDbContext context)
        {
            _context = context;
        }

        // GET: Deals/Index/A1
        public async Task<IActionResult> Index(string serviceId)
        {
            if (serviceId == null)
            {
                return NotFound();
            }

            var viewModel = new DealsViewModel
            {
                FoodDeliveryServices = await _context.FoodDeliveryServices
                  .AsNoTracking()
                  .Where(i => i.Id == serviceId)
                  .OrderBy(i => i.Id)
                  .ToListAsync(),

                Deals = await _context.Deals
                  .AsNoTracking()
                  .Where(i => i.FoodDeliveryServiceId == serviceId)
                  .OrderBy(i => i.DealId)
                  .ToListAsync()
            };
            return View(viewModel);
            //return View(await _context.FoodDeliveryServices.ToListAsync());
        }

        public async Task<IActionResult> Back()
        {
            return RedirectToAction("Index", "FoodDeliveryServices");
        }

        // GET: Deals/Create
        public async Task<IActionResult> Create(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viewModel = new DealsViewModel
            {
                FoodDeliveryServices = await _context.FoodDeliveryServices
                  .AsNoTracking()
                  .Where(i => i.Id == id)
                  .OrderBy(i => i.Id)
                  .ToListAsync()
            };

            return View(viewModel);
        }

        // POST: Deals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DealId,ImageLink,FoodDeliveryServiceId")] Deal deal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deal);
        }

        // GET: Deals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deal = await _context.Deals
                .FirstOrDefaultAsync(m => m.DealId == id);
            if (deal == null)
            {
                return NotFound();
            }

            return View(deal);
        }

        // POST: Deals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deal = await _context.Deals.FindAsync(id);
            if (deal != null)
            {
                _context.Deals.Remove(deal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "FoodDeliveryServices"); ;
        }

    }
}
