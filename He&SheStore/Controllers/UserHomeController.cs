using He_SheStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Net.Mail;
using System.Net.Mime;
using System.Xml.Linq;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using iText.Kernel.Pdf.Canvas.Draw;

namespace He_SheStore.Controllers
{
	public class UserHomeController : Controller
	{
		private readonly ModelContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public UserHomeController(IWebHostEnvironment webHostEnvironment, ModelContext context)
		{
			_webHostEnvironment = webHostEnvironment;
			_context = context;
		}

		public IActionResult Index()
		{
			var category = _context.Categories.ToList();
			var product = _context.Products.ToList();
			var customer = _context.Customers.ToList();
			var testimonial = _context.Testmonials.ToList();

			// Retrieve user information from session
			decimal customerId = HttpContext.Session.GetInt32("UserId") ?? 0; // Default value if not found
			var user = _context.Customers.FirstOrDefault(c => c.CustomerId == customerId);

			if (user != null)
			{
				// Set ViewData for use in the view
				ViewData["FirstName"] = user.Fname;
				ViewData["LastName"] = user.Lname;
				ViewData["ProfilePicture"] = user.Profilepicture;

				// Pass user information and other data to the view using a tuple
				var model3 = Tuple.Create(
					category as IEnumerable<Category>,
					product as IEnumerable<Product>,
					customer as IEnumerable<Customer>,
					testimonial as IEnumerable<Testmonial>,
					user.Fname,
					user.Lname,
					user.Profilepicture);

				return View(model3);
			}

			// Handle if user details are not found in the database
			return RedirectToAction("Login", "LoginandRegister");
		}



		public IActionResult Privacy2()
		{
			return View();
		}

		public IActionResult About2()
		{
			return View();
		}


		public IActionResult Contact2()
		{
			return View();
		}

		public IActionResult FAQ2()
		{
			return View();
		}


		//user adding testmonials page
		public IActionResult UserAddTestiminal()
		{
			//check user login to work based on the id
			decimal customerId = HttpContext.Session.GetInt32("UserId") ?? 0; // Default value if not found
			if (customerId != null)
			{
				return View();
			}
			return RedirectToAction("Index", "UserHome");
		}
		public async Task<IActionResult> TestmonialsNavBar()
		{
			decimal customerId = HttpContext.Session.GetInt32("UserId") ?? 0;
			var user = _context.Customers.FirstOrDefault(c => c.CustomerId == customerId);

			if (user != null)
			{
				// Get testimonials
				var testimonials = _context.Testmonials.ToList();
				var clients = _context.Customers.ToList();
				var Models = Tuple.Create<IEnumerable<Testmonial>, IEnumerable<Customer>>(testimonials, clients);
				return View(Models);
			}

			// Handle if something went wrong
			return RedirectToAction("Index", "UserHome");
		}

		public async Task<IActionResult> Details2(decimal? id)
		{
			if (id == null || _context.Products == null)
			{
				return NotFound();
			}

			var product = await _context.Products.Include(p => p.Reviews).FirstOrDefaultAsync(m => m.ProductId == id);

			if (product == null)
			{
				return NotFound();
			}

			var reviews = product.Reviews.ToList();
			var customers = _context.Customers.ToList();


			decimal customerId = HttpContext.Session.GetInt32("UserId") ?? 0;
			var user = _context.Customers.FirstOrDefault(c => c.CustomerId == customerId);

			//if (user != null)
			//{
			//ViewData["ProfilePicture"] = user.Profilepicture;

			// Calculate average rating
			if (reviews.Any())
			{
				product.AverageRating = reviews.Any() ? Math.Round(reviews.Average(r => r.Reating).GetValueOrDefault(), 2) : (decimal?)null;

			}
			else
			{
				product.AverageRating = null;
			}

			// Convert List<Review> to IEnumerable<Review>
			IEnumerable<Review> reviewsEnumerable = reviews;
			List<Review> reviewsList = reviewsEnumerable.ToList();

			var model = Tuple.Create(product, reviewsList, user.CustomerId);


			return View(model);
		}

