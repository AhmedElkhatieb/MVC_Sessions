using IKEA.BLL.Services.EmailSettings;
using IKEA.DAL.Models.Identity;
using IKEA.PL.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IKEA.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSettings _emailSettings;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSettings emailSettings) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSettings = emailSettings;
        }
        #region Register
        #region GET
        [HttpGet]
        public async Task<IActionResult> SignUp()
        {
            return View();
        }
        #region POST
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var existingUser = await _userManager.FindByNameAsync(signUpViewModel.UserName);
            if (existingUser != null)
            {
                ModelState.AddModelError(nameof(signUpViewModel.UserName), "This User Name Is Already Exist");
                return View(signUpViewModel);
            }
            var User = new ApplicationUser()
            {
                FirstName = signUpViewModel.FirstName,
                LastName = signUpViewModel.LastName,
                UserName = signUpViewModel.UserName,
                Email = signUpViewModel.Email,
                IsAgree = signUpViewModel.IsAgree
            };
            var Result = await _userManager.CreateAsync(User, signUpViewModel.Password);
            if (Result.Succeeded)
            {
                return RedirectToAction(nameof(SignIn));
            }
            foreach(var error in Result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(signUpViewModel);
        }
        #endregion
        #endregion
        #endregion

        #region Login
        #region GET
        [HttpGet]
        public async Task<IActionResult> SignIn()
        {
            return View();
        }
        #endregion
        #region POST
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel signInViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var User = await _userManager.FindByEmailAsync(signInViewModel.Email);
            if (User is { })
            {
                var flag = await _userManager.CheckPasswordAsync(User, signInViewModel.Password);
                if (flag)
                {
                    var result = await _signInManager.PasswordSignInAsync(User, signInViewModel.Password, signInViewModel.RememberMe, true);
                    if (result.IsNotAllowed)
                    {
                        ModelState.AddModelError(string.Empty, "Your Account Is Not Confirmed Yet");
                    }
                    if (result.IsLockedOut)
                    {
                        ModelState.AddModelError(string.Empty, "Your Account Has Been Locked");
                    }
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }   
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid Login Credentials");
            return View(signInViewModel);
        }
        #endregion
        #endregion

        #region Logout
        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
        #endregion

        #region Forget Password
        #region GET
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        #endregion
        #region POST
        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordViewModel forgetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(forgetPasswordViewModel.Email);
                if (User is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(User);
                    var url = Url.Action("ResetPassword", "Account", new {email = forgetPasswordViewModel.Email, Token = token}, Request.Scheme);
                    // To Subject Body
                    var email = new Email()
                    {
                        To = forgetPasswordViewModel.Email,
                        Subject = "Reset Your Password",
                        Body = url
                    };
                    //Send Email
                    _emailSettings.SendEmail(email);
                    return RedirectToAction("CheckYourInbox");
                }
                ModelState.AddModelError(string.Empty, "Invalid Operation, Please Try Again");
            }
            return View(forgetPasswordViewModel);
        }
        #endregion
        #endregion

        #region Check Your Inbox
        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }
        #endregion

        #region Reset Password
        //New Password - Confirm Password
        #region GET
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            // Pass email , token
            return View();
        }
        #endregion
        #region POST
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;
                var User = await _userManager.FindByEmailAsync(email);
                if (User is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(User, token, resetPasswordViewModel.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(SignIn));
                    }
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid Operation, Please Try Again");
            return View(resetPasswordViewModel);
        }
        #endregion
        #endregion
    }
}
