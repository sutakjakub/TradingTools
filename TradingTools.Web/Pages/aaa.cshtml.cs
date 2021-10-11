using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TradingTools.Db;
using TradingTools.Db.Entities;

namespace TradingTools.Web.Pages
{
    public class aaaModel : PageModel
    {
        private readonly TradingTools.Db.TradingToolsDbContext _context;

        public aaaModel(TradingTools.Db.TradingToolsDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["SymbolInfoId"] = new SelectList(_context.T2SymbolInfos, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public T2TradeGroupEntity T2TradeGroupEntity { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.T2TradeGroups.Add(T2TradeGroupEntity);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
