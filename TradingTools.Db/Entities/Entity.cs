using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingTools.Db.Entities
{
    /// <summary>
    /// Base class for all EntityFramework's (master) entities
    /// </summary>
    /// <typeparam name="TId">Type of Id property (PK)</typeparam>
    public abstract class Entity<TId> : Entity
        , IEquatable<Entity<TId>>
        , IUnique<TId>
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// PK
        /// </summary>
        public virtual TId Id { get; set; }

        public virtual bool Equals(Entity<TId> other)
        {
            var result = false;

            if (other != null)
            {
                result = other.GetType() == GetType() //Same Table
                    && Id.Equals(other.Id); //Same Id
            }

            return result;
        }

        public override bool Equals(object obj) => Equals(obj as Entity<TId>);

        public override int GetHashCode() => Id.GetHashCode();
    }

    /// <summary>
    /// Base class for all EntityFramework's (master) entities.
    /// <para/>
    /// ! As your root class you should always use the generic version <see cref="Entity{TId}"/> instead of this.
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// DateTime Created
        /// </summary>
        public virtual DateTimeOffset Created { get; set; }

        /// <summary>
        /// DateTime Modified
        /// </summary>
        public virtual DateTimeOffset Updated { get; set; }

        /// <summary>
        /// Concurrency Token
        /// </summary>
        public virtual byte[] Timestamp { get; set; }
    }
}
