using Microsoft.Playwright;

namespace BacktestStudio.Web.UITests;

[TestClass]
public class NavigationAndLayoutTests : BasePageTest
{
    [TestInitialize]
    public async Task Setup()
    {
        await BaseSetup();
        // 確保每個測試都從首頁開始
        await NavigateToAsync("");
    }

    [TestCleanup]
    public async Task Cleanup()
    {
        await BaseCleanup();
    }

    [TestMethod]
    public async Task HomePage_ShouldLoadCorrectly()
    {
        // 驗證頁面標題
        await Expect(Page).ToHaveTitleAsync("儀表板 - BacktestStudio");
        
        // 驗證主要標題
        var mainHeading = Page.Locator("h1");
        await Expect(mainHeading).ToContainTextAsync("交易策略儀表板");
        
        // 驗證歡迎訊息
        var welcomeText = Page.Locator("text=歡迎使用BacktestStudio");
        await Expect(welcomeText).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task Navigation_ShouldWorkForAllPages()
    {
        // 測試策略管理頁面
        await Page.ClickAsync("text=策略管理");
        await Expect(Page).ToHaveTitleAsync("策略管理 - BacktestStudio");
        await Expect(Page).ToHaveURLAsync($"{BaseUrl}/strategies");
        
        var strategiesHeading = Page.Locator("h1");
        await Expect(strategiesHeading).ToContainTextAsync("策略管理");

        // 測試圖表分析頁面
        await Page.ClickAsync("text=圖表分析");
        await Expect(Page).ToHaveTitleAsync("圖表分析 - BacktestStudio");
        await Expect(Page).ToHaveURLAsync($"{BaseUrl}/charts");
        
        var chartsHeading = Page.Locator("h1");
        await Expect(chartsHeading).ToContainTextAsync("圖表分析");

        // 測試分析報表頁面
        await Page.ClickAsync("text=分析報表");
        await Expect(Page).ToHaveTitleAsync("分析報表 - BacktestStudio");
        await Expect(Page).ToHaveURLAsync($"{BaseUrl}/reports");
        
        var reportsHeading = Page.Locator("h1");
        await Expect(reportsHeading).ToContainTextAsync("分析報表");

        // 回到首頁
        await Page.ClickAsync("text=儀表板");
        await Expect(Page).ToHaveTitleAsync("儀表板 - BacktestStudio");
        await Expect(Page).ToHaveURLAsync(BaseUrl + "/");
    }

    [TestMethod]
    public async Task NavigationMenu_ShouldHighlightActiveLink()
    {
        // 檢查首頁active狀態
        var homeLink = Page.Locator("nav a[href='']");
        await Expect(homeLink).ToHaveClassAsync(new Regex(".*active.*"));
        
        // 點擊策略管理並檢查active狀態
        await Page.ClickAsync("text=策略管理");
        var strategiesLink = Page.Locator("nav a[href='strategies']");
        await Expect(strategiesLink).ToHaveClassAsync(new Regex(".*active.*"));
        
        // 確認首頁連結不再是active
        await Expect(homeLink).Not.ToHaveClassAsync(new Regex(".*active.*"));
    }

    [TestMethod]
    public async Task Layout_ShouldBeConsistentAcrossPages()
    {
        var pagesToTest = new[]
        {
            new { LinkText = "策略管理", ExpectedUrl = "/strategies" },
            new { LinkText = "圖表分析", ExpectedUrl = "/charts" },
            new { LinkText = "分析報表", ExpectedUrl = "/reports" }
        };

        foreach (var page in pagesToTest)
        {
            await Page.ClickAsync($"text={page.LinkText}");
            
            // 檢查導航欄存在
            var navbar = Page.Locator(".navbar-brand");
            await Expect(navbar).ToBeVisibleAsync();
            await Expect(navbar).ToContainTextAsync("BacktestStudio.Web");
            
            // 檢查所有導航連結都存在 - 使用更精確的選擇器
            await Expect(Page.Locator("nav a", new() { HasTextString = "儀表板" })).ToBeVisibleAsync();
            await Expect(Page.Locator("nav a", new() { HasTextString = "策略管理" })).ToBeVisibleAsync();
            await Expect(Page.Locator("nav a", new() { HasTextString = "圖表分析" })).ToBeVisibleAsync();
            await Expect(Page.Locator("nav a", new() { HasTextString = "分析報表" })).ToBeVisibleAsync();
            
            // 檢查About連結存在
            var aboutLink = Page.Locator("a[href*='learn.microsoft.com']");
            await Expect(aboutLink).ToBeVisibleAsync();
        }
    }

    [TestMethod]
    public async Task BrandLogo_ShouldNavigateToHome()
    {
        // 先導航到其他頁面
        await Page.ClickAsync("text=策略管理");
        await Expect(Page).ToHaveURLAsync($"{BaseUrl}/strategies");
        
        // 點擊品牌標誌返回首頁
        await Page.ClickAsync(".navbar-brand");
        await Expect(Page).ToHaveURLAsync(BaseUrl + "/");
        await Expect(Page).ToHaveTitleAsync("儀表板 - BacktestStudio");
    }
}
