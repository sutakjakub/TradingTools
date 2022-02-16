using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingTools.Db.Entities;
using TradingTools.Persistence.Stores.Interfaces;

namespace TradingTools.Web.Pages.Notifications
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IT2NotificationStore _notificationStore;

        public IndexModel(
            ILogger<IndexModel> logger,
            IT2NotificationStore notificationStore)
        {
            _logger = logger;
            _notificationStore = notificationStore;
        }

        public IList<T2NotificationEntity> Notifications { get; private set; }

        public async Task OnGet()
        {
            var collection =  await _notificationStore.All();
            Notifications = collection.ToList();
        }
    }
}
