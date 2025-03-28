using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BO;
using Repo;
using System.Security.Claims;

namespace FUNewsManagement_BuiHuaXuanHuy.Pages.NewsArticlePages
{
    public class CreateModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateModel(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult OnGet()
        {
            ViewData["CategoryId"] = new SelectList(_unitOfWork.GetRepository<Category>().Entities, "CategoryId", "CategoryName");
            ViewData["CreatedById"] = new SelectList(_unitOfWork.GetRepository<SystemAccount>().Entities, "AccountId", "AccountEmail");
            return Page();
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Get the current user ID from claims
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null && short.TryParse(userIdClaim.Value, out short currentUserId))
            {
                NewsArticle.CreatedById = currentUserId;
            }
            else
            {
                // Handle case when the user is not authenticated or ID is invalid
                return RedirectToPage("/Error");
            }

            NewsArticle.CreatedDate = DateTime.Now;
            NewsArticle.ModifiedDate = DateTime.Now;

            await _unitOfWork.GetRepository<NewsArticle>().InsertAsync(NewsArticle);
            await _unitOfWork.SaveAsync();

            return RedirectToPage("./Index");
        }
    }
}
