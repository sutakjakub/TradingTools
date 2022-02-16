using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Db.Enums;

namespace TradingTools.Db.Entities
{
    public class T2NotificationEntity : Entity<long>
    {
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
