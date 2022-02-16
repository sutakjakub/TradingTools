using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Db.Enums;

namespace TradingTools.Db.Entities
{
    public class T2SyncEntity : Entity<long>
    {
        public SyncState State { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
