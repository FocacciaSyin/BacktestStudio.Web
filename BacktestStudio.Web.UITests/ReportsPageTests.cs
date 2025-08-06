using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace BacktestStudio.Web.UITests;

[TestClass]
public class ReportsPageTests : PageTest
{
    private const string BaseUrl = "http://localhost:5182";
    
    [TestInitialize]
    public async Task Setup()
    {
        await Page.GotoAsync($"{BaseUrl}/reports");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [TestMethod]
    public async Task ReportsPage_ShouldDisplayCorrectTitle()
    {
        await Expect(Page).ToHaveTitleAsync("分析報表 - BacktestStudio");
        
        var heading = Page.Locator("h1");
        await Expect(heading).ToContainTextAsync("分析報表");
    }

    [TestMethod]
    public async Task ReportsPage_ShouldDisplayExportButtons()
    {
        // 驗證匯出CSV按鈕
        var csvButton = Page.Locator("button").Filter(new() { HasTextString = "匯出CSV" });
        await Expect(csvButton).ToBeVisibleAsync();
        await Expect(csvButton).ToBeEnabledAsync();
        
        // 驗證匯出PDF按鈕
        var pdfButton = Page.Locator("button").Filter(new() { HasTextString = "匯出PDF" });
        await Expect(pdfButton).ToBeVisibleAsync();
        await Expect(pdfButton).ToBeEnabledAsync();
    }

    [TestMethod]
    public async Task ReportsPage_ShouldDisplayPerformanceMetrics()
    {
        // 驗證總獲利
        await Expect(Page.Locator("text=總獲利")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=+$12,580")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=+12.58%")).ToBeVisibleAsync();
        
        // 驗證勝率
        await Expect(Page.Locator("text=勝率")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=68.5%")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=87/127 筆交易")).ToBeVisibleAsync();
        
        // 驗證最大回撤
        await Expect(Page.Locator("text=最大回撤")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=-5.2%")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=\"-$5,200\"")).ToBeVisibleAsync();
        
        // 驗證夏普比率
        await Expect(Page.Locator("text=夏普比率")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=1.85")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=優秀")).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task ReportsPage_ShouldDisplayFilters()
    {
        // 驗證報表篩選區塊
        await Expect(Page.Locator("text=報表篩選")).ToBeVisibleAsync();
        
        // 驗證策略選擇器
        var strategySelect = Page.Locator("select").First;
        await Expect(strategySelect).ToBeVisibleAsync();
        await Expect(strategySelect).ToHaveValueAsync("全部策略");
        
        // 驗證日期範圍選擇器
        var dateRangeSelect = Page.Locator("select").Last;
        await Expect(dateRangeSelect).ToBeVisibleAsync();
        await Expect(dateRangeSelect).ToHaveValueAsync("最近30天");
        
        // 驗證套用篩選按鈕
        var applyButton = Page.Locator("button").Filter(new() { HasTextString = "套用篩選" });
        await Expect(applyButton).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task ReportsPage_ShouldDisplayTradingHistory()
    {
        // 驗證交易記錄區塊
        await Expect(Page.Locator("text=交易記錄")).ToBeVisibleAsync();
        
        // 驗證表格標題
        await Expect(Page.Locator("text=日期")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=策略")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=類型")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=價格")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=數量")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=損益")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=累積")).ToBeVisibleAsync();
        
        // 驗證範例交易記錄
        await Expect(Page.Locator("text=2025-02-01")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=長線策略")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=買入")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=$15,200")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=+$850")).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task ReportsPage_ShouldDisplayPagination()
    {
        // 驗證分頁控制項
        await Expect(Page.Locator("text=上一頁")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=下一頁")).ToBeVisibleAsync();
        
        // 驗證頁數按鈕
        await Expect(Page.Locator("a").Filter(new() { HasTextString = "1" })).ToBeVisibleAsync();
        await Expect(Page.Locator("a").Filter(new() { HasTextString = "2" })).ToBeVisibleAsync();
        await Expect(Page.Locator("a").Filter(new() { HasTextString = "3" })).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task ReportsPage_FilterDropdowns_ShouldHaveCorrectOptions()
    {
        // 測試策略下拉選單
        var strategySelect = Page.Locator("select").First;
        await Expect(strategySelect.Locator("option")).ToHaveTextAsync([
            "全部策略",
            "長線策略", 
            "短線策略",
            "均值回歸策略"
        ]);
        
        // 測試日期範圍下拉選單
        var dateRangeSelect = Page.Locator("select").Last;
        await Expect(dateRangeSelect.Locator("option")).ToHaveTextAsync([
            "最近30天",
            "最近3個月",
            "最近一年", 
            "全部期間"
        ]);
    }

    [TestMethod]
    public async Task ReportsPage_ShouldDisplayDevelopmentNotices()
    {
        // 驗證績效趨勢圖開發中提示
        await Expect(Page.Locator("text=績效趨勢圖表開發中，將顯示累積獲利和回撤曲線")).ToBeVisibleAsync();
        await Expect(Page.Locator("text=績效趨勢圖將在此顯示")).ToBeVisibleAsync();
        
        // 驗證圓餅圖提示
        await Expect(Page.Locator("text=圓餅圖")).ToBeVisibleAsync();
    }
}
