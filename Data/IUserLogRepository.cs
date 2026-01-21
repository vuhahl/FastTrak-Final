using FastTrak.Models;

namespace FastTrak.Data
{
    /// <summary>
    /// Repository interface for user-specific data that stays local (SQLite).
    /// This data is private to the user and never syncs to the cloud API.
    ///
    /// WHY THIS INTERFACE EXISTS:
    /// Separating user data from reference data (restaurants, menus) allows us to:
    /// 1. Keep private nutrition logs on-device only
    /// 2. Migrate reference data to API without touching user data
    /// 3. Enable unit testing with mock implementations
    /// 4. Follow Interface Segregation Principle (ISP)
    /// </summary>
    public interface IUserLogRepository
    {
        /// <summary>
        /// Gets all items logged today (since midnight).
        /// Includes associated LoggedItemOptions for each item.
        /// </summary>
        Task<List<LoggedItem>> GetLoggedItemsForTodayAsync();

        /// <summary>
        /// Inserts a new logged item. Returns the number of rows inserted.
        /// The LoggedItem.Id will be populated after insert.
        /// </summary>
        Task<int> InsertLoggedItemAsync(LoggedItem item);

        /// <summary>
        /// Updates an existing logged item (e.g., quantity change).
        /// </summary>
        Task<int> UpdateLoggedItemAsync(LoggedItem item);

        /// <summary>
        /// Deletes a single logged item by ID.
        /// </summary>
        Task<int> DeleteLoggedItemAsync(int id);

        /// <summary>
        /// Inserts a customization option record for a logged item.
        /// Used to preserve historical accuracy of what options were selected.
        /// </summary>
        Task InsertLoggedItemOptionAsync(LoggedItemOption option);

        /// <summary>
        /// Clears all logged items for today. Used for "Clear All" feature.
        /// </summary>
        Task<int> ClearLoggedItemsForTodayAsync();
    }
}
