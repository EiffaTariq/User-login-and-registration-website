//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Data.SqlClient;
//using UserLoginAndRegistration.Models;

//namespace UserLoginAndRegistration.Controllers
//{
//    public class AccountController : Controller
//    {
//        private string conn = "Data Source=(localdb)\\ProjectModels;Initial Catalog=User;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

//        private UserRepository _userRepository;

//        public AccountController(UserRepository userRepository)
//        {
//            _userRepository = userRepository;
//        }
//        public IActionResult Index()
//        {
//            return View();
//        }


//        public IActionResult Login()
//        {
//            return View();
//        }




//        [HttpPost]

//        public IActionResult Login(string Email, string Password)
//        {
//            if (User != null)
//            {
//                string message = string.Empty;

//                if (HttpContext.Request.Cookies.ContainsKey("CreateDate"))
//                {
//                    message = "You visit us at" + Request.Cookies["CreateDate"];
//                }
//                else
//                {
//                    CookieOptions cookieOptions = new CookieOptions();
//                    cookieOptions.Secure = true;
//                    cookieOptions.Expires = DateTime.Now.AddDays(1);
//                    HttpContext.Response.Cookies.Append("CreateDate", DateTime.Now.ToString(), cookieOptions);
//                    message = "you visit us first time";

//                }
//                return View("Index", message);
//            }
//            UserRepository ur = new UserRepository();
//            bool IsEmailExists = ur.IsEmailExists(Email);
//            if (IsEmailExists)
//            {
//                return RedirectToAction("Login", "User");
//            }
//            else
//            {
//                ViewBag.Message = "Email does not exist";
//                return View();
//            }

//        }

//        [HttpGet]
//        public IActionResult Register()
//        {
//            return View();
//        }

//        [HttpPost]
//        public IActionResult Register(User user)
//        {
//            UserRepository ur = new UserRepository();
//            if (ur.IsEmailExists(user.Email))
//            {
//                ViewBag.Message = "Email already exists";
//                return View();
//            }
//            ur.AddUser(user);
//            ViewBag.Message = "User registered successfully";
//            return View();
//        }


//        // CRUD Operations
//        public IActionResult UserDetails(int id)
//        {
//            UserRepository ur = new UserRepository();
//            var user = ur.getUserById(id);
//            return View(user);
//        }

//        public IActionResult EditUser(int id)
//        {
//            UserRepository ur = new UserRepository();
//            var user = ur.getUserById(id);
//            return View(user);
//        }

//        [HttpPost]
//        public IActionResult EditUser(User user)
//        {
//            UserRepository ur = new UserRepository();
//            ur.updateUser(user);
//            ViewBag.Message = "User updated successfully";
//            return View(user);
//        }

//        [HttpPost]
//        public IActionResult DeleteUser(int id)
//        {
//            UserRepository ur = new UserRepository();
//            ur.deleteUser();
//            return RedirectToAction("Index", "Home");
//        }

//        public IActionResult Logout()
//        {
//            Response.Cookies.Delete("UserEmail");
//            return RedirectToAction("Login");
//        }
//    }
//}

////[HttpGet]
////public IActionResult Login()
////{
////    return View();
////}
////[HttpPost]
////public IActionResult Login(string email, string password)
////{
////    UserRepository ur = new UserRepository();
////    User User = new User();
////    ur.GetUsers(email, password);

////    string msg = String.Empty;
////    if (User != null)
////    {
////        msg = "You visited us at " + HttpContext.Response.Cookies.Append("CreateDate");
////        return RedirectToAction("index", "Home");
////    }
////    ViewBag.Message = "Invalid credentials";
////    return View();
////}

////[HttpPost]
////public IActionResult Login(string email, string password)
////{
////    User u = new User();
////    var user = _userRepository.GetUsers(email, password);
////    if (user != null)
////    {
////        // Set login cookie
////        Response.Cookies.Append("UserEmail", u.Email);
////        return RedirectToAction("Index", "Home");
////    }
////    ViewBag.Message = "Invalid credentials";
////    return View();
////}

using Microsoft.AspNetCore.Mvc;
using UserLoginAndRegistration.Models;
using System;

namespace UserLoginAndRegistration.Controllers
{
    public class AccountController : Controller
    {
        private  UserRepository _userRepository;

        public AccountController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (_userRepository.IsEmailExists(email))
            {
                User u = new User();
                var user = _userRepository.GetUsers(email, password);
                if (user != null)
                {
                    // Set a login cookie
                    Response.Cookies.Append("UserEmail", u.Email); // Store needed info

                    return RedirectToAction("Index", "Home"); // Redirect upon successful login
                }
                ViewBag.Message = "Invalid credentials";
            }
            else
            {
                ViewBag.Message = "Email does not exist";
            }

            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (_userRepository.IsEmailExists(user.Email))
            {
                ViewBag.Message = "Email already exists";
                return View();
            }

            _userRepository.AddUser(user);
            ViewBag.Message = "User registered successfully";

            return RedirectToAction("Login");
        }

        // CRUD Operations
        public IActionResult UserDetails(int id)
        {
            var user = _userRepository.getUserById(id);
            return View(user);
        }

        public IActionResult EditUser(int id)
        {
            var user = _userRepository.getUserById(id);
            return View(user);
        }

        [HttpPost]
        public IActionResult EditUser(User user)
        {
            _userRepository.updateUser(user);
            ViewBag.Message = "User updated successfully";
            return View(user);
        }

        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            _userRepository.deleteUser(id);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("UserEmail");
            return RedirectToAction("Login");
        }
    }
}
