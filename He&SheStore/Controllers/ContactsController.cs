//using MailKit.Search;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using He_SheStore.Models;
using System.Diagnostics.Contracts;
using System.Net.Mail;

namespace He_SheStore.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ModelContext _context;

        public ContactsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Contacts
        public async Task<IActionResult> Index()
        {
              return _context.Contacts != null ? 
                          View(await _context.Contacts.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Contacts'  is null.");
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.ContactId == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContactId,Messeage,Email,Fullname,Title")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }


        
		[HttpPost]
		public IActionResult SendContact(string name, string email, string subject, string message)
		{
			Contact contact = new Contact();
			contact.Fullname = name;
            contact.Email = email;
            contact.Title=subject;
            contact.Messeage = message;

			_context.Contacts.Add(contact);
           
			_context.SaveChanges();
			TempData["SuccessMessage"] = "The Message Was Sent Successfully";







			//MimeMessage message_x = new MimeMessage();
			//MailboxAddress from = new MailboxAddress(contact.Fullname, contact.Email);
			//message_x.From.Add(from);
			//MailboxAddress to = new MailboxAddress("Admin", "esraaalshaikh200@gmail.com");
			//message_x.To.Add(to);
			//message_x.Subject = "Arrival of the request";
			//BodyBuilder builder = new BodyBuilder();
			//builder.HtmlBody = $"<h3>Thank you!</h3>" +
			//				 $"<p>Name: {contact.Fullname}</p>" +
			//				 $"<p>Subject: {contact.Title}</p>" +
			//				 $"<p>User Email: {contact.Email}</p>" +
			//				 $"<p>Message: {contact.Messeage}</p>";
			//message_x.Body = builder.ToMessageBody();

			//using (var clinte = new SmtpClient())
			//{
			//	clinte.Connect("smtp.gmail.com", 465, true);
			//	clinte.Authenticate("esraaalshaikh200@gmail.com", "cvbnmdfgdfg");
			//	clinte.Send(message_x);
			//	clinte.Disconnect(true);
			//}

			return RedirectToAction("Index", "Home");
		}

		
		// GET: Contacts/Edit/5
		public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("ContactId,Messeage,Email,Fullname,Title")] Contact contact)
        {
            if (id != contact.ContactId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.ContactId))
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
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.ContactId == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
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
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(decimal id)
        {
          return (_context.Contacts?.Any(e => e.ContactId == id)).GetValueOrDefault();
        }
    }
}
