using Microsoft.Playwright;
using System.Threading.Tasks;
using TeamsTestProject.Helpers;
using static Microsoft.Playwright.Assertions;

namespace TeamsTestProject.Pages
{
    public class LoginPage
    {
        private readonly IPage _page;

        public LoginPage(IPage page)
        {
            _page = page;
        }

        // Locators for elements on the login page
        private ILocator EmailInput => _page.Locator("input[type='email']");
        private ILocator PasswordInput => _page.Locator("input[type='password']");
        private ILocator SubmitButton => _page.Locator("#idSIButton9");
        private ILocator StaySignedInNoButton => _page.Locator("#idBtn_Back"); 

        // Methods to interact with the login page
        public async Task NavigateAsync()
        {
            var config = Config.Load();
            await _page.GotoAsync(config.BaseUrl);
        }

        public async Task EnterEmailAsync(string email)
        {
            await EmailInput.FillAsync(email);
            await SubmitButton.ClickAsync();
            await Expect(PasswordInput).ToBeVisibleAsync(new() { Timeout = 10000 });
            Console.WriteLine("Email entered and submit button clicked, waiting for password input to be visible.");
        }

        public async Task EnterPasswordAsync(string password)
        {
            await PasswordInput.FillAsync(password);
            await SubmitButton.ClickAsync();
        }

        public async Task StaySignedInAsync()
        {
            await StaySignedInNoButton.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            if (await StaySignedInNoButton.IsVisibleAsync())
            {
                await StaySignedInNoButton.ClickAsync();
            }
        }

        public async Task LoginAsync(string email, string password)
        {
            await NavigateAsync();
            await EnterEmailAsync(email);
            await EnterPasswordAsync(password);
            await StaySignedInAsync();
        }
    }
}
