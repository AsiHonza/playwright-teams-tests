using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TeamsTestProject.Base;
using Microsoft.Playwright;

namespace TeamsTestProject.Tests
{
    [TestClass]
    public class LoginTest : BaseTest
    {
        [TestMethod]
        public async Task LoginToTeamsTest()
        {
            Console.WriteLine("Starting login test...");
            var navBar = Page.Locator("[data-testid='title-bar']");
            await navBar.WaitForAsync(new() { State = WaitForSelectorState.Visible });

            var title = await Page.TitleAsync();
            Console.WriteLine($"Page title after login: '{title}'");

            StringAssert.Contains(title, "Chat", "Page title does not contain 'Chat' after login.");
            Assert.IsTrue(await navBar.IsVisibleAsync(), "Navigation bar is not visible after login.");
        }
    }
}
