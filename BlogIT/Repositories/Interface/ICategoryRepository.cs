using BlogIT.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace BlogIT.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(Guid id);
        Task<Category?> UpdateAsync(Category category);
        Task<Category> CreateAsync(Category category);
        Task<Category?> DeleteAsync(Guid id);
    }
}
