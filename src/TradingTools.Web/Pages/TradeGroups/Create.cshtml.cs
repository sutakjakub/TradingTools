using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using TradingTools.Db.Entities;
using TradingTools.Persistence.Queries.Interfaces;
using TradingTools.Persistence.Stores.Interfaces;

namespace TradingTools.Web.Pages.TradeGroups
{
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel> logger;
        private readonly IT2TradeGroupStore tradeGroupStore;
        private readonly IT2SymbolInfoQuery symbolInfoQuery;

        public CreateModel(ILogger<CreateModel> logger, IT2TradeGroupStore tradeGroupStore, IT2SymbolInfoQuery symbolInfoQuery)
        {
            this.logger = logger;
            this.tradeGroupStore = tradeGroupStore;
            this.symbolInfoQuery = symbolInfoQuery;

            SymbolInfoList = new List<SelectListItem>();
        }

        [BindProperty]
        public T2TradeGroupEntity T2TradeGroupEntity { get; set; }

        public int SelectedSymbolInfoId { get; set; }

        public IList<SelectListItem> SymbolInfoList { get; set; }

        public async Task OnGetAsync()
        {
            var symbols = await symbolInfoQuery.All();
            foreach (var item in symbols)
            {
                SymbolInfoList.Add(
                    new SelectListItem
                    {
                        Text = item.Symbol,
                        Value = item.Id.ToString()
                    });
            }
            SymbolInfoList = SymbolInfoList.OrderBy(o => o.Text).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var entity = await tradeGroupStore.Create(T2TradeGroupEntity);
            return RedirectToPage("Detail", new { entity.Id });
        }
    }
}
