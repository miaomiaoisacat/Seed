using System.Data;

namespace Seed.Core.Interfaces
{
    public interface IConFactory
    {
        IDbConnection CreateCon();
    }
}
