using System.Linq.Expressions;

namespace OnlineCourse.Repositories
{

    /// Interface repository chung cho tất cả entity.
    /// Cung cấp CRUD cơ bản, phân trang, và query có điều kiện.

    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// Lấy entity theo Id (throw KeyNotFoundException nếu không thấy).
        Task<TEntity> GetByIdAsync(int id);
        /// Lấy danh sách entity theo điều kiện (không tracking).
        Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate);
        /// Thêm entity mới.

        Task AddAsync(TEntity entity);
        /// Cập nhật entity.
        Task UpdateAsync(TEntity entity);
        /// Xóa entity theo Id.
        Task DeleteAsync(int id);
        /// Lấy dữ liệu phân trang, có thể kèm filter.
        Task<IEnumerable<TEntity>> GetPagedAsync(
            int page,
            int pageSize,
            Expression<Func<TEntity, bool>>? filter = null);
        /// Đếm số lượng entity thỏa điều kiện.
        Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null);
        /// Kiểm tra có entity nào thỏa điều kiện không.

    }
}