		//public async Task<IActionResult> Details2(decimal? id)
		//{
		//	if (id == null || _context.Products == null)
		//	{
		//		return NotFound();
		//	}

		//	var product = await _context.Products.Include(p => p.Reviews).FirstOrDefaultAsync(m => m.ProductId == id);

		//	if (product == null)
		//	{
		//		return NotFound();
		//	}

		//	var reviews = product.Reviews.ToList();

		//	decimal customerId = HttpContext.Session.GetInt32("UserId") ?? 0;
		//	var user = _context.Customers.FirstOrDefault(c => c.CustomerId == customerId);

		//	if (user != null)
		//	{
		//		ViewData["ProfilePicture"] = user.Profilepicture;

		//		// Calculate average rating
		//		if (reviews.Any())
		//		{
		//			product.AverageRating = reviews.Average(r => r.Reating);
		//		}
		//		else
		//		{
		//			product.AverageRating = null;
		//		}

		//		// Convert List<Review> to IEnumerable<Review>
		//		IEnumerable<Review> reviewsEnumerable = reviews;

		//		var model = Tuple.Create(product, reviews, user.Profilepicture, user.CustomerId);

		//		return View(model);
		//	}

		//	// Handle if user details are not found in the database
		//	return RedirectToAction("Index", "UserHome");
		//}

		public IActionResult Categories2()
		{
			var categories = _context.Categories.ToList();
			return View(categories);
		}

		public IActionResult Products2()
		{
			var category = _context.Categories.ToList();
			var products = _context.Products.Include(p => p.Reviews).ToList();

			foreach (var product in products)
			{
				if (product.Reviews.Any())
				{
					product.AverageRating = product.Reviews.Average(r => r.Reating);
				}
				else
				{
					product.AverageRating = null;
				}
			}

			var model3 = Tuple.Create<IEnumerable<Category>, IEnumerable<Product>>(category, products);
			return View(model3);
		}



		public IActionResult GetProductByCategory2(int id)
		{
			//var products = _context.Products.Where(p => p.CategoryId == id).ToList();
			//return View(products);
			var products = _context.Products
				.Where(p => p.CategoryId == id)
				.Include(p => p.Reviews) // Include Reviews for each product
				.ToList();

			// Calculate average rating for each product
			foreach (var product in products)
			{
				if (product.Reviews.Any())
				{
					product.AverageRating = product.Reviews.Average(r => r.Reating);
				}
				else
				{
					product.AverageRating = null;
				}
			}

			return View(products);

		}



