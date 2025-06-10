// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using EbookMVC.Models;

namespace EbookMVC.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ForgotPasswordModel> _logger;

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender, ILogger<ForgotPasswordModel> logger)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("=== FORGOT PASSWORD REQUEST STARTED ===");
            _logger.LogInformation("Email requested: {Email}", Input?.Email);

            if (ModelState.IsValid)
            {
                _logger.LogInformation("ModelState is valid, searching for user with email: {Email}", Input.Email);

                var user = await _userManager.FindByEmailAsync(Input.Email);

                if (user == null)
                {
                    _logger.LogWarning("User not found with email: {Email}", Input.Email);
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
                _logger.LogInformation("User found: {UserId}, Email confirmed: {IsConfirmed}", user.Id, isEmailConfirmed);

                if (!isEmailConfirmed)
                {
                    _logger.LogWarning("User email not confirmed: {Email}", Input.Email);
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                _logger.LogInformation("Generating password reset token for user: {UserId}", user.Id);
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                // Sử dụng HTTPS cho production, HTTP cho development
                var protocol = Request.IsHttps ? "https" : "http";
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: protocol);

                _logger.LogInformation("Sending password reset email to: {Email}", Input.Email);
                _logger.LogInformation("Reset URL: {CallbackUrl}", callbackUrl);

                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Đặt lại mật khẩu",
                    $"Vui lòng đặt lại mật khẩu của bạn bằng cách <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>nhấp vào đây</a>.");

                _logger.LogInformation("Password reset email sent successfully to: {Email}", Input.Email);
                return RedirectToPage("./ForgotPasswordConfirmation");
            }
            else
            {
                _logger.LogWarning("ModelState is invalid for forgot password request");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning("ModelState error: {Error}", error.ErrorMessage);
                }
            }

            return Page();
        }
    }
}
