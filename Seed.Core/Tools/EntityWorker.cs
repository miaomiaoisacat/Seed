using Seed.Core.Bases;
using System;

namespace Seed.Core.Tools
{
    public class EntityWorker
    {
        public BaseEntity Data { get; set; }
        public Type DataType { get; set; }
        public Func<BaseEntity, string> SqlBuilder { get; set; }
    }
}
