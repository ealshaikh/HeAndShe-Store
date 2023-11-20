using He_SheStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;

namespace He_SheStore.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ModelContext _context;

		public HomeController(ILogger<HomeController> logger, ModelContext context)
		{
			_logger = logger;
			_context = context;
		}

		public IActionResult Index()
		{
			var testimonials = _context.Testmonials.ToList();
			var clients = _context.Customers.ToList();
			var categories = _context.Categories.ToList();

			// Query to get top 6 existing in-stock products
			var featuredProducts = _context.Products
				.Where(p => p.StockQuantity > 0)  // Only in-stock products
				.OrderByDescending(p => p.ProductId)
				.Take(6)
				.ToList();

			var model3 = Tuple.Create<IEnumerable<Category>, IEnumerable<Product>, IEnumerable<Testmonial>, IEnumerable<Customer>>(
				categories, featuredProducts, testimonials, clients);

			return View(model3);
		}


		public IActionResult Privacy()
		{
			return View();
		}

		public IActionResult About()
		{
			return View();
		}


		public IActionResult Contact()
		{
			return View();
		}


		public IActionResult FAQ()
		{
			return View();
		}
		public IActionResult Testmonials()
		{
			// Get testimonials
			var testimonials = _context.Testmonials.ToList();
			var clients = _context.Customers.ToList();
			var Models = Tuple.Create < IEnumerable < Testmonial >, IEnumerable<Customer>>(testimonials, clients);
			return View(Models);
		}


		public IActionResult Categories()
		{
			var categories = _context.Categories.ToList();
			return View(categories);
		}

		public IActionResult Products()
		{
			//var products = _context.Products.ToList();
			//return View(products);

			//var category = _context.Categories.ToList();
			//var product = _context.Products.ToList();
			//var model3 = Tuple.Create<IEnumerable<Category>,
			//IEnumerable<Product>>(category, product);
			//return View(model3);
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


		public IActionResult GetProductByCategory(int id)
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



		public async Task<IActionResult> Details(decimal? id)
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

			//Calculate average rating
			if (reviews.Any())
			{
				product.AverageRating = reviews.Average(r => r.Reating);
			}
			else
			{
				product.AverageRating = null;
			}

			// Convert List<Review> and List<Customer> to IEnumerable<Review> and IEnumerable<Customer>
			IEnumerable<Review> reviewsEnumerable = reviews;
			IEnumerable<Customer> customersEnumerable = customers;

			var model = Tuple.Create(product, reviewsEnumerable, customersEnumerable);

			return View(model);
		}









		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		
	}
}