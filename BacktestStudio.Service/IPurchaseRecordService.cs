using BacktestStudio.Repository.Models;

namespace BacktestStudio.Service;

/// <summary>
/// 買入記錄服務介面，提供買入記錄的 CRUD 操作和統計計算功能
/// </summary>
public interface IPurchaseRecordService
{
    /// <summary>
    /// 取得所有買入記錄，按買入日期降序排列
    /// </summary>
    /// <returns>所有買入記錄的集合</returns>
    /// <exception cref="InvalidOperationException">當資料庫操作發生錯誤時拋出</exception>
    Task<IEnumerable<PurchaseRecord>> GetAllAsync();

    /// <summary>
    /// 根據 ID 取得特定的買入記錄
    /// </summary>
    /// <param name="id">買入記錄的唯一識別碼</param>
    /// <returns>找到的買入記錄，若不存在則返回 null</returns>
    /// <exception cref="ArgumentException">當 ID 小於或等於 0 時拋出</exception>
    /// <exception cref="InvalidOperationException">當資料庫操作發生錯誤時拋出</exception>
    Task<PurchaseRecord?> GetByIdAsync(int id);

    /// <summary>
    /// 根據股票代碼取得相關的買入記錄，按買入日期降序排列
    /// </summary>
    /// <param name="symbol">股票代碼</param>
    /// <returns>指定股票代碼的買入記錄集合</returns>
    /// <exception cref="ArgumentException">當股票代碼為空或 null 時拋出</exception>
    /// <exception cref="InvalidOperationException">當資料庫操作發生錯誤時拋出</exception>
    Task<IEnumerable<PurchaseRecord>> GetBySymbolAsync(string symbol);

    /// <summary>
    /// 新增一筆買入記錄到資料庫
    /// </summary>
    /// <param name="purchaseRecord">要新增的買入記錄</param>
    /// <returns>新增後的買入記錄，包含系統生成的 ID 和時間戳記</returns>
    /// <exception cref="ArgumentNullException">當 purchaseRecord 為 null 時拋出</exception>
    /// <exception cref="ArgumentException">當買入記錄的屬性值無效時拋出（如價格或數量小於等於 0）</exception>
    /// <exception cref="InvalidOperationException">當資料庫操作發生錯誤時拋出</exception>
    Task<PurchaseRecord> AddAsync(PurchaseRecord purchaseRecord);

    /// <summary>
    /// 更新現有的買入記錄
    /// </summary>
    /// <param name="purchaseRecord">包含更新資訊的買入記錄</param>
    /// <returns>更新後的買入記錄</returns>
    /// <exception cref="ArgumentNullException">當 purchaseRecord 為 null 時拋出</exception>
    /// <exception cref="ArgumentException">當找不到指定 ID 的記錄或屬性值無效時拋出</exception>
    /// <exception cref="InvalidOperationException">當資料庫操作發生錯誤時拋出</exception>
    Task<PurchaseRecord> UpdateAsync(PurchaseRecord purchaseRecord);

    /// <summary>
    /// 根據 ID 刪除買入記錄
    /// </summary>
    /// <param name="id">要刪除的買入記錄 ID</param>
    /// <returns>刪除成功返回 true，找不到記錄返回 false</returns>
    /// <exception cref="ArgumentException">當 ID 小於或等於 0 時拋出</exception>
    /// <exception cref="InvalidOperationException">當資料庫操作發生錯誤時拋出</exception>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// 計算總投入金額，公式：∑(買入價格 × 買入數量)
    /// </summary>
    /// <param name="symbol">股票代碼，若為 null 則計算所有股票的總投入金額</param>
    /// <returns>總投入金額</returns>
    /// <exception cref="InvalidOperationException">當資料庫操作發生錯誤時拋出</exception>
    Task<decimal> GetTotalInvestmentAsync(string? symbol = null);

    /// <summary>
    /// 計算總持有數量，公式：∑(買入數量)
    /// </summary>
    /// <param name="symbol">股票代碼，若為 null 則計算所有股票的總持有數量</param>
    /// <returns>總持有數量</returns>
    /// <exception cref="InvalidOperationException">當資料庫操作發生錯誤時拋出</exception>
    Task<int> GetTotalQuantityAsync(string? symbol = null);

    /// <summary>
    /// 計算加權平均買入價格，公式：總投入金額 ÷ 總持有數量
    /// </summary>
    /// <param name="symbol">股票代碼，若為 null 則計算所有股票的平均買入價格</param>
    /// <returns>加權平均買入價格，若沒有買入記錄則返回 0</returns>
    /// <exception cref="InvalidOperationException">當資料庫操作發生錯誤時拋出</exception>
    Task<decimal> GetAveragePurchasePriceAsync(string? symbol = null);
}