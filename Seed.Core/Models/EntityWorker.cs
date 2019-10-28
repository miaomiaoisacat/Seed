using System;

namespace Seed.Core.Models
{
    public class EntityWorker
    {
        public BaseEntity Data { get; set; }
        public Type DataType { get; set; }
        public Func<BaseEntity, string> SqlBuilder { get; set; }
    }
}
