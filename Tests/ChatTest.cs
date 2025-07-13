using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TeamsTestProject.Base;
using TeamsTestProject.Pages;

namespace TeamsTestProject.Tests
{
    [TestClass]
    public class ChatTest : BaseTest
    {
        private ChatPage _chatPage;

        [TestInitialize]
        public void Init()
        {
            _chatPage = new ChatPage(Page);
        }

        [TestMethod]
        public async Task FullChatFlowTest()
        {

            await _chatPage.UploadUniqueFileAsync();

            await _chatPage.DownloadLastFileFromChatAsync();

            const string messagePrefix = "Hello, this is a test message number ";
            await _chatPage.SendMultipleMessagesAsync(messagePrefix);

            var lastMessageText = await _chatPage.GetLastChatMessageTextAsync();
            Assert.IsTrue(lastMessageText.Contains("test message number 3"), "Last message was not sent correctly.");
        }
    }
}
