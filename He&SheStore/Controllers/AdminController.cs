using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using He_SheStore.Models;
using Microsoft.AspNetCore.Http;

namespace He_SheStore.Controllers
{
	public class AdminController : Controller
	{
		private readonly ModelContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public AdminController(IWebHostEnvironment webHostEnvironment, ModelContext context)
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
        decimal adminId = HttpContext.Session.GetInt32("AdminID") ?? 0; // Default value if not found
        var user = _context.Customers.FirstOrDefault(c => c.CustomerId == adminId);

        if (adminId != 0)
        {
         // Set ViewData for use in the view
         ViewData["FirstName"] = user.Fname;
         ViewData["LastName"] = user.Lname;
         ViewData["ProfilePicture"] = user.Profilepicture;

         // Pass user information, other data, and ViewData to the view using a tuple
         var model = Tuple.Create(
             category as IEnumerable<Category>,
             product as IEnumerable<Product>,
             customer as IEnumerable<Customer>,
             testimonial as IEnumerable<Testmonial>,
             user.Fname,
             user.Lname,
             user.Profilepicture);

        return View(model);
    }

    // Handle if user details are not found in the database
    return RedirectToAction("Login", "LoginandRegister");
}





        public IActionResult JoinTables()
        {
            var products = _context.Products.ToList();
            var categories = _context.Categories.ToList();

            var categoryProducts = from cat in categories
                                   join p in products on cat.CategoryId equals p.CategoryId
                                   select new JoinTables { Category = cat, Product = p };

            return View(categoryProducts.ToList());
        }

        public IActionResult ProductReviews()
        {
            var products = _context.Products.ToList();
            var reviews = _context.Reviews.ToList();
            var customers = _context.Customers.ToList();
            var categories= _context.Categories.ToList();

            var productReviews = (from product in products
                                  join category in categories on product.CategoryId equals category.CategoryId into categoryGroup
                                  from category in categoryGroup.DefaultIfEmpty()  // Left join
                                  join review in reviews on product.ProductId equals review.ProductId
                                  join customer in customers on review.CustomerId equals customer.CustomerId
                                  select new JoinTablesReview { Product = product, Review = review, Customer = customer, Category = category }).ToList();


            return View(productReviews);
        }

        public async Task<IActionResult> ContactMessages()
		{
            return _context.Contacts != null ?
                         View(await _context.Contacts.ToListAsync()) :
                         Problem("Entity set 'ModelContext.Contacts'  is null.");
            
		}

        public async Task<IActionResult> TestimonialsMessages()
        {
            var testimonials = await _context.Testmonials
                .Include(t => t.Customer)
                .ToListAsync();

            var userLogins = await _context.UserLogins
                .Include(u => u.Customer)
                .ToListAsync();

            var model = new Tuple<IEnumerable<Testmonial>, IEnumerable<UserLogin>>(testimonials, userLogins);

            return View(model);
        }




        public async Task<IActionResult> DeleteMessages(decimal id )
        {
            if (_context.Contacts == null)
            {
                return Problem("Entity set 'ModelContext.Contacts'  is null.");
            }
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Admin");
        }


  //      public async Task<IActionResult> AccountInfo()
  //      {
  //          // Retrieve the logged-in AdminId from the session
  //          decimal adminId = HttpContext.Session.GetInt32("AdminID") ?? 0;

		//	var customer = await _context.Customers.FindAsync(adminId);

		//	if (customer == null)
		//	{
		//		return NotFound();
		//	}

		//	return View(customer);
		//}
		// GET: Customers/Edit/
		public async Task<IActionResult> AccountInfo()
		{
			// Retrieve the logged-in UserId from the session
			decimal adminId = HttpContext.Session.GetInt32("AdminID") ?? 0;

			// Fetch the existing user details from the database
			var customer = await _context.Customers.FindAsync(adminId);

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
					decimal adminId = HttpContext.Session.GetInt32("AdminID") ?? 0;
					// Fetch the existing user details from the database
					var existingUser = await _context.Customers.FindAsync(adminId);

					if (existingUser == null)
					{
						return NotFound("User not found");
					}

					// Ensure the userId from the session matches the customerId in the form
					if (adminId != customer.CustomerId)
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

	}
}