		// GET: Customers/Edit/
		public async Task<IActionResult> AccountInfo()
		{
			// Retrieve the logged-in UserId from the session
			decimal userId = HttpContext.Session.GetInt32("UserId") ?? 0;

			// Fetch the existing user details from the database
			var customer = await _context.Customers.FindAsync(userId);

			if (customer == null)
			{
				return NotFound();
			}

			return View(customer);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AccountInfo(Customer customer)
		{
			if (ModelState.IsValid)
			{
				try
				{
					// Retrieve the logged-in UserId from the session
					decimal userId = HttpContext.Session.GetInt32("UserId") ?? 0;

					// Fetch the existing user details from the database
					var existingUser = await _context.Customers.FindAsync(userId);

					if (existingUser == null)
					{
						return NotFound("User not found");
					}

					// Ensure the userId from the session matches the customerId in the form
					if (userId != customer.CustomerId)
					{
						return BadRequest("Unauthorized access");
					}

					// Update the user details
					existingUser.Fname = customer.Fname;
					existingUser.Lname = customer.Lname;
					existingUser.Ph = customer.Ph;
					existingUser.Birthdate = customer.Birthdate;

					if (customer.ImageFile != null)
					{
						// Handle image upload
						string wwwRootPath = _webHostEnvironment.WebRootPath;
						string fileName = Guid.NewGuid().ToString() + "_" + customer.ImageFile.FileName;
						string path = Path.Combine(wwwRootPath, "Images", fileName);

						using (var fileStream = new FileStream(path, FileMode.Create))
						{
							await customer.ImageFile.CopyToAsync(fileStream);
						}

						// Update the user's profile picture path
						existingUser.Profilepicture = fileName;
					}

					// Update the user in the database
					_context.Update(existingUser);
					await _context.SaveChangesAsync();

					return RedirectToAction(nameof(Index));
				}
				catch (DbUpdateConcurrencyException)
				{
					return NotFound("User not found");
				}
			}

			return View(customer);
		}

		public IActionResult CheckLoginStatus()
		{
			bool isLoggedIn = HttpContext.Session.GetString("IsLoggedIn") == "true";
			return Json(isLoggedIn);
		}


		//user adding ThankYou
		public IActionResult ThanksReview()
		{
			return View();
		}

		//user adding Testimonial
		public IActionResult ThanksTestimonial()
		{
			return View();
		}



		public IActionResult Checkout(string isReady)
		{
			// Get the user ID from the session
			decimal userId = HttpContext.Session.GetInt32("UserId") ?? 0;

			if (userId != 0)
			{
				if (isReady == "true")
				{
					// Display the Checkout view
					return View();
				}

				// Check the number of items in the user's cart
                var cartItems = _context.Carts
				.Where(x => x.CustomerId == userId)
				.Include(x => x.Product)
				.ToList();

                if (cartItems !=null && cartItems.Any())
				{
                   


                    // Fetch the existing user details from the database
                    var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == userId);

					if (customer != null)
					{
						// Create an Orderaddress object and pre-fill it with user details
						Orderaddress orderaddress = new Orderaddress
						{
							Fname = customer.Fname,
							Lname = customer.Lname,
							Phonenumber = customer.Ph // Assuming 'Ph' is the property for phone number in the Customer model
													  // Add other properties as needed
						};

						// Pass the pre-filled Orderaddress to the view
						return View(orderaddress);
					}
					else
					{
						// If user details are not retrieved, show an error message or redirect as needed
						return Json(new { error = "An error occurred while retrieving user details" });
					}
				}
				else
				{
					// If the cart is empty and not ready for payment, return an error message
					return Json(new { error = "Must Have One Product Or More to Check Out" });
				}
			}

			// If the user is not logged in, redirect to the home page
			return RedirectToAction("Login", "LoginandRegister");
		}




		//	[HttpPost]
		//	[ValidateAntiForgeryToken]
		//	public async Task<IActionResult> Checkout([Bind("Id,Fname,Lname,Email,Country,City,Streetaddress,Postalcode,Phonenumber")] Orderaddress orderaddress, string HolderName, long CardNumber, DateTime Expiration, byte Cvv)
		//	{
		//		decimal userId = HttpContext.Session.GetInt32("UserId") ?? 0; // Get the user ID from the session

		//		if (userId != 0)
		//		{

		//			var userLogin = _context.UserLogins
		//.Include(ul => ul.Cards)  // Include the Cards navigation property
		//.FirstOrDefault(ul => ul.UserloginId == userId);

		//			if (userLogin != null)
		//			{
		//				// Find the card associated with the provided details
		//				var cardAccount = userLogin.Cards
		//					.Where(x => x.CardholderName == HolderName && x.CardCvc == Cvv && x.ExpiryDate == Expiration && x.CardNumber == CardNumber)
		//					.SingleOrDefault();

		//				if (cardAccount == null)
		//				{
		//					Console.WriteLine("Card not found with the specified details.");
		//				}

		//				var UserCart = _context.Carts.Include(c => c.Product).Where(x => x.CustomerId == userId).ToList();
		//				var total = UserCart.Sum(x => (x.Product.ProductPrice * x.Quantity));

		//				if (total < cardAccount.Balance)
		//				{
		//					cardAccount.Balance -= (decimal)total;
		//					_context.Cards.Update(cardAccount);
		//					await _context.SaveChangesAsync();

		//					Orderaddress address = new Orderaddress();
		//					address.Fname = orderaddress.Fname;
		//					address.Lname = orderaddress.Lname;
		//					address.Email = orderaddress.Email;
		//					address.Country = orderaddress.Country;
		//					address.City = orderaddress.City;
		//					address.Streetnumber = orderaddress.Streetnumber;
		//					address.Phonenumber = orderaddress.Phonenumber;
		//					address.Postalcode = orderaddress.Postalcode;
		//					_context.Orderaddresses.Add(address);
		//					await _context.SaveChangesAsync();

