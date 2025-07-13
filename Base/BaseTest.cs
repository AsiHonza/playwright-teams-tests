using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TeamsTestProject.Helpers;
using TeamsTestProject.Pages;

namespace TeamsTestProject.Base
{
    public class BaseTest
    {
        protected IBrowser Browser;
        protected IBrowserContext Context;
        protected IPage Page;

        [TestInitialize]
        public async Task Setup()
        {
            ClearDownloadsFolder();
            var playwright = await Playwright.CreateAsync();
            Browser = await playwright.Chromium.LaunchAsync(new() { Headless = false, SlowMo = 50 });
            Context = await Browser.NewContextAsync(new BrowserNewContextOptions
            {
                AcceptDownloads = true
            });
            Page = await Context.NewPageAsync();

            var config = Config.Load();
            var loginPage = new LoginPage(Page);
            await loginPage.LoginAsync(config.Email, config.Password);
        }

        private void ClearDownloadsFolder()
        {
            var downloadsPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Resources", "Downloads"));
            if (Directory.Exists(downloadsPath))
            {
                foreach (var file in Directory.GetFiles(downloadsPath))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to delete file {file}: {ex.Message}");
                    }
                }
            }
        }

        [TestCleanup]
        public async Task Teardown()
        {
            var screenshotsPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "screenshots"));
            if (TestContext.CurrentTestOutcome == UnitTestOutcome.Failed)
            {
                var screenshotPath = Path.Combine(screenshotsPath, $"{TestContext.TestName}_{DateTime.Now:yyyyMMdd_HH-mm-ss}.png"
);
                await Page.ScreenshotAsync(new() { Path = screenshotPath });
                Console.WriteLine($"Screenshot saved to: {screenshotPath}");
            }
            await Context.CloseAsync();
            await Browser.CloseAsync();
        }

        public TestContext TestContext { get; set; }
    }
}
