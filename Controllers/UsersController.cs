using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ikvm.runtime;
using Microsoft.Owin.Security;
using School_Manager.Models;


namespace School_Manager.Controllers
{
    public class UsersController : Controller
    {
       

        managerSchoolAccount db = new managerSchoolAccount();
        private ClaimsIdentity email;

        //GET: Users
        public ActionResult Index()
        {
            return View();
          
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
       
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var check = db.Users.FirstOrDefault(s => s.Email == user.Email);
                if (check == null)
                {
                    user.CreateOn = DateTime.Now;
                    user.Status = 3;
                    user.Password = GetMD5(user.Password);
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Users.Add(user);
                    db.SaveChanges();
                    //return RedirectToAction("Index");
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }
            }

            return View();
        }
 
        // GET: Account
        [AllowAnonymous]
        [HttpGet]
     
        public ActionResult Login(string returnUrl)
        {
            try
            {
                //Verification
                if (this.Request.IsAuthenticated)   
                {
                    //Info
                    return this.RedirectToLocal(returnUrl);
                }
            }
            catch(Exception ex)
            {
                //info
                Console.Write(ex);
            }
            //Info
            return this.View(); ;
        }
       
        [HttpPost]
        public  ActionResult Login(LoginModel model, string returnUrl)
        {
            try
            {
                //Verification
                if (ModelState.IsValid)
                {
                    //Initialization
            //var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) }, ClaimTypes.Role);
            //        identity.AddClaim(new Claim(ClaimTypes.Email, email));
                    var loginInfo = db.Users.Where(x => x.Email.ToUpper() == model.Email.ToUpper() && x.Password == model.Password).ToList();
                    //Verification
                    if(loginInfo != null && loginInfo.Count() > 0)
                    {
                        Session["Email"] = loginInfo.FirstOrDefault().Email;
                        //Initialization
                        var loginDetails = loginInfo.First();
                        // LoginIn
                        this.SignInUser(loginDetails.Email, false);
                        //Info 
                        return RedirectToAction("Index", "Students");
                    }
                    else
                    {
                        //Setting
                        ModelState.AddModelError(string.Empty, "Invalid email or password");
                   
                    }
                    
                }

            }
            catch(Exception ex)
            {
                //Info
                Console.Write(ex);

            }

            //if we got this far, something failed, redisplay form
            return this.View(model);
           
        }
       
        private void SignInUser(string Email, bool isPersistent)
        {
            //Initialization
                
            var claims = new List<Claim> {
            new Claim(ClaimTypes.Role, "AdminManager"),
            new Claim(ClaimTypes.Role, "Teacher"),
            new Claim(ClaimTypes.Role,"Customer")
            };
            
            
            try
            {     
                //Settings
 
                claims.Add(new Claim(ClaimTypes.Email, Email));
                var claimIdenties = new ClaimsIdentity(claims);
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;
                //sign In
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, claimIdenties);
            }
            catch (Exception ex)
            {
                //info
                throw ex;

            }
        }
        private void ClaimIdentities(string email)
        {
            var claims = new List<Claim>();
            try
            {
                claims.Add(new Claim(ClaimTypes.Email, email));
                var ClaimIdentities = new ClaimsIdentity(claims);

            }
            catch(Exception ex)
            {
                throw ex;

            }

        }
        //[ValidateAntiForgeryToken]
        //public ActionResult Login(string email, string password)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var f_password = GetMD5(password);
        //        var data = db.Users.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
        //        if (data.Count() > 0)
        //        {
        //            //add session
        //            Session["FullName"] = data.FirstOrDefault().FirstName + " " + data.FirstOrDefault().LastName;
        //            Session["Email"] = data.FirstOrDefault().Email;
        //            Session["idUser"] = data.FirstOrDefault().ID;
        //            Session["userStatus"] = data.FirstOrDefault().Status;
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            ViewBag.error = "Login failed";
        //            return RedirectToAction("Login");
        //        }
        //    }

        //    return View();
        //}
        //public ActionResult UserDashBoard()
        //{
        //    if (Session["ID"] != null)
        //    {
        //        return View();
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login");
        //    }
        //}






        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            try
            {
                //Verification
                if (Url.IsLocalUrl(returnUrl))
                {
                    //info
                    return this.Redirect(returnUrl);

                }
            }
            catch(Exception ex)
            {
                //Info
                throw ex;
            }
            //Info
            return this.RedirectToAction("Login", "Users");
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            return RedirectToAction("Index", "Home");
        }



        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Email,Password,ComfirmPassword,CreateOn,Status")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
       
    }
}