		//					Order order = new Order();
		//					order.CustomerId = userId;
		//					order.Status = "Pending";
		//					order.Orderdate = DateTime.Now;
		//					order.Totalamount = (decimal)total;
		//					order.OrderAddressId = address.OrderAddressId;
		//					_context.Orders.Add(order);
		//					await _context.SaveChangesAsync();

		//					foreach (var item in UserCart)
		//					{
		//						var product = _context.Products.Where(c => c.ProductId == item.ProductId).SingleOrDefault();
		//						Orderitem orderitem = new Orderitem();
		//						orderitem.OrderId = order.Orderid;
		//						orderitem.ProductId = item.ProductId;
		//						orderitem.Quantitiy = (decimal)item.Quantity;
		//						orderitem.ItemPrice = (decimal)item.Product.ProductPrice;
		//						product.StockQuantity -= item.Quantity;

		//						if (product.StockQuantity == 0)
		//						{
		//							product.ProductStatus = "Out of Stock";
		//							var carts = _context.Carts.ToList();
		//							foreach (var item_cart in carts)
		//							{
		//								if (item_cart.ProductId == product.ProductId && item_cart.CustomerId != userId)
		//								{
		//									_context.Carts.Remove(item_cart);
		//									await _context.SaveChangesAsync();
		//								}
		//							}
		//						}
		//						else
		//						{
		//							var carts = _context.Carts.ToList();
		//							foreach (var item_cart in carts)
		//							{
		//								if (item_cart.ProductId == product.ProductId && item_cart.CustomerId != userId)
		//								{
		//									if (item_cart.Quantity > product.StockQuantity)
		//									{
		//										item_cart.Quantity = product.StockQuantity;
		//										_context.Carts.Update(item_cart);
		//										await _context.SaveChangesAsync();
		//									}
		//								}
		//							}
		//						}
		//						_context.Products.Update(product);
		//						_context.Orderitems.Add(orderitem);
		//					}

		//					await _context.SaveChangesAsync();

		//					foreach (var item in UserCart)
		//					{
		//						_context.Carts.Remove(item);
		//					}

		//					await _context.SaveChangesAsync();

		//					TempData["CorrectPay"] = "Payment successful!";
		//					return RedirectToAction("Checkout", "UserHome", new { isReady = "true" });
		//				}
		//				else
		//				{
		//					ViewBag.Total = total;
		//					TempData["ErrorBalanceAccount"] = "Balance Account not enough to Check Out ";
		//					return View();
		//				}
		//			}
		//			//else
		//			//{
		//			//	var cartUser = _context.Carts.Include(c => c.Product).Where(x => x.CustomerId == userId).ToList();
		//			//	ViewBag.Total = cartUser.Sum(x => (x.Product.ProductPrice * x.Quantity));
		//			//	TempData["ErrorCardAccount"] = "The card information is incorrect ";
		//			//	return View();
		//			//}
		//			else
		//			{
		//				var userLogin1 = _context.UserLogins
		//					.Include(ul => ul.Cards)
		//					.FirstOrDefault(ul => ul.UserloginId == userId);

		//				if (userLogin != null && userLogin.Cards.Any())
		//				{
		//					var cartUser = _context.Carts.Include(c => c.Product).Where(x => x.CustomerId == userId).ToList();
		//					ViewBag.Total = cartUser.Sum(x => (x.Product.ProductPrice * x.Quantity));
		//					// Continue with the rest of your logic
		//					return View();
		//				}
		//				else
		//				{
		//					TempData["ErrorCardAccount"] = "The card information is incorrect or missing.";
		//					// Redirect or handle the scenario where the card information is incorrect or missing
		//					return View();
		//				}
		//			}

		//		}
		//		else
		//		{
		//			var cartUser = _context.Carts.Include(c => c.Product).Where(x => x.CustomerId == userId).ToList();
		//			var totalAmount = cartUser.Sum(x => (x.Product.ProductPrice * x.Quantity));
		//			ViewBag.Total = totalAmount;
		//			return View(orderaddress);
		//		}
		//	}


	}
}
