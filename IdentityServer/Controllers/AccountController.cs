using Expendiq.Application.Dtos;
using Expendiq.Application.ViewModels;
using Expendiq.Domain.Entities.Users;
using Expendiq.Domain.Enums.User;
using IdentityServer.IdentityExtensions.Email;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Web;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace IdentityServer.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly IEmailSender _emailSender;
        private const string returnUrlToBeRemoved = "https://www.gmail.com";

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IIdentityServerInteractionService interactionService,
                                 IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interactionService = interactionService;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            returnUrl = Request.QueryString.ToString();
            if (String.IsNullOrEmpty(returnUrl))
            {
                returnUrl = returnUrlToBeRemoved; // change this to application home page.
                return Redirect(returnUrl);
            }
            LoginViewModel vm = new LoginViewModel { ReturnUrl = returnUrl };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user;

                if (Regex.IsMatch(loginViewModel.UserName, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
                {
                    user = await _userManager.FindByEmailAsync(loginViewModel.UserName);
                }
                else
                {
                    user = await _userManager.FindByNameAsync(loginViewModel.UserName.ToLower());
                }

                if (user is null)
                {
                    ModelState.AddModelError(nameof(LoginViewModel.UserName), "Incorrect Username or Password");
                    return View(loginViewModel);
                }
                if (string.IsNullOrEmpty(loginViewModel.ReturnUrl))
                {
                    ModelState.AddModelError(nameof(LoginViewModel.Password), "Return Url Can not be null");
                    return Redirect(returnUrlToBeRemoved);
                }

                string redirectUri = GetRedirectUri(loginViewModel.ReturnUrl);

                SignInResult checkSignInResult = await _signInManager.CheckPasswordSignInAsync(user, loginViewModel.Password, false);
                if (!checkSignInResult.Succeeded)
                {
                    ModelState.AddModelError(nameof(LoginViewModel.UserName), "Incorrect Username or Password");
                    return View(loginViewModel);
                }

                if (checkSignInResult.Succeeded)
                {
                    if (user.EmailConfirmed is false)
                    {
                        return RedirectToAction(nameof(VerifyEmail), new { returnUrl = loginViewModel.ReturnUrl });
                    }
                }

                SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                if (signInResult.Succeeded)
                {
                    return Redirect(loginViewModel.ReturnUrl);
                }
            }
            else
            {
                ModelState.AddModelError(nameof(LoginViewModel.UserName), "Incorrect Username or Password");
                return View(loginViewModel);
            }
            return View(loginViewModel);
        }

        private static string GetRedirectUri(string returnUrl)
        {
            string[] parameters = returnUrl.Split("?");

            string[] splitedParameters = parameters[1].Split("&");

            string[] clientIdParameter = splitedParameters[3].Split("=");

            return HttpUtility.UrlDecode(clientIdParameter[1]);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            RegisterViewModel vm = new()
            {
                ReturnUrl = returnUrl,
            };
            return View(vm);
        }

        private async Task<string> GenerateUserNameForIdentityUser(string fullName)
        {
            fullName = fullName.Trim().ToLower();
            var splitedName = fullName.Split(" ");

            // Store the first possible name.
            var possibleUsername = string.Join("", splitedName.First(), splitedName.Last());

            // ✅ Check if username exists using UserManager (already injected)
            var existingUser = await _userManager.FindByNameAsync(possibleUsername);
            if (existingUser == null)
            {
                return possibleUsername;
            }

            // Find the first available username with a number suffix
            for (var i = 1; i <= 100; i++)
            {
                var newUsername = string.Join("", splitedName.First(), splitedName.Last(), i);
                var userExists = await _userManager.FindByNameAsync(newUsername);
                if (userExists == null)
                {
                    return newUsername;
                }
            }

            // Fallback - generate unique username with GUID
            return $"{possibleUsername}_{Guid.NewGuid().ToString().Substring(0, 5)}";
        }

        public static ModelStateDictionary RegisterValidation(ModelStateDictionary modelStateDictionary, RegisterViewModel registerViewModel)
        {
            if (registerViewModel.FullName is null)
            {
                modelStateDictionary.AddModelError(nameof(RegisterViewModel.FullName), "Full Name is required.");
            }
            if (registerViewModel.ContactNumber is null)
            {
                modelStateDictionary.AddModelError(nameof(RegisterViewModel.ContactNumber), "Contact Number is required.");
            }
            if (registerViewModel.Email is null)
            {
                modelStateDictionary.AddModelError(nameof(RegisterViewModel.Email), "Email is required.");
            }

            return modelStateDictionary;
        }
        public static string GeneratePassword()
        {

            string password = "";
            string allowedChar = "abcdefghijkmnopqrstuvwxyz";
            password = string.Concat(password, GetRandomChar(allowedChar, allowedChar.Length).Trim());
            allowedChar = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
            password = string.Concat(password, GetRandomChar(allowedChar, allowedChar.Length).Trim());
            allowedChar = "0123456789";
            password = string.Concat(password, GetRandomChar(allowedChar, allowedChar.Length).Trim());
            allowedChar = "@#";
            password = string.Concat(password, GetRandomChar(allowedChar, allowedChar.Length).Trim());
            return password;
        }
        public static string GetRandomChar(string allowedChar, int charcount)
        {
            Random randNum = new Random();
            char[] chars = new char[2];
            for (int i = 0; i < 2; i++)
            {
                chars[i] = allowedChar[(int)((charcount) * randNum.NextDouble())];
            }
            return new string(chars);
        }



        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                RegisterValidation(ModelState, registerViewModel);
                if (!ModelState.IsValid)
                {
                    return View(registerViewModel);
                }

                ApplicationUser user = new()
                {
                    FullName = registerViewModel.FullName,
                    PhoneNumber = registerViewModel.ContactNumber,
                    Email = registerViewModel.Email,
                    MobileNumber = registerViewModel.ContactNumber,
                    Status = UserStatus.Active,
                    UserName = await GenerateUserNameForIdentityUser(registerViewModel.FullName),
                    EntryUserID = 0,
                    EntryDate = DateTime.Now,
                    LockoutEnabled = true,
                    LockoutEnd = null,
                    EmailConfirmed = true  //work on this for
                };

                string generatedPassword = GeneratePassword();

                IdentityResult identityResult = await _userManager.CreateAsync(user, generatedPassword);


                if (identityResult.Succeeded)
                {

                    string subject = "Customer Registration";
                    string emailContent = $"Username: {user.UserName} Password: {generatedPassword}";

                    DataResult result = await _emailSender.SendEmailAsync(user.Email, subject, emailContent, user.FullName ?? "User");
                    if (result.Success)
                    {
                        return View(nameof(InitialRegistration), new InitialRegistrationViewModel { ReturnUrl = registerViewModel.ReturnUrl });
                    }
                    else
                    {

                        ModelState.AddModelError(nameof(RegisterViewModel.Email), "Something went wrong. We are unable to send email.");
                        return View(nameof(Register), new RegisterViewModel() { ReturnUrl = registerViewModel.ReturnUrl });
                    }
                }
                else
                {
                    ModelState.AddModelError(nameof(RegisterViewModel.Email), string.Join(",", identityResult.Errors.Select(x => x.Description).ToList()));
                    return View(nameof(Register), new RegisterViewModel() { ReturnUrl = registerViewModel.ReturnUrl });
                }

            }
            return View(registerViewModel);
        }

        [HttpGet]
        public IActionResult InitialRegistration(InitialRegistrationViewModel initialRegistrationViewModel)
        {
            return View(initialRegistrationViewModel);
        }

        [HttpGet]
        public IActionResult VerifyEmail(VerifyEmailViewModel verifyEmailViewModel)
        {
            return View(verifyEmailViewModel);
        }

        [HttpGet]
        public IActionResult ResendEmailVerification(string returnUrl)
        {
            ResendEmailVerificationViewModel vm = new ResendEmailVerificationViewModel { ReturnUrl = returnUrl };
            return View(vm);
        }


        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();

            var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);

            if (string.IsNullOrEmpty(logoutRequest.PostLogoutRedirectUri))
            {
                return Redirect(returnUrlToBeRemoved);
                //return RedirectToAction("Index", "Home");
            }

            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }

    }
}
