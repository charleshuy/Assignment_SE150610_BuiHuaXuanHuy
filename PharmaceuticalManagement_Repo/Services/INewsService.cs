using BO;

namespace Repo.Services
{
    public interface INewsService
    {
        Task<IEnumerable<NewsArticle>> GetAllNewsAsync();
        Task<NewsArticle?> GetNewsByIdAsync(string id);
        Task AddNewsAsync(NewsArticle newsArticle);
        Task UpdateNewsAsync(NewsArticle newsArticle);
        Task DeleteNewsAsync(string id);
    }
}