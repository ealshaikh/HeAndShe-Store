using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using He_SheStore.Models;
using Microsoft.CodeAnalysis;

namespace He_SheStore.Controllers
{
    public class TestmonialsController : Controller
    {
        private readonly ModelContext _context;

        public TestmonialsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Testmonials
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Testmonials.Include(t => t.Customer);
            return View(await modelContext.ToListAsync());
        }

        // GET: Testmonials/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Testmonials == null)
            {
                return NotFound();
            }

            var testmonial = await _context.Testmonials
                .Include(t => t.Customer)
                .FirstOrDefaultAsync(m => m.Testmonialid == id);
            if (testmonial == null)
            {
                return NotFound();
            }

            return View(testmonial);
        }

        // GET: Testmonials/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            return View();
        }

        // POST: Testmonials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Testmonialid,Status,Comment,CustomerId")] Testmonial testmonial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testmonial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", testmonial.CustomerId);
            return View(testmonial);
        }

        // GET: Testmonials/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Testmonials == null)
            {
                return NotFound();
            }

            var testmonial = await _context.Testmonials.FindAsync(id);
            if (testmonial == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", testmonial.CustomerId);
            return View(testmonial);
        }

        // POST: Testmonials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Testmonialid,Status,Comment,CustomerId")] Testmonial testmonial)
        {
            if (id != testmonial.Testmonialid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testmonial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestmonialExists(testmonial.Testmonialid))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", testmonial.CustomerId);
            return View(testmonial);
        }

        // GET: Testmonials/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Testmonials == null)
            {
                return NotFound();
            }

            var testmonial = await _context.Testmonials
                .Include(t => t.Customer)
                .FirstOrDefaultAsync(m => m.Testmonialid == id);
            if (testmonial == null)
            {
                return NotFound();
            }

            return View(testmonial);
        }

        // POST: Testmonials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Testmonials == null)
            {
                return Problem("Entity set 'ModelContext.Testmonials'  is null.");
            }
            var testmonial = await _context.Testmonials.FindAsync(id);
            if (testmonial != null)
            {
                _context.Testmonials.Remove(testmonial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestmonialExists(decimal id)
        {
          return (_context.Testmonials?.Any(e => e.Testmonialid == id)).GetValueOrDefault();
        }



        [HttpPost]
        public IActionResult AddTestimonial([Bind("Testmonialid,Status,Comment,CustomerId")] Testmonial testmonial)
        {
            decimal customerId = HttpContext.Session.GetInt32("UserId") ?? 0;

            if (customerId != 0)
            {
                testmonial.CustomerId = customerId;
                testmonial.Status = "Pending"; // Set Status to 'Pending' by default
                _context.Add(testmonial);
                _context.SaveChangesAsync();
                TempData["SuccessReview"] = "The Message Review Was Sent Successfully";

                return RedirectToAction("ThanksTestimonial", "UserHome");
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public IActionResult AcceptTestimonial(decimal testimonialId)
        {
            var testimonial = _context.Testmonials.Find(testimonialId);

            if (testimonial != null)
            {
                testimonial.Status = "Accepted";
                _context.SaveChanges();

				TempData["SuccessMessage"] = "Testimonial accepted successfully";

			}
			else
			{
				TempData["ErrorMessage"] = "Failed to accept testimonial";
			}

			return RedirectToAction("Index","Admin"); // Redirect to the appropriate action
		}




        [HttpPost]
        public async Task<IActionResult> DeleteTestimoniall(decimal id)
        {
            try
            {
                var testmonial = await _context.Testmonials.FindAsync(id);

                if (testmonial != null)
                {
                    _context.Testmonials.Remove(testmonial);
                    await _context.SaveChangesAsync();
                }

                // Return the same view with the updated testimonial list
                var testimonials = await _context.Testmonials.ToListAsync();
                return RedirectToAction("TestimonialsMessages", "Admin", new { id = id });
            }
            catch (Exception ex)
            {
                return Problem($"An error occurred: {ex.Message}");
            }
        }


        [HttpPost]
        public IActionResult RejectTestimonial(decimal testimonialId)
        {
            var testimonial = _context.Testmonials.Find(testimonialId);

            if (testimonial != null)
            {
                testimonial.Status = "Rejected";
                _context.SaveChanges();

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

    }
}
