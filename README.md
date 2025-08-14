# BacktestStudio.Web
開發一個功能完整的大盤回測應用程式，讓用戶能夠測試不同的投資策略，分析各策略的獲利表現，並透過視覺化圖表展示結果。










# 大盤回測應用程式 PRD (Product Requirements Document)

## 1. 產品概述

### 1.1 產品名稱

**股市策略回測系統**

### 1.2 產品目標

開發一個功能完整的大盤回測應用程式，讓用戶能夠測試不同的投資策略，分析各策略的獲利表現，並透過視覺化圖表展示結果。

### 1.3 技術架構

- **前端**: Blazor Server
- **後端**: ASP.NET Core Web API (C#)
- **資料庫**: SQLite + Entity Framework Core
- **圖表**: ApexCharts.NET
- **UI框架**: Bootstrap CSS

### 1.4 專案結構規範

#### 模型組織原則
所有模型類別統一放在 `/Models` 資料夾底下，按照功能和用途分類：

```
Models/
├── Chart/                      # 圖表相關模型
│   ├── ChartDataPoint.cs       # 統一的圖表數據點模型
│   ├── CandlestickData.cs      # K線圖資料模型
│   ├── ApexCandlestickSeries.cs # ApexCharts K線圖資料序列格式
│   ├── ApexCandlestickDataPoint.cs # ApexCharts K線資料點
│   └── ChartOptions.cs         # 圖表配置選項模型
├── DTOs/                       # 數據傳輸對象
│   ├── MarketDataPoint.cs      # 市場數據點
│   ├── TechnicalIndicatorData.cs # 技術指標數據
│   ├── StrategyDto.cs          # 策略數據傳輸對象
│   └── PerformanceMetrics.cs   # 績效指標
└── ViewModels/                 # 檢視模型
    └── DashboardViewModel.cs   # 主儀表板視圖模型
```

#### 模型設計規範
- **單一職責**: 每個模型類別只負責一個特定的數據結構
- **完整文檔**: 每個類別和欄位都必須有詳細的XML註解
- **統一命名**: 使用描述性的類別和屬性命名
- **獨立文件**: 每個類別使用獨立的 .cs 文件

## 2. 核心功能需求

### 2.1 策略管理模組

- **策略建立**: 用戶可建立多個投資策略

- **策略類型**: 支援做多、做空操作

- 策略參數設定

  :

  - 進場條件 (價格、技術指標)
  - 出場條件 (停利、停損)
  - 資金分配比例

- **策略編輯與刪除**

### 2.2 交易記錄模組

- **交易點位記錄**: 記錄買進/賣出的時間點和價格
- **交易類型**: 區分做多/做空
- **關聯策略**: 每筆交易歸屬於特定策略
- **交易狀態**: 開倉/平倉狀態追蹤

### 2.3 技術指標模組

- **移動平均線**: MA(5), MA(10), MA(20), MA(50)
- **指標計算**: 自動計算各時間點的技術指標
- **指標顯示**: 在圖表上顯示技術指標線

### 2.4 回測引擎

- **歷史資料處理**: 載入大盤歷史價格資料
- **策略執行**: 根據策略規則模擬交易
- **績效計算**: 計算每個策略的獲利/虧損
- **風險指標**: 計算最大回撤、夏普比率等

### 2.5 資料視覺化模組

- **K線圖**: 顯示大盤歷史價格走勢
- **交易點標記**: 在圖表上標示買進/賣出點位
- **技術指標疊加**: MA線顯示在價格圖上
- **策略績效圖**: 各策略損益曲線

### 2.6 報表分析模組

- **策略比較**: 並列比較多個策略表現
- **獲利排行**: 策略獲利金額排序
- **詳細分析**: 勝率、平均獲利、交易次數等統計

## 3. 資料庫設計

### 3.1 核心資料表

#### MarketData (大盤資料表)

sql

```sql
Id (Primary Key)
Date (DateTime)
Open (decimal)
High (decimal)  
Low (decimal)
Close (decimal)
Volume (long)
```

#### Strategy (策略表)

sql

```sql
Id (Primary Key)
Name (string)
Description (string)
IsActive (bool)
CreatedDate (DateTime)
```

#### Trade (交易記錄表)

sql

```sql
Id (Primary Key)
StrategyId (Foreign Key)
TradeDate (DateTime)
TradeType (enum: Buy/Sell)
Position (enum: Long/Short)
Price (decimal)
Quantity (int)
Amount (decimal)
Status (enum: Open/Closed)
```

#### TechnicalIndicator (技術指標表)

sql

```sql
Id (Primary Key)
Date (DateTime)
MA5 (decimal)
MA10 (decimal)
MA20 (decimal)
MA50 (decimal)
```

#### StrategyResult (策略結果表)

sql

```sql
Id (Primary Key)
StrategyId (Foreign Key)
TotalProfit (decimal)
TotalTrades (int)
WinRate (decimal)
MaxDrawdown (decimal)
LastUpdated (DateTime)
```

## 4. 系統架構設計

### 4.1 後端架構 (C# Web API)

```
BacktestStudio.Web
Controllers/
├── MarketDataController.cs
├── StrategyController.cs
├── TradeController.cs
└── BacktestController.cs

BacktestStudio.Web.Service
├── IMarketDataService.cs
├── IStrategyService.cs  
├── ITechnicalIndicatorService.cs
└── IBacktestEngine.cs

BacktestStudio.Web.Common
Models/
├── MarketData.cs
├── Strategy.cs
├── Trade.cs
└── BacktestResult.cs

BacktestStudio.Web.Repository
├── ApplicationDbContext.cs
└── Repositories/
```

### 4.2 前端架構 (Blazor)

```
Pages/
├── Dashboard.razor
├── StrategyManager.razor
├── ChartView.razor
└── Reports.razor

Components/
├── TradingChart.razor
├── StrategyForm.razor
└── PerformanceTable.razor

Services/
├── ApiService.cs
└── ChartService.cs
```

## 5. 功能實作優先順序

### Phase 1: 基礎功能 (4-6週)

1. 資料庫架構建立
2. 大盤歷史資料匯入功能
3. 基本的策略CRUD操作
4. 簡單的K線圖顯示

### Phase 2: 核心回測 (4-6週)

1. 技術指標計算(MA線)
2. 交易記錄管理
3. 基礎回測引擎
4. 交易點位標記功能

### Phase 3: 進階分析 (3-4週)

1. 策略績效分析
2. 多策略比較
3. 詳細報表生成
4. UI優化與用戶體驗改善

## 6. 技術實作建議

### 6.1 資料來源

- 使用免費的股市API (如 Alpha Vantage, Yahoo Finance API)
- 或準備CSV格式的歷史資料匯入功能

### 6.2 圖表實作

- 建議使用 ApexCharts.NET 套件
- 支援K線圖、技術指標疊加
- 可互動式縮放與滑鼠懸停資訊

### 6.3 效能優化

- 大量歷史資料分頁載入
- 技術指標預先計算並儲存
- 使用記憶體快取提升查詢速度

### 6.4 Blazor元件規劃

csharp

```csharp
// 主要元件結構
<TradingDashboard>
  <StrategyPanel />
  <ChartComponent>
    <CandlestickChart />
    <TechnicalIndicators />
    <TradeMarkers />
  </ChartComponent>
  <PerformancePanel />
</TradingDashboard>
```

## 7. 非功能性需求

### 7.1 效能需求

- 圖表渲染時間 < 2秒
- 回測計算時間 < 5秒 (1年資料)
- 支援同時載入多個策略

### 7.2 可用性需求

- 響應式設計，支援桌面與平板
- 直覺的操作介面
- 完整的錯誤處理與用戶提示

### 7.3 擴充性需求

- 模組化設計，易於新增技術指標
- 支援未來新增其他金融商品
- 資料庫結構支援未來功能擴展

## 8. 風險評估與解決方案

### 8.1 技術風險

- **風險**: 大量歷史資料處理效能問題
- **解決**: 實作分頁載入、資料快取機制

### 8.2 資料風險

- **風險**: 歷史資料準確性與完整性
- **解決**: 多重資料來源驗證、資料清理機制

### 8.3 使用者體驗風險

- **風險**: 複雜的策略設定介面
- **解決**: 提供策略範本、分步驟引導設定

## 9. 成功指標

- 成功載入並顯示1年以上的大盤歷史資料
- 支援建立並執行至少5種不同策略
- 圖表能清楚標示所有交易點位
- 準確計算各策略獲利金額與統計指標
- 系統響應時間符合效能需求

## 10. 後續發展方向

- 新增更多技術指標 (RSI, MACD, 布林通道等)
- 支援多商品回測 (個股、期貨、外匯)
- 實作即時交易模擬功能
- 加入機器學習預測模型
