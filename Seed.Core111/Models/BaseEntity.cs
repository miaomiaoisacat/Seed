using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Seed.Core.Models
{
    public abstract class BaseEntity
    {
        protected string BaseSelectSql;
        protected string BaseCountSql;

        public BaseEntity()
        {
            this.IniBaseSelectSql();
            this.IniBaseCountSql();
        }

        public virtual (string, DbParameter[]) BuildSelectSql<T>(T model) where T : class, new() 
        {
            var arrProps = typeof(T).GetProperties();
            var str = new StringBuilder();
            var lstParams = new List<DbParameter>();

            for (int i = 0; i < arrProps.Length; i++)
            {
                if (null == arrProps[i].GetValue(model))
                    continue;
                if (str.Length == 0)
                    str.Append(" where " + arrProps[i].Name + "=@" + arrProps[i].Name);
                else
                    str.Append(" and " + arrProps[i].Name + "=@" + arrProps[i].Name);
            }

            return (str.ToString(),null);
        }
        public abstract (string, DbParameter[]) BuildCountSql<T>(T model) where T : class, new();
        protected abstract void IniBaseSelectSql();
        protected abstract void IniBaseCountSql();
    }
}
