namespace Seed.Core.Models.Entities.Bases
{
    public class Sys : BaseEntity
    {

        //public const string SelectSql = "select ID,Name from sys ";
        public int? ID { get; set; } = null;
        public string Name { get; set; } = null;

        protected override (string, DbParameter[]) BuildCountSql<T>(T model)
        {
            throw new System.NotImplementedException();
        }

        protected override (string, DbParameter[]) BuildSelectSql<T>(T model)
        {
            throw new System.NotImplementedException();
        }

        protected override void IniBaseCountSql()
        {
            this.BaseCountSql = "select count(ID) from sys";
        }

        protected override void IniBaseSelectSql()
        {
            this.BaseSelectSql = "select ID,Name from sys";
        }
    }
}
