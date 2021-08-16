using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingTools.Db.Entities
{
    /// <summary>
    /// Defines a PK for target entity in common way
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUnique<out T> where T : IEquatable<T>
    {
        T Id { get; }
    }
}
