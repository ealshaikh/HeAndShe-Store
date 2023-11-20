using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using He_SheStore.Models;

namespace He_SheStore.Controllers
{
    public class OrderaddressesController : Controller
    {
        private readonly ModelContext _context;

        public OrderaddressesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Orderaddresses
        public async Task<IActionResult> Index()
        {
              return _context.Orderaddresses != null ? 
                          View(await _context.Orderaddresses.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Orderaddresses'  is null.");
        }

        // GET: Orderaddresses/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Orderaddresses == null)
            {
                return NotFound();
            }

            var orderaddress = await _context.Orderaddresses
                .FirstOrDefaultAsync(m => m.OrderAddressId == id);
            if (orderaddress == null)
            {
                return NotFound();
            }

            return View(orderaddress);
        }

        // GET: Orderaddresses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orderaddresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderAddressId,Fname,Lname,Country,City,Streetnumber,Postalcode,Phonenumber,Email")] Orderaddress orderaddress)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderaddress);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orderaddress);
        }

        // GET: Orderaddresses/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Orderaddresses == null)
            {
                return NotFound();
            }

            var orderaddress = await _context.Orderaddresses.FindAsync(id);
            if (orderaddress == null)
            {
                return NotFound();
            }
            return View(orderaddress);
        }

        // POST: Orderaddresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("OrderAddressId,Fname,Lname,Country,City,Streetnumber,Postalcode,Phonenumber,Email")] Orderaddress orderaddress)
        {
            if (id != orderaddress.OrderAddressId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderaddress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderaddressExists(orderaddress.OrderAddressId))
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
            return View(orderaddress);
        }

        // GET: Orderaddresses/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Orderaddresses == null)
            {
                return NotFound();
            }

            var orderaddress = await _context.Orderaddresses
                .FirstOrDefaultAsync(m => m.OrderAddressId == id);
            if (orderaddress == null)
            {
                return NotFound();
            }

            return View(orderaddress);
        }

        // POST: Orderaddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Orderaddresses == null)
            {
                return Problem("Entity set 'ModelContext.Orderaddresses'  is null.");
            }
            var orderaddress = await _context.Orderaddresses.FindAsync(id);
            if (orderaddress != null)
            {
                _context.Orderaddresses.Remove(orderaddress);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderaddressExists(decimal id)
        {
          return (_context.Orderaddresses?.Any(e => e.OrderAddressId == id)).GetValueOrDefault();
        }
    }
}
