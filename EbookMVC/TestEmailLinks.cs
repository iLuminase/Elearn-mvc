using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;

namespace EbookMVC
{
    /// <summary>
    /// Test class để kiểm tra việc tạo URL cho reset password và email confirmation
    /// </summary>
    public class TestEmailLinks
    {
        public static void TestUrlGeneration()
        {
            Console.WriteLine("=== TEST URL GENERATION ===");
            
            // Simulate code generation
            var code = "test-reset-code-123";
            var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            
            Console.WriteLine($"Original code: {code}");
            Console.WriteLine($"Encoded code: {encodedCode}");
            
            // Test URL patterns
            var baseUrl = "https://localhost:7187";
            var resetPasswordUrl = $"{baseUrl}/Identity/Account/ResetPassword?area=Identity&code={encodedCode}";
            var confirmEmailUrl = $"{baseUrl}/Identity/Account/ConfirmEmail?area=Identity&userId=test-user-id&code={encodedCode}&returnUrl=%2F";
            
            Console.WriteLine($"Reset Password URL: {resetPasswordUrl}");
            Console.WriteLine($"Confirm Email URL: {confirmEmailUrl}");
            
            // Test HTML encoding
            var htmlEncodedResetUrl = HtmlEncoder.Default.Encode(resetPasswordUrl);
            var htmlEncodedConfirmUrl = HtmlEncoder.Default.Encode(confirmEmailUrl);
            
            Console.WriteLine($"HTML Encoded Reset URL: {htmlEncodedResetUrl}");
            Console.WriteLine($"HTML Encoded Confirm URL: {htmlEncodedConfirmUrl}");
            
            // Test email content
            var resetEmailContent = $"Vui lòng đặt lại mật khẩu của bạn bằng cách <a href='{htmlEncodedResetUrl}'>nhấp vào đây</a>.";
            var confirmEmailContent = $"Please confirm your account by <a href='{htmlEncodedConfirmUrl}'>clicking here</a>.";
            
            Console.WriteLine("\n=== EMAIL CONTENT ===");
            Console.WriteLine("Reset Password Email:");
            Console.WriteLine(resetEmailContent);
            Console.WriteLine("\nConfirm Email:");
            Console.WriteLine(confirmEmailContent);
            
            Console.WriteLine("\n=== TEST COMPLETED ===");
        }
    }
}
