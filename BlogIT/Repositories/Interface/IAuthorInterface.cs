using BlogIT.Models.Domain;

namespace BlogIT.Repositories.Interface
{
    public interface IAuthorInterface
    {
        Task<IEnumerable<Author>> GetAuthorsAsync();
        Task<Author?> GetAuthorByIdAsync(Guid id);
        Task<Author?> UpdateAsync(Author author);
        Task<Author> CreateAsync(Author author);
        Task<Author?> DeleteAsync(Guid id);

    }
}
