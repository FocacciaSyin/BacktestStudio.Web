using BacktestStudio.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace BacktestStudio.Service;

/// <summary>
/// 買入記錄服務實作類別，提供買入記錄的 CRUD 操作和統計計算功能
/// </summary>
public class PurchaseRecordService : IPurchaseRecordService
{
    private readonly BackestStudioContext _context;

    /// <summary>
    /// 初始化 PurchaseRecordService 實例
    /// </summary>
    /// <param name="context">Entity Framework 資料庫上下文</param>
    public PurchaseRecordService(BackestStudioContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 取得所有買入記錄，按買入日期降序排列
    /// </summary>
    /// <returns>所有買入記錄的集合</returns>
    public async Task<IEnumerable<PurchaseRecord>> GetAllAsync()
    {
        return await _context.PurchaseRecords
            .OrderByDescending(p => p.Date)
            .ToListAsync();
    }

    /// <summary>
    /// 根據 ID 取得特定的買入記錄
    /// </summary>
    /// <param name="id">買入記錄的唯一識別碼</param>
    /// <returns>找到的買入記錄，若不存在則返回 null</returns>
    public async Task<PurchaseRecord?> GetByIdAsync(int id)
    {
        return await _context.PurchaseRecords
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>
    /// 根據股票代碼取得相關的買入記錄，按買入日期降序排列
    /// </summary>
    /// <param name="symbol">股票代碼</param>
    /// <returns>指定股票代碼的買入記錄集合</returns>
    public async Task<IEnumerable<PurchaseRecord>> GetBySymbolAsync(string symbol)
    {
        return await _context.PurchaseRecords
            .Where(p => p.Symbol == symbol)
            .OrderByDescending(p => p.Date)
            .ToListAsync();
    }

    /// <summary>
    /// 新增一筆買入記錄到資料庫，自動設定建立和更新時間
    /// </summary>
    /// <param name="purchaseRecord">要新增的買入記錄</param>
    /// <returns>新增後的買入記錄，包含系統生成的 ID 和時間戳記</returns>
    public async Task<PurchaseRecord> AddAsync(PurchaseRecord purchaseRecord)
    {
        // 設定建立時間和更新時間為當前 UTC 時間
        purchaseRecord.CreatedAt = DateTime.UtcNow;
        purchaseRecord.UpdatedAt = DateTime.UtcNow;

        _context.PurchaseRecords.Add(purchaseRecord);
        await _context.SaveChangesAsync();

        return purchaseRecord;
    }

    /// <summary>
    /// 更新現有的買入記錄，使用追蹤實體的方式確保資料一致性
    /// </summary>
    /// <param name="purchaseRecord">包含更新資訊的買入記錄</param>
    /// <returns>更新後的買入記錄</returns>
    /// <exception cref="ArgumentException">當找不到指定 ID 的記錄時拋出</exception>
    public async Task<PurchaseRecord> UpdateAsync(PurchaseRecord purchaseRecord)
    {
        var existingRecord = await GetByIdAsync(purchaseRecord.Id);
        if (existingRecord == null)
        {
            throw new ArgumentException($"找不到ID為 {purchaseRecord.Id} 的買入記錄");
        }

        // 更新所有可編輯欄位，保留原始建立時間
        existingRecord.Date = purchaseRecord.Date;
        existingRecord.Price = purchaseRecord.Price;
        existingRecord.Quantity = purchaseRecord.Quantity;
        existingRecord.Symbol = purchaseRecord.Symbol;
        existingRecord.StopLossPrice = purchaseRecord.StopLossPrice;
        existingRecord.ProfitAmount = purchaseRecord.ProfitAmount;
        existingRecord.SettlementDate = purchaseRecord.SettlementDate;
        existingRecord.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return existingRecord;
    }

    /// <summary>
    /// 根據 ID 刪除買入記錄，先查詢再刪除以確保記錄存在
    /// </summary>
    /// <param name="id">要刪除的買入記錄 ID</param>
    /// <returns>刪除成功返回 true，找不到記錄返回 false</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var record = await GetByIdAsync(id);
        if (record == null)
        {
            return false;
        }

        _context.PurchaseRecords.Remove(record);
        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// 計算總投入金額，使用 Entity Framework 的 SumAsync 方法進行資料庫層級計算
    /// </summary>
    /// <param name="symbol">股票代碼，若為 null 則計算所有股票的總投入金額</param>
    /// <returns>總投入金額，公式：∑(買入價格 × 買入數量)</returns>
    public async Task<decimal> GetTotalInvestmentAsync(string? symbol = null)
    {
        var query = _context.PurchaseRecords.AsQueryable();

        if (!string.IsNullOrEmpty(symbol))
        {
            query = query.Where(p => p.Symbol == symbol);
        }

        return await query.SumAsync(p => p.Price * p.Quantity);
    }

    /// <summary>
    /// 計算總持有數量，使用 Entity Framework 的 SumAsync 方法進行資料庫層級計算
    /// </summary>
    /// <param name="symbol">股票代碼，若為 null 則計算所有股票的總持有數量</param>
    /// <returns>總持有數量，公式：∑(買入數量)</returns>
    public async Task<int> GetTotalQuantityAsync(string? symbol = null)
    {
        var query = _context.PurchaseRecords.AsQueryable();

        if (!string.IsNullOrEmpty(symbol))
        {
            query = query.Where(p => p.Symbol == symbol);
        }

        return await query.SumAsync(p => p.Quantity);
    }

    /// <summary>
    /// 計算加權平均買入價格，載入相關記錄後在記憶體中進行計算
    /// 使用加權平均方式確保價格反映實際投入比例
    /// </summary>
    /// <param name="symbol">股票代碼，若為 null 則計算所有股票的平均買入價格</param>
    /// <returns>加權平均買入價格，公式：總投入金額 ÷ 總持有數量，若沒有買入記錄則返回 0</returns>
    public async Task<decimal> GetAveragePurchasePriceAsync(string? symbol = null)
    {
        var query = _context.PurchaseRecords.AsQueryable();

        if (!string.IsNullOrEmpty(symbol))
        {
            query = query.Where(p => p.Symbol == symbol);
        }

        var records = await query.ToListAsync();

        if (!records.Any())
        {
            return 0;
        }

        // 使用加權平均價格計算：總投入金額 ÷ 總數量
        // 這樣可以準確反映不同價格和數量的買入對平均價格的影響
        var totalInvestment = records.Sum(p => p.Price * p.Quantity);
        var totalQuantity = records.Sum(p => p.Quantity);

        return totalQuantity > 0 ? totalInvestment / totalQuantity : 0;
    }
}