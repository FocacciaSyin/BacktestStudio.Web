using Microsoft.Playwright;

namespace BacktestStudio.Web.UITests;

[TestClass]
public class AssemblyInitializer
{
    private static IPlaywright? _playwright;
    private static IBrowser? _browser;

    [AssemblyInitialize]
    public static async Task AssemblyInitialize(TestContext context)
    {
        // 安装并初始化 Playwright
        Program.Main(new[] { "install", "chromium" });
        
        _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
    }

    [AssemblyCleanup]
    public static async Task AssemblyCleanup()
    {
        if (_browser != null)
        {
            await _browser.CloseAsync();
            await _browser.DisposeAsync();
        }
        
        if (_playwright != null)
        {
            _playwright.Dispose();
        }
    }

    public static IBrowser Browser => _browser!;
    public static IPlaywright Playwright => _playwright!;
}
