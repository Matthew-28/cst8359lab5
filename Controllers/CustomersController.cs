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

namespace Lab5.Controllers
{
    public class CustomersController : Controller
    {
        private readonly DealsFinderDbContext _context;

        public CustomersController(DealsFinderDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index(int id = 0)
        {
            var viewModel = new DealsViewModel
            {
                Subscriptions = await _context.Subscriptions
                  .AsNoTracking()
                  .Where(i => i.CustomerId == id)
                  .OrderBy(i => i.FoodDeliveryServiceId)
                  .ToListAsync(),

                FoodDeliveryServices = await _context.FoodDeliveryServices
                  .AsNoTracking()
                  .OrderBy(i => i.Id)
                  .ToListAsync(),

                Customers = await _context.Customers
                  .AsNoTracking()
                  .OrderBy(i => i.Id)
                  .ToListAsync()
            };

            return View(viewModel);
            //return View(await _context.Customers.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LastName,FirstName,BirthDate")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LastName,FirstName,BirthDate")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        public async Task<IActionResult> EditSubscriptions(int? id)
        {
            var viewModel = new DealsViewModel
            {
                Customers = await _context.Customers
                  .AsNoTracking()
                  .Where(i => i.Id == id)
                  .ToListAsync(),

                Subscriptions = await _context.Subscriptions
                  .AsNoTracking()
                  .Where(i => i.CustomerId == id)
                  .OrderBy(i => i.FoodDeliveryServiceId)
                  .ToListAsync()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> AddSubscriptions(int customerId, String serviceId)
        {
            _context.Add(new Subscription { CustomerId = customerId, FoodDeliveryServiceId = serviceId });
            await _context.SaveChangesAsync();
            return RedirectToAction("EditSubscriptions", "Customers", new { id = customerId });
        }

        public async Task<IActionResult> RemoveSubscriptions(int customerId, String serviceId)
        {
            var subscription = await _context.Subscriptions.FindAsync(customerId, serviceId);
            if (subscription != null)
            {
                _context.Subscriptions.Remove(subscription);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("EditSubscriptions", "Customers", new { id = customerId });
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
