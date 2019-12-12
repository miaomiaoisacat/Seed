using Seed.Core.Models;

namespace Seed.Core.Interfaces
{
    public interface IUnitOfWork
    {
        void Create<T>(T model) where T : BaseEntity, new();
        void Update<T>(T model) where T : BaseEntity, new();
        void Delete<T>(T model) where T : BaseEntity, new();
        /// <summary>
        /// 单元操作提交
        /// </summary>
        /// <returns>（执行结果，执行行数）</returns>
        (bool, int) Commit();
    }
}
