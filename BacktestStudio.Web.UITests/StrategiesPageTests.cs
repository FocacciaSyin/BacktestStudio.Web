using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace BacktestStudio.Web.UITests;

[TestClass]
public class StrategiesPageTests : PageTest
{
    private const string BaseUrl = "http://localhost:5182";
    
    [TestInitialize]
    public async Task Setup()
    {
        await Page.GotoAsync($"{BaseUrl}/strategies");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [TestMethod]
    public async Task StrategiesPage_ShouldDisplayCorrectTitle()
    {
        await Expect(Page).ToHaveTitleAsync("策略管理 - BacktestStudio");
        
        var heading = Page.Locator("h1");
        await Expect(heading).ToContainTextAsync("策略管理");
    }

    [TestMethod]
    public async Task StrategiesPage_ShouldDisplayAddStrategyButton()
    {
        var addButton = Page.Locator("button").Filter(new() { HasTextString = "新增策略" });
        await Expect(addButton).ToBeVisibleAsync();
        await Expect(addButton).ToBeEnabledAsync();
    }

    [TestMethod]
    public async Task StrategiesPage_ShouldDisplayStrategyTable()
    {
        // 驗證表格標題
        var tableHeader = Page.Locator("text=我的策略");
        await Expect(tableHeader).ToBeVisibleAsync();
        
        // 驗證表格欄位
        await Expect(Page.Locator("text=策略名稱")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=類型")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=狀態")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=績效")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=建立時間")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=操作")).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task StrategiesPage_ShouldDisplaySampleStrategies()
    {
        // 驗證長線策略
        await Expect(Page.Locator("text=長線策略")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=基於移動平均線的長線投資策略")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=多頭")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=啟用")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=+15.2%")).ToBeVisibleAsync();
        
        // 驗證短線策略
        await Expect(Page.Locator("text=短線策略")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=快速進出的短線交易策略")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=空頭")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=停用")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=-3.8%")).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task StrategiesPage_ShouldDisplayActionButtons()
    {
        // 檢查操作按鈕存在（雖然功能尚未實作）
        var actionButtons = Page.Locator("tbody button");
        var buttonCount = await actionButtons.CountAsync();
        
        // 應該有6個按鈕（每個策略3個操作按鈕）
        Assert.AreEqual(6, buttonCount, "應該有6個操作按鈕");
    }

    [TestMethod]
    public async Task StrategiesPage_ShouldDisplayDevelopmentNotice()
    {
        // 驗證開發中的提示訊息
        var devNotice = Page.Locator("text=策略管理功能開發中，即將推出完整的CRUD功能");
        await Expect(devNotice).ToBeVisibleAsync();
    }
}
