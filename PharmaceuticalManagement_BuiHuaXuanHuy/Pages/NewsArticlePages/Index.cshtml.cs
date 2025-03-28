using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BO;
using Microsoft.AspNetCore.Mvc;
using Repo;
using System.Security.Claims;

namespace FUNewsManagement_BuiHuaXuanHuy.Pages.NewsArticlePages
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<NewsArticle> NewsArticle { get; set; } = default!;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 4;
        public int TotalPages { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }
        public async Task OnGetAsync(int pageNumber = 1)
        {
            PageNumber = pageNumber;
            var query = _unitOfWork.GetRepository<NewsArticle>().Entities
                .Include(m => m.Category).Include(n => n.CreatedBy).AsQueryable();

            var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (userRole == "2" || string.IsNullOrEmpty(userRole))
            {
                query = query.Where(m => m.NewsStatus == true);
            }

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                query = query.Where(m => m.NewsTitle.Contains(SearchTerm));
            }

            int totalItems = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            NewsArticle = await query
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}
