using Dapper;
using Seed.Core.Interfaces;
using Seed.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Seed.DataMySql
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IConFactory _conFactory;
        public Queue<EntityWorker> Workers { get; set; }

        public UnitOfWork(IConFactory conFactory)
        {
            this._conFactory = conFactory;
            this.Workers = new Queue<EntityWorker>();
        }

        public (bool, int) Commit()
        {
            bool res = false;
            int num = 0;

            try
            {
                using (var con = _conFactory.CreateCon())
                {
                    con.Open();
                    IDbTransaction trans = con.BeginTransaction();
                    try
                    {
                        if (this.Workers.Count > 0)
                        {
                            while (Workers.Count > 0) 
                            {
                                EntityWorker worker = Workers.Dequeue();
                                con.Execute(worker.SqlBuilder.Invoke(worker.Data), worker.Data, trans);
                                num++;
                            }
                            trans.Commit();
                            this.Workers.Clear();
                        }
                        res = true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        num = 0;
                    }
                    finally
                    {
                        if (con.State != ConnectionState.Closed)
                            con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return (res, num);
        }

        public void Create<T>(T model) where T : BaseEntity, new()
        {
            this.Workers.Enqueue(new EntityWorker
            {
                Data = model,
                DataType = typeof(T),
                SqlBuilder = new Func<BaseEntity, string>(x =>
                {
                    var arrProps = typeof(T).GetProperties();
                    StringBuilder builder = new StringBuilder();
                    bool isNeedComma = false;

                    builder.Append("insert into ");
                    builder.Append(EntityMapper.GetTableName(typeof(T)));
                    builder.Append(" (");

                    for (int i = 0; i < arrProps.Length; i++)
                    {
                        if (null == arrProps[i].GetValue(x))
                            continue;

                        if (isNeedComma)
                        {
                            builder.Append(",");
                            isNeedComma = false;
                        }

                        builder.Append(arrProps[i].Name);
                        isNeedComma = true;
                    }

                    isNeedComma = false;

                    builder.Append(") ");
                    builder.Append("values (");

                    for (int i = 0; i < arrProps.Length; i++)
                    {
                        if (null == arrProps[i].GetValue(x))
                            continue;

                        if (isNeedComma)
                        {
                            builder.Append(",");
                            isNeedComma = false;
                        }

                        builder.Append("@");
                        builder.Append(arrProps[i].Name);
                        isNeedComma = true;
                    }

                    builder.Append(")");
                    return builder.ToString();
                })
            });
        }

        public void Delete<T>(T model) where T : BaseEntity, new()
        {
            this.Workers.Enqueue(new EntityWorker
            {
                Data = model,
                DataType = typeof(T),
                SqlBuilder = new Func<BaseEntity, string>(x =>
                {
                    var arrProps = typeof(T).GetProperties();
                    StringBuilder builder = new StringBuilder();
                    bool isNeedAnd = false;

                    builder.Append("delete from ");
                    builder.Append(EntityMapper.GetTableName(typeof(T)));
                    builder.Append(" where ");

                    for (int i = 0; i < arrProps.Length; i++)
                    {
                        if (null == arrProps[i].GetValue(x))
                            continue;

                        if (isNeedAnd)
                        {
                            builder.Append(" and ");
                            isNeedAnd = false;
                        }

                        builder.Append(arrProps[i].Name);
                        builder.Append("=@");
                        builder.Append(arrProps[i].Name);
                        isNeedAnd = true;
                    }

                    return builder.ToString();
                })
            });
        }

        public void Update<T>(T model) where T : BaseEntity, new()
        {
            this.Workers.Enqueue(new EntityWorker
            {
                Data = model,
                DataType = typeof(T),
                SqlBuilder = new Func<BaseEntity, string>(x =>
                {
                    var arrProps = typeof(T).GetProperties();
                    StringBuilder builder = new StringBuilder();
                    bool isNeedComma = false;

                    builder.Append("update ");
                    builder.Append(EntityMapper.GetTableName(typeof(T)));
                    builder.Append(" set ");
                    for (int i = 0; i < arrProps.Length; i++)
                    {
                        if (null == arrProps[i].GetValue(x))
                            continue;
                        if (arrProps[i].Name == "ID")
                            continue;

                        if (isNeedComma)
                        {
                            builder.Append(",");
                            isNeedComma = false;
                        }

                        builder.Append(arrProps[i].Name);
                        builder.Append("=@");
                        builder.Append(arrProps[i].Name);
                        isNeedComma = true;
                    }
                    builder.Append(" where ID=@ID");
                    return builder.ToString();
                })
            });
        }
    }
}
