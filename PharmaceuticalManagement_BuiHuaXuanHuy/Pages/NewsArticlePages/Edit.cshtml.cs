using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BO;
using Repo;
using System.Security.Claims;

namespace FUNewsManagement_BuiHuaXuanHuy.Pages.NewsArticlePages
{
    public class EditModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EditModel(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = default!;

        public IActionResult OnGet(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            NewsArticle = _unitOfWork.GetRepository<NewsArticle>().Entities.FirstOrDefault(n => n.NewsArticleId == id);

            if (NewsArticle == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(_unitOfWork.GetRepository<Category>().Entities, "CategoryId", "CategoryName");
            ViewData["CreatedById"] = new SelectList(_unitOfWork.GetRepository<SystemAccount>().Entities, "AccountId", "AccountEmail");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null && short.TryParse(userIdClaim.Value, out short currentUserId))
            {
                NewsArticle.ModifiedDate = DateTime.Now;
                NewsArticle.UpdatedById = currentUserId;
            }
            else
            {
                return RedirectToPage("/Error");
            }

            try
            {

                _unitOfWork.GetRepository<NewsArticle>().Update(NewsArticle);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception)
            {
                if (_unitOfWork.GetRepository<NewsArticle>().Entities.Any(n => n.NewsArticleId == NewsArticle.NewsArticleId) == false)
                {
                    return NotFound();
                }
                throw;
            }

            return RedirectToPage("./Index");
        }
    }
}
