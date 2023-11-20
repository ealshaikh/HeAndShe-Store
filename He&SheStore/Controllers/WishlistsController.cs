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
    public class WishlistsController : Controller
    {
        private readonly ModelContext _context;

        public WishlistsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Wishlists
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Wishlists.Include(w => w.Customer).Include(w => w.Product);
            return View(await modelContext.ToListAsync());
        }

        // GET: Wishlists/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Wishlists == null)
            {
                return NotFound();
            }

            var wishlist = await _context.Wishlists
                .Include(w => w.Customer)
                .Include(w => w.Product)
                .FirstOrDefaultAsync(m => m.Wishlistid == id);
            if (wishlist == null)
            {
                return NotFound();
            }

            return View(wishlist);
        }

        // GET: Wishlists/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
            return View();
        }

        // POST: Wishlists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Wishlistid,CustomerId,ProductId")] Wishlist wishlist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wishlist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", wishlist.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", wishlist.ProductId);
            return View(wishlist);
        }

        // GET: Wishlists/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Wishlists == null)
            {
                return NotFound();
            }

            var wishlist = await _context.Wishlists.FindAsync(id);
            if (wishlist == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", wishlist.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", wishlist.ProductId);
            return View(wishlist);
        }

        // POST: Wishlists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Wishlistid,CustomerId,ProductId")] Wishlist wishlist)
        {
            if (id != wishlist.Wishlistid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wishlist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WishlistExists(wishlist.Wishlistid))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", wishlist.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", wishlist.ProductId);
            return View(wishlist);
        }

        // GET: Wishlists/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Wishlists == null)
            {
                return NotFound();
            }

            var wishlist = await _context.Wishlists
                .Include(w => w.Customer)
                .Include(w => w.Product)
                .FirstOrDefaultAsync(m => m.Wishlistid == id);
            if (wishlist == null)
            {
                return NotFound();
            }

            return View(wishlist);
        }

        // POST: Wishlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Wishlists == null)
            {
                return Problem("Entity set 'ModelContext.Wishlists'  is null.");
            }
            var wishlist = await _context.Wishlists.FindAsync(id);
            if (wishlist != null)
            {
                _context.Wishlists.Remove(wishlist);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WishlistExists(decimal id)
        {
          return (_context.Wishlists?.Any(e => e.Wishlistid == id)).GetValueOrDefault();
        }

		//public IActionResult Wishlist()
		//{
		//	try
		//	{
		//		var currentUserId = HttpContext.Session.GetInt32("UserId");

		//		if (currentUserId != null)
		//		{
		//			var wishlistItems = _context.Wishlists
		//				.Where(w => w.CustomerId == currentUserId)
		//				.Include(w => w.Product) // Include Product information
		//				.Select(w => w.Product) // Select the products from the wishlist
		//				.ToList();

		//			return View(wishlistItems);
		//		}

		//		TempData["ErrorMessage"] = "User not logged in."; // Set an error message if the user is not logged in
		//		return RedirectToAction("Login", "LoginandRegister");
		//	}
		//	catch (Exception ex)
		//	{
		//		// Log the exception
		//		TempData["ErrorMessage"] = "An error occurred while retrieving the wishlist items.";
		//		return RedirectToAction("Index", "UserHome");
		//	}
		//}

		public IActionResult Wishlist()
		{
			try
			{
				var currentUserId = HttpContext.Session.GetInt32("UserId");

				if (currentUserId != null)
				{
					var wishlistItems = _context.Wishlists
						.Where(w => w.CustomerId == currentUserId)
						.Include(w => w.Product)
						.ToList();

					return View(wishlistItems);
				}

				TempData["ErrorMessage"] = "User not logged in.";
				return RedirectToAction("Login", "LoginandRegister");
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "An error occurred while retrieving the wishlist items.";
				// Log the exception
				Console.WriteLine(ex);
				return View("Error");
			}
		}

		//public IActionResult AddToWishlist(decimal productId)
		//{
		//	try
		//	{
		//		var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);

		//		if (product != null)
		//		{
		//			var currentUserId = HttpContext.Session.GetInt32("UserId");

		//			if (currentUserId != null)
		//			{
		//				try
		//				{
		//					// Check if the product is already in the wishlist for the current user
		//					var isAlreadyInWishlist = _context.Wishlists.Any(w => w.ProductId == productId && w.CustomerId == currentUserId);

		//					if (isAlreadyInWishlist)
		//					{
		//						TempData["Message"] = "Product is already in your wishlist.";
		//					}
		//					else
		//					{
		//						var wishlistItem = new Wishlist
		//						{
		//							ProductId = productId,
		//							CustomerId = currentUserId.Value
		//						};

		//						_context.Wishlists.Add(wishlistItem);
		//						_context.SaveChanges();

		//						TempData["Message"] = "Product added to your wishlist!";
		//					}

		//					return RedirectToAction("Wishlist");
		//				}
		//				catch (Exception ex)
		//				{
		//					TempData["ErrorMessage"] = "Error occurred while adding the product to the wishlist.";
		//					// Log the exception for debugging purposes
		//					Console.WriteLine(ex);
		//					return View("Error");
		//				}
		//			}
		//			else
		//			{
		//				TempData["ErrorMessage"] = "User not logged in.";
		//				return RedirectToAction("Login", "LoginandRegister");
		//			}
		//		}
		//		else
		//		{
		//			TempData["ErrorMessage"] = "Product not found.";
		//			return View("Error");
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		TempData["ErrorMessage"] = "An error occurred while processing your request.";
		//		// Log the exception for debugging purposes
		//		Console.WriteLine(ex);
		//		return View("Error");
		//	}
		//}

		public IActionResult AddToWishlist(decimal productId)
		{
			try
			{
				var currentUserId = HttpContext.Session.GetInt32("UserId");

				if (currentUserId != null)
				{
					var isAlreadyInWishlist = _context.Wishlists.Any(w => w.ProductId == productId && w.CustomerId == currentUserId);

					if (isAlreadyInWishlist)
					{
						TempData["Message"] = "Product is already in your wishlist.";
					}
					else
					{
						var wishlistItem = new Wishlist
						{
							ProductId = productId,
							CustomerId = currentUserId.Value
						};

						_context.Wishlists.Add(wishlistItem);
						_context.SaveChanges();

						TempData["Message"] = "Product added to your wishlist!";
					}

					return RedirectToAction("Wishlist");
				}
				TempData["ErrorMessage"] = "User not logged in.";
				return RedirectToAction("Login", "LoginandRegister");
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "An error occurred while adding the product to the wishlist.";
				// Log the exception for debugging purposes
				Console.WriteLine(ex);
				return View("Error");
			}
		}

		public async Task<IActionResult> RemoveFromWishList(decimal id)
		{
			var wishListItem = await _context.Wishlists.FindAsync(id); // Find the wishlist item by WishlistId

			if (wishListItem == null)
			{
				return NotFound();
			}

			_context.Wishlists.Remove(wishListItem);
			await _context.SaveChangesAsync();

			return RedirectToAction("Wishlist", "Wishlists");
		}



	}
}
