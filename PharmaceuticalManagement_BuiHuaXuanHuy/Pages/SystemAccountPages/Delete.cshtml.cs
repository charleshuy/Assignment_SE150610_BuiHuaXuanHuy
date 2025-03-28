using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BO;
using DAO;

namespace FUNewsManagement_BuiHuaXuanHuy.Pages.SystemAccountPages
{
    public class DeleteModel : PageModel
    {
        private readonly DAO.FunewsManagementContext _context;

        public DeleteModel(DAO.FunewsManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SystemAccount SystemAccount { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemaccount = await _context.SystemAccounts.FirstOrDefaultAsync(m => m.AccountId == id);

            if (systemaccount == null)
            {
                return NotFound();
            }
            else
            {
                SystemAccount = systemaccount;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemaccount = await _context.SystemAccounts.FindAsync(id);
            if (systemaccount != null)
            {
                SystemAccount = systemaccount;
                _context.SystemAccounts.Remove(SystemAccount);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
