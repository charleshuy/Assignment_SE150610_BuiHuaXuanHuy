using BO;


namespace Repo.Services
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NewsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<NewsArticle>> GetAllNewsAsync()
        {
            return await _unitOfWork.GetRepository<NewsArticle>().GetAllAsync();
        }

        public async Task<NewsArticle?> GetNewsByIdAsync(string id)
        {
            return await _unitOfWork.GetRepository<NewsArticle>().GetByIdAsync(id);
        }

        public async Task AddNewsAsync(NewsArticle newsArticle)
        {
            var repo = _unitOfWork.GetRepository<NewsArticle>();
            await repo.InsertAsync(newsArticle);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateNewsAsync(NewsArticle newsArticle)
        {
            var repo = _unitOfWork.GetRepository<NewsArticle>();
            await repo.UpdateAsync(newsArticle);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteNewsAsync(string id)
        {
            var repo = _unitOfWork.GetRepository<NewsArticle>();
            var existingNews = await repo.GetByIdAsync(id);
            if (existingNews != null)
            {
                await repo.DeleteAsync(existingNews);
                await _unitOfWork.SaveAsync();
            }
        }
    }
}
