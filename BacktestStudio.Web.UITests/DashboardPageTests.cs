using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace BacktestStudio.Web.UITests;

[TestClass]
public class DashboardPageTests : PageTest
{
    private const string BaseUrl = "http://localhost:5182";
    
    [TestInitialize]
    public async Task Setup()
    {
        await Page.GotoAsync(BaseUrl);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [TestMethod]
    public async Task Dashboard_ShouldDisplaySystemStatus()
    {
        // 驗證系統狀態區塊存在
        var statusSection = Page.Locator("text=系統狀態");
        await Expect(statusSection).ToBeVisibleAsync();
        
        // 驗證系統狀態訊息
        var statusMessage = Page.Locator("text=系統正常運行，準備開始您的策略分析");
        await Expect(statusMessage).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task Dashboard_ShouldDisplayPerformanceMetrics()
    {
        // 驗證績效指標卡片
        var strategyCount = Page.Locator("text=3").First;
        await Expect(strategyCount).ToBeVisibleAsync();
        
        var strategyLabel = Page.Locator("text=策略數量");
        await Expect(strategyLabel).ToBeVisibleAsync();
        
        var totalTrades = Page.Locator("text=127");
        await Expect(totalTrades).ToBeVisibleAsync();
        
        var tradesLabel = Page.Locator("text=總交易次數");
        await Expect(tradesLabel).ToBeVisibleAsync();
        
        var winRate = Page.Locator("text=68%");
        await Expect(winRate).ToBeVisibleAsync();
        
        var winRateLabel = Page.Locator("text=勝率");
        await Expect(winRateLabel).ToBeVisibleAsync();
        
        var totalProfit = Page.Locator("text=+12.5%");
        await Expect(totalProfit).ToBeVisibleAsync();
        
        var profitLabel = Page.Locator("text=總獲利");
        await Expect(profitLabel).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task Dashboard_ShouldDisplayQuickStartLinks()
    {
        // 驗證快速開始區塊
        var quickStartSection = Page.Locator("text=快速開始");
        await Expect(quickStartSection).ToBeVisibleAsync();
        
        // 驗證管理策略連結
        var manageStrategiesLink = Page.Locator("a[href='/strategies']").Filter(new() { HasTextString =  "管理策略" });
        await Expect(manageStrategiesLink).ToBeVisibleAsync();
        
        // 驗證查看圖表連結
        var viewChartsLink = Page.Locator("a[href='/charts']").Filter(new() { HasTextString = "查看圖表" });
        await Expect(viewChartsLink).ToBeVisibleAsync();
        
        // 驗證分析報表連結
        var viewReportsLink = Page.Locator("a[href='/reports']").Filter(new() { HasTextString = "分析報表" });
        await Expect(viewReportsLink).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task Dashboard_QuickStartLinks_ShouldNavigateCorrectly()
    {
        // 測試管理策略連結
        await Page.ClickAsync("a[href='/strategies']");
        await Expect(Page).ToHaveURLAsync($"{BaseUrl}/strategies");
        await Page.GoBackAsync();
        
        // 測試查看圖表連結
        await Page.ClickAsync("a[href='/charts']");
        await Expect(Page).ToHaveURLAsync($"{BaseUrl}/charts");
        await Page.GoBackAsync();
        
        // 測試分析報表連結
        await Page.ClickAsync("a[href='/reports']");
        await Expect(Page).ToHaveURLAsync($"{BaseUrl}/reports");
        await Page.GoBackAsync();
    }
}
