using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BO;
using Repo;

namespace FUNewsManagement_BuiHuaXuanHuy.Pages.SystemAccountPages
{
    public class CreateModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public SystemAccount SystemAccount { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _unitOfWork.GetRepository<SystemAccount>().InsertAsync(SystemAccount);
            await _unitOfWork.SaveAsync();

            return RedirectToPage("./Index");
        }
    }
}
