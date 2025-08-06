using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace BacktestStudio.Web.UITests;

[TestClass]
public class ChartsPageTests : PageTest
{
    private const string BaseUrl = "http://localhost:5182";
    
    [TestInitialize]
    public async Task Setup()
    {
        await Page.GotoAsync($"{BaseUrl}/charts");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [TestMethod]
    public async Task ChartsPage_ShouldDisplayCorrectTitle()
    {
        await Expect(Page).ToHaveTitleAsync("圖表分析 - BacktestStudio");
        
        var heading = Page.Locator("h1");
        await Expect(heading).ToContainTextAsync("圖表分析");
    }

    [TestMethod]
    public async Task ChartsPage_ShouldDisplayTimeFrameButtons()
    {
        // 驗證時間框架按鈕
        var dailyButton = Page.Locator("button").Filter(new() { HasTextString = "日線" });
        await Expect(dailyButton).ToBeVisibleAsync();
        await Expect(dailyButton).ToHaveClassAsync(new Regex(".*active.*"));
        
        var weeklyButton = Page.Locator("button").Filter(new() { HasTextString = "週線" });
        await Expect(weeklyButton).ToBeVisibleAsync();
        
        var monthlyButton = Page.Locator("button").Filter(new() { HasTextString = "月線" });
        await Expect(monthlyButton).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task ChartsPage_ShouldDisplayTechnicalIndicatorSettings()
    {
        // 驗證技術指標設定區塊
        await Expect(Page.Locator("text=技術指標設定")).ToBeVisibleAsync();
        
        // 驗證移動平均線選項
        var ma5Checkbox = Page.Locator("input[type='checkbox']").First;
        await Expect(ma5Checkbox).ToBeCheckedAsync();
        await Expect(Page.Locator("text=MA5 (5日移動平均線)")).ToBeVisibleAsync();
        
        await Expect(Page.Locator("text=MA10 (10日移動平均線)")).ToBeVisibleAsync();
        
        var ma20Checkbox = Page.Locator("text=MA20 (20日移動平均線)").Locator("..").Locator("input");
        await Expect(ma20Checkbox).ToBeCheckedAsync();
        
        await Expect(Page.Locator("text=MA50 (50日移動平均線)")).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task ChartsPage_ShouldDisplayDateRangeControls()
    {
        // 驗證時間範圍控制項
        await Expect(Page.Locator("text=時間範圍")).ToBeVisibleAsync();
        
        // 驗證開始日期輸入框
        var startDateInput = Page.Locator("input[value='2024-01-01']");
        await Expect(startDateInput).ToBeVisibleAsync();
        
        // 驗證結束日期輸入框
        var endDateInput = Page.Locator("input[value='2025-01-01']");
        await Expect(endDateInput).ToBeVisibleAsync();
        
        // 驗證更新圖表按鈕
        var updateButton = Page.Locator("button").Filter(new() { HasTextString = "更新圖表" });
        await Expect(updateButton).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task ChartsPage_ShouldDisplayDevelopmentNotice()
    {
        // 驗證開發中的提示訊息
        var devNotice = Page.Locator("text=圖表功能開發中，即將整合ApexCharts.NET來顯示蠟燭圖和技術指標");
        await Expect(devNotice).ToBeVisibleAsync();
        
        var placeholderText = Page.Locator("text=蠟燭圖表將在此顯示");
        await Expect(placeholderText).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task ChartsPage_TechnicalIndicatorCheckboxes_ShouldBeInteractive()
    {
        // 測試MA10複選框的互動性
        var ma10Checkbox = Page.Locator("text=MA10 (10日移動平均線)").Locator("..").Locator("input");
        
        // 應該預設為未選中
        await Expect(ma10Checkbox).Not.ToBeCheckedAsync();
        
        // 點擊選中
        await ma10Checkbox.CheckAsync();
        await Expect(ma10Checkbox).ToBeCheckedAsync();
        
        // 再次點擊取消選中
        await ma10Checkbox.UncheckAsync();
        await Expect(ma10Checkbox).Not.ToBeCheckedAsync();
    }
}
