using System;

namespace diga.bl.Interfaces
{
    public interface IDbServiceEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; }
    }
}
