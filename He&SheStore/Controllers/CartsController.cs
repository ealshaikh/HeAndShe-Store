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
    public class CartsController : Controller
    {
        private readonly ModelContext _context;

        public CartsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Carts.Include(c => c.Customer).Include(c => c.Product);
            return View(await modelContext.ToListAsync());
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Carts == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Customer)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.CartId == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartId,ProductQuantity,TotalPrice,ProductId,CustomerId,Quantity")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", cart.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", cart.ProductId);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Carts == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", cart.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", cart.ProductId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("CartId,ProductQuantity,TotalPrice,ProductId,CustomerId,Quantity")] Cart cart)
        {
            if (id != cart.CartId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.CartId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", cart.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", cart.ProductId);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Carts == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Customer)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.CartId == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Carts == null)
            {
                return Problem("Entity set 'ModelContext.Carts'  is null.");
            }
            var cart = await _context.Carts.FindAsync(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(decimal id)
        {
          return (_context.Carts?.Any(e => e.CartId == id)).GetValueOrDefault();
        }

		public IActionResult MyCart()
		{
			//var currentUserId = HttpContext.Session.GetInt32("UserId");

			//var products = _context.Carts.Where(p => p.CustomerId == currentUserId).Include(x=>x.Product).ToList();

			//return View(products);

			var currentUserId = HttpContext.Session.GetInt32("UserId");

			var cartItems = _context.Carts
				.Where(c => c.CustomerId == currentUserId)
				.Include(x => x.Product)
				.ToList();

			// Convert Cart items to a list of Products to pass to the view
			var products = cartItems.Select(cartItem => cartItem.Product);

			return View(cartItems);
		}


		public IActionResult AddToCart(decimal productId, decimal quantity)
		{
			try
			{
				var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);

				if (product != null)
				{
					var currentUserId = HttpContext.Session.GetInt32("UserId");

					if (currentUserId != null)
					{
						var existingCartItem = _context.Carts.FirstOrDefault(c => c.CustomerId == currentUserId && c.ProductId == product.ProductId);

						if (existingCartItem != null)
						{
							// Product already exists in the cart; increase the quantity
							existingCartItem.ProductQuantity += quantity;
							existingCartItem.TotalPrice = product.ProductPrice * existingCartItem.ProductQuantity;
						}
						else
						{
							// Product not in the cart; add a new entry
							var cartItem = new Cart
							{
								ProductId = product.ProductId,
								ProductQuantity = quantity,
								TotalPrice = product.ProductPrice * quantity,
								CustomerId = (decimal)currentUserId
							};
							_context.Carts.Add(cartItem);
						}

						_context.SaveChanges();

						//return View(); // return my cart redirect
						return RedirectToAction("MyCart", "Carts");

					}

					// Handle if the user is not logged in; maybe redirect to a login page
					return RedirectToAction("Login", "LoginandRegister");
				}

				// Show an error message in the view
				TempData["ErrorMessage"] = "The product is not available.";
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "An error occurred while adding the product to the cart.";
				// Log the exception for debugging purposes: ex.Message
			}

			// Handle errors by showing an error message in the view
			return Json(new { error = "something went wrong" });
		}

		public IActionResult AddToCartWithWishlist(decimal productId)
		{
			try
			{
				var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);

				if (product != null)
				{
					var currentUserId = HttpContext.Session.GetInt32("UserId");

					if (currentUserId != null)
					{
						// Check if the product is in the wishlist
						var wishlistItem = _context.Wishlists.FirstOrDefault(w => w.ProductId == productId && w.CustomerId == currentUserId);

						if (wishlistItem != null)
						{
							// Add the item to the cart
							var existingCartItem = _context.Carts.FirstOrDefault(c => c.CustomerId == currentUserId && c.ProductId == product.ProductId);

							if (existingCartItem != null)
							{
								// Product already exists in the cart; increase the quantity
								existingCartItem.ProductQuantity += 1;
								existingCartItem.TotalPrice = product.ProductPrice * existingCartItem.ProductQuantity;
							}
							else
							{
								// Product not in the cart; add a new entry
								var cartItem = new Cart
								{
									ProductId = product.ProductId,
									ProductQuantity = 1, // Default quantity is 1
									TotalPrice = product.ProductPrice,
									CustomerId = (decimal)currentUserId
								};
								_context.Carts.Add(cartItem);
							}

							// Remove the item from the wishlist
							_context.Wishlists.Remove(wishlistItem);

							_context.SaveChanges();

							return RedirectToAction("MyCart", "Carts");
						}
					}

					// Handle if the user is not logged in or if the product is not in the wishlist
					return RedirectToAction("Index", "UserHome");
				}

				// Show an error message in the view
				TempData["ErrorMessage"] = "The product is not available.";
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "An error occurred while adding the product to the cart.";
				// Log the exception for debugging purposes: ex.Message
			}

			// Handle errors by showing an error message in the view
			return Json(new { error = "something went wrong" });
		}


		public async Task<IActionResult> RemoveFromCart(decimal id)
		{
			var cart = await _context.Carts.FindAsync(id);
			if (cart == null)
			{
				return NotFound();
			}

			_context.Carts.Remove(cart);
			await _context.SaveChangesAsync();

			return RedirectToAction("MyCart", "Carts");
		}

		public async Task<IActionResult> EditOrder(decimal id)
		{
			var cart = await _context.Carts.FindAsync(id);
			if (cart == null)
			{
				return NotFound();
			}



			return RedirectToAction("MyCart", "Carts");
		}


		public IActionResult AddToWishlistFromCart(decimal productId)
		{
			try
			{
				var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);

				if (product != null)
				{
					var currentUserId = HttpContext.Session.GetInt32("UserId");

					if (currentUserId != null)
					{
						// Check if the product is in the cart
						var cartItem = _context.Carts.FirstOrDefault(c => c.ProductId == productId && c.CustomerId == currentUserId);

						if (cartItem != null)
						{
							// Add the item to the wishlist
							var wishlistItem = new Wishlist
							{
								ProductId = product.ProductId,
								CustomerId = (decimal)currentUserId
							};

							_context.Wishlists.Add(wishlistItem);

							// Remove the item from the cart
							_context.Carts.Remove(cartItem);

							_context.SaveChanges();

							return RedirectToAction("Wishlist", "Wishlists");
						}
					}

					// Handle if the user is not logged in or if the product is not in the cart
					return RedirectToAction("MyCart", "Carts");
				}

				// Show an error message in the view
				TempData["ErrorMessage"] = "The product is not available.";
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "An error occurred while adding the product to the wishlist.";
				// Log the exception for debugging purposes: ex.Message
			}

			// Handle errors by showing an error message in the view
			return Json(new { error = "something went wrong" });
		}






		public IActionResult IncrementQuantity(int id)
		{
			var cartItem = _context.Carts.Include(c => c.Product).SingleOrDefault(c => c.CartId == id);

			if (cartItem != null && cartItem.Product != null)
			{

				cartItem.ProductQuantity += 1;
				cartItem.TotalPrice = cartItem.Product.ProductPrice * cartItem.ProductQuantity;
				_context.SaveChanges(); // Save changes to the database

			}

			var updatedModel = _context.Carts.Include(c => c.Product).ToList(); // Retrieve the updated model

			return RedirectToAction("MyCart");
		}




		public IActionResult DecrementQuantity(int id)
		{
			var cartItem = _context.Carts.Include(c => c.Product).FirstOrDefault(c => c.CartId == id);

			if (cartItem != null && cartItem.Product != null)
			{
				if (cartItem.ProductQuantity > 1)
				{
					cartItem.ProductQuantity -= 1;
					cartItem.TotalPrice = cartItem.Product.ProductPrice * cartItem.ProductQuantity;
					_context.SaveChanges(); // Save changes to the database
				}
			}

			var updatedModel = _context.Carts.Include(c => c.Product).ToList(); // Retrieve the updated model

			return RedirectToAction("MyCart");
		}
        
	}
}
