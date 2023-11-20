using He_SheStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace He_SheStore.Controllers
{
	public class LoginandRegisterController : Controller
	{
		private readonly ModelContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public LoginandRegisterController(ModelContext context, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Login()
		{
			return View();
		}

		public IActionResult Register()
		{
			return View();
		}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Email", "Password")] UserLogin userLogin)
        {
            var auth = _context.UserLogins
                .Where(x => x.Email == userLogin.Email && x.Password == userLogin.Password)
                .SingleOrDefault();

            if (auth != null)
            {
                switch (auth.RoleId)
                {
                    case 1:
                        HttpContext.Session.SetInt32("AdminID", (int)auth.CustomerId);

                        return RedirectToAction("Index", "Admin");

                    case 2:
                        HttpContext.Session.SetInt32("UserId", (int)auth.CustomerId);
                        return RedirectToAction("Index", "UserHome");

                    default:
                        // Log or throw an exception for unexpected RoleId values
                        return RedirectToAction("Login");
                }
            }

            // If the login fails, return to the login page
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Fname,Lname,ImageFile", "Ph", "Birthdate")] Customer customer, string Email, string Password)
        {
            if (ModelState.IsValid)
            {
                //Add customer details
                if (customer.ImageFile != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" +
                    customer.ImageFile.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/",
                    fileName);
                    using (var fileStream = new FileStream(path,
                    FileMode.Create))
                    {
                        await customer.ImageFile.CopyToAsync(fileStream);
                    }
                    customer.Profilepicture = fileName; /*to retrive the image file name path correctly*/
                }


                _context.Add(customer);
                await _context.SaveChangesAsync();

                //Add user login details
                UserLogin login = new UserLogin();
                login.RoleId = 2;
                login.Email = Email;
                login.Password = Password;
                login.CustomerId = customer.CustomerId;
                _context.Add(login);
                await _context.SaveChangesAsync();


                return RedirectToAction("Login");// ازا كلشي صحيح انقلني لهون
            }
            //return View();
            return View("Register");// ازا في ايرورو اتركني بنفس الصفحة
        }





        public IActionResult Logout()
        {
			
			
			HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home"); 
        }
    }
}
