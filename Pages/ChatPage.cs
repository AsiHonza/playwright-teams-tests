using Microsoft.Playwright;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TeamsTestProject.Pages
{
    public class ChatPage
    {
        private readonly IPage _page;

        public ChatPage(IPage page) => _page = page;

        private ILocator MessageInput => _page.Locator("div[role='textbox'][contenteditable='true'][data-tid='ckeditor']");
        private ILocator SendMessageButton => _page.Locator("button[data-tid='sendMessageCommands-send']");
        private ILocator PlusMenuButton => _page.Locator("[data-tid='sendMessageCommands-message-extension-flyout-command']");
        private ILocator AttachmentButton => _page.Locator("[data-tid='flyout-list-item']").First;
        private ILocator AttachmentInput => _page.Locator("input[type='file']");
        private ILocator LastAttachment => _page.Locator("div[data-tid='file-preview-root']").Last;
        private ILocator LastAttachmentOptions => LastAttachment.Locator("button").Last;
        private ILocator PopUpOptionsMenu => _page.Locator("[data-tid='more-options-popup']");
        private ILocator DownloadButton => PopUpOptionsMenu.Locator("a").Nth(1);

        public async Task WriteMessageAsync(string message)
        {
            await MessageInput.ClickAsync();
            await MessageInput.FillAsync(message);
        }

        public async Task SendMessageAsync() =>
            await SendMessageButton.ClickAsync();

        public async Task SendMultipleMessagesAsync(string messagePrefix, int count = 3)
        {
            for (int i = 0; i < count; i++)
            {
                var message = $"{messagePrefix}{i + 1}";
                await WriteMessageAsync(message);
                await SendMessageAsync();
                Console.WriteLine($"Message {i + 1} sent: {message}");
                await Task.Delay(1000);
            }
        }

        public async Task UploadFileAsync(string filePath)
        {
            await PlusMenuButton.ClickAsync();
            await AttachmentButton.ClickAsync();
            await AttachmentInput.SetInputFilesAsync(filePath);
            await MessageInput.ClickAsync();
            await Task.Delay(2000);
            Console.WriteLine($"File uploaded: {filePath}");
            await _page.Keyboard.PressAsync("Control+Enter");
            await Task.Delay(2000);
            Console.WriteLine("Message sent after file upload.");
        }

        public async Task UploadUniqueFileAsync(string sourceFileName = "example.pdf")
        {
            var uniqueFileName = $"example_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            var sourcePath = Path.Combine("Resources", sourceFileName);
            var tempPath = Path.Combine(Path.GetTempPath(), uniqueFileName);

            File.Copy(sourcePath, tempPath, true);
            await UploadFileAsync(tempPath);
        }

        public async Task DownloadLastFileFromChatAsync()
        {
            await LastAttachmentOptions.ClickAsync();
            await PopUpOptionsMenu.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            Console.WriteLine("Options menu opened for the last attachment.");

            var waitForDownload = _page.WaitForDownloadAsync();
            await DownloadButton.ClickAsync();
            Console.WriteLine("Download initiated for the last attachment.");
            var download = await waitForDownload;

            var downloadsPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Resources", "Downloads"));
            var targetPath = Path.Combine(downloadsPath, download.SuggestedFilename);
            await download.SaveAsAsync(Path.GetFullPath(targetPath));
            Console.WriteLine($"Downloaded file saved to: {download.SuggestedFilename}");
        }

        public async Task<string> GetLastChatMessageTextAsync()
        {
            return await _page.Locator("[data-tid='chat-pane-message']").Last.InnerTextAsync();
        }
    }
}
