using BlogIT.Data;
using BlogIT.Models.Domain;
using BlogIT.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Runtime.InteropServices;

namespace BlogIT.Repositories.Implementation
{
    public class CategoryRepository : ICategoryInterface
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CategoryRepository(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            //return list of categories
            return await _applicationDbContext.Categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid id)
        {
            return await _applicationDbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            //check if the category exists
            var existingCategory = await _applicationDbContext.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);

            //if exists, set new values
            if (existingCategory != null)
            {
                existingCategory.Name = category.Name;
                existingCategory.UrlHandle = category.UrlHandle;
                await _applicationDbContext.SaveChangesAsync();

                return existingCategory;
            }

            return null;
            
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await _applicationDbContext.Categories.AddAsync(category);
            await _applicationDbContext.SaveChangesAsync();

            return category;
        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
            //check if data exists
            var response = await _applicationDbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (response != null)
            {
                _applicationDbContext.Categories.Remove(response);
                await _applicationDbContext.SaveChangesAsync();
                return response;
            }

            return null;
        }
    }
}
