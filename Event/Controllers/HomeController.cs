using AspNetCoreHero.ToastNotification.Abstractions;
using Event.Data;
using Event.Models;
using Event.ViewModals;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NuGet.Protocol.Plugins;
using System.Diagnostics;
using System.Security.Claims;

namespace Event.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment WebHostEnvironment;
        private readonly INotyfService _notyf;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        private AppDbContext Context { get; }

        public HomeController(AppDbContext _context, IWebHostEnvironment webHostEnvironment, INotyfService notyf, ILogger<HomeController> logger)
        {
            this.Context = _context;
            _notyf = notyf;
            _logger = logger;
            WebHostEnvironment = webHostEnvironment;
        }
        [Authorize]

        public IActionResult Index()
        {

            return View(Context.Events.ToList());
        }
        [Authorize]

        public IActionResult Privacy()
        {
            return View(Context.Events.ToList());
        }
        [Authorize]

        public IActionResult AddEvent(EventsVM events)
        {
            string stringFile = upload(events);
            var data = new Eventss
            {
                Name = events.Name,
                Venue = events.Venue,
                Price = events.Price,
                Time = events.Time,
                Image = stringFile
            };
            this.Context.Events.Add(data);
            this.Context.SaveChanges();
            _notyf.Success("Event Inserted Successfully");
            return RedirectToAction("Index");
        }
        [Authorize]

        public IActionResult EditEvent(int id)
        {
            var data = Context.Events.Find(id);
            return View(data);
        }
        [Authorize]

        [HttpPost]

        public IActionResult UpdateEvent(EventsVM update)
        {
            string stringFile = upload(update);
            if (update.Image != null)
            {
                var data = Context.Events.Find(update.Id);
                string delDir = Path.Combine(WebHostEnvironment.WebRootPath, "Images", data.Image);
                FileInfo f1 = new FileInfo(delDir);
                if (f1.Exists)
                {
                    System.IO.File.Delete(delDir);
                    f1.Delete();
                }
                data.Name = update.Name;
                data.Venue = update.Venue;
                data.Price = update.Price;
                data.Time = update.Time;
                data.Image = stringFile;
                Context.SaveChanges();
            }

            if (update.Image == null)
            {
                var data = Context.Events.Find(update.Id);
                data.Name = update.Name;
                data.Venue = update.Venue;
                data.Price = update.Price;
                data.Time = update.Time;
                Context.SaveChanges();

            }
            _notyf.Success("Updated Successfully");

            return RedirectToAction("Index");
        }
        [Authorize]
        public IActionResult Delete(int id)
        {
            var data = Context.Events.Find(id);
            Context.Events.Remove(data);
            Context.SaveChanges();
            _notyf.Warning("Deleted Successfully");
            return RedirectToAction("Index");
        }
        [Authorize]
        private string upload(EventsVM s)
        {
            string fileName = "";
            if (s.Image != null)
            {
                string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "Images");
                fileName = Guid.NewGuid().ToString() + "-" + s.Image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    s.Image.CopyTo(fileStream);
                }
            }
            return fileName;
        }
        [Authorize]
        public IActionResult BookEvent(int id)
        {
            var data = Context.Events.Find(id);
            return View(data);
        }
        [Authorize]

        public IActionResult Registeruser()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginVM login)
        {

            if (ModelState.IsValid)
            {
                var data = Context.Users.Where(e => e.Email == login.Email).SingleOrDefault();
                if (data != null)
                {
                    bool isValid = (data.Email == login.Email && BCrypt.Net.BCrypt.Verify(login.Password, data.Password) );
                    if (isValid)
                    {
                        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, login.Email) }, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        HttpContext.Session.SetString("Username", login.Email);
                        HttpContext.Session.SetInt32("Id", data.Id);
                        return RedirectToAction("Privacy");
                    }
                    else
                    {
                        TempData["password"] = "Password Incorrect";
                        return View(login);
                    }
                }
                else
                {
                    TempData["Email"] = "Email not Found";
                    return View(login);
                }
            }
            else
            {
                TempData["errorMessage"] ="Found";
                return View(login);
            }
        }
        [Authorize]

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult SignUp()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(Register register)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(register.Password);
            if (ModelState.IsValid)
            {
                var data1 = Context.Users.Where(e => e.Email == register.Email).SingleOrDefault();
                if (data1 == null)
                {
                    var data = new User
                    {
                        Name = register.Name,
                        Email = register.Email,
                        Password = passwordHash,
                        Phone = register.phone,
                        Address = register.Address,
                        Gender = register.Gender,
                    };
                    Context.Users.Add(data);
                    Context.SaveChanges();
                    _notyf.Success("Please Login using same Credentials");
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["errorEmail"] = "Email already registered";
                    return View(register);
                }
            }
            else
            {
                TempData["errorMessage"] = "Empty Form Cannot be Submitted";
                return View(register);
            }
        }
        public IActionResult Book(BookEvent book)
        {
            Context.Bookings.Add(book);
            Context.SaveChanges();
            _notyf.Success("Booked Successfully");
            return RedirectToAction("ViewBook");
        }

        public IActionResult ViewBook()
        {
            return View(Context.Bookings.ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}