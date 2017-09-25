using System;

namespace Core.Entities
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }

        DateTime ModifiedDate { get; set; }
    }
}
