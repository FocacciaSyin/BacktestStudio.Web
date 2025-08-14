using Microsoft.Playwright;

namespace BacktestStudio.Web.UITests;

/// <summary>
/// 基础测试类，处理 Playwright 初始化
/// </summary>
public class BasePageTest
{
    protected const string BaseUrl = "http://localhost:5182";
    protected IPage Page { get; private set; } = null!;
    private IBrowserContext? _context;

    [TestInitialize]
    public async Task BaseSetup()
    {
        // 使用自定义的程序集初始化器中的 Browser
        _context = await AssemblyInitializer.Browser.NewContextAsync();
        Page = await _context.NewPageAsync();
    }

    [TestCleanup]
    public async Task BaseCleanup()
    {
        if (Page != null)
        {
            await Page.CloseAsync();
        }
        
        if (_context != null)
        {
            await _context.CloseAsync();
        }
    }

    protected async Task NavigateToAsync(string path = "")
    {
        var url = string.IsNullOrEmpty(path) ? BaseUrl : $"{BaseUrl}/{path.TrimStart('/')}";
        await Page.GotoAsync(url);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    // 修复 Expect 方法，直接返回断言对象而不是 Task
    protected static ILocatorAssertions Expect(ILocator locator) => 
        Assertions.Expect(locator);

    protected static IPageAssertions Expect(IPage page) => 
        Assertions.Expect(page);
}
