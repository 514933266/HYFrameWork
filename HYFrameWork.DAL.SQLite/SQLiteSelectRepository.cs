using HYFrameWork.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dapper;

namespace HYFrameWork.DAL.SQLite
{
    
    partial class SQLiteRepository<T> : IRepository<T> 
    {
        #region 其他查询
        public int Count(Expression<Func<T, bool>> predicate, string dbLock = DbLock.Default, bool readOnly = false)
        {
            var cmd = SqlBuilder<T>.BuildCountCommand(predicate, dbLock);
            return GetConnection(readOnly).ExecuteScalar<int>(cmd.Sql, cmd.Parameters);
        }
        public bool Exists(Expression<Func<T, bool>> predicate, string dbLock = DbLock.Default, bool readOnly = false)
        {
            return Count(predicate, dbLock, readOnly) > 0;
        }
        #endregion

        #region  查询单个实体
        public T Get(
            Expression<Func<T, bool>> predicate,
            string dbLock = DbLock.Default,
            bool readOnly = false)
        {

            return GetEx<T>(predicate, null, null, dbLock, readOnly);
        }
        public T Get(
            Expression<Func<T, bool>> predicate,
            Func<DbSort<T>, DbSort<T>> orderby,
            string dbLock = DbLock.Default,
            bool readOnly = false)
        {
            return GetEx<T>(predicate, orderby, null, dbLock, readOnly);
        }
        public TResult GetEx<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            string dbLock = DbLock.Default,
            bool readOnly = false
            )
        {
            return GetEx(predicate, null, selector, dbLock, readOnly);
        }
        public TResult GetEx<TResult>(
            Expression<Func<T, bool>> predicate,
            Func<DbSort<T>, DbSort<T>> orderby,
            Expression<Func<T, TResult>> selector,
            string dbLock,
            bool readOnly)
        {
            return GetListEx( predicate, orderby, selector, dbLock, readOnly).FirstOrDefault();
        }

        #endregion

        #region 查询实体列表
        public List<T> GetList(
            Expression<Func<T, bool>> predicate,
            string dbLock = DbLock.Default,
            bool readOnly = false
            )
        {
            return GetListEx<T>( predicate, null, null, dbLock, readOnly);
        }
        public List<T> GetList(
            Expression<Func<T, bool>> predicate,
            Func<DbSort<T>, DbSort<T>> orderby,
            string dbLock = DbLock.Default,
            bool readOnly = false
            )
        {
            return GetListEx<T>(predicate, orderby, null, dbLock, readOnly);
        }
        public List<T> GetList(
            Expression<Func<T, bool>> predicate,
            int top,
            string dbLock = DbLock.Default,
            bool readOnly = false
            )
        {
            return GetListEx<T>(predicate, null, null, top, dbLock, readOnly);
        }
        public List<T> GetList(
            Expression<Func<T, bool>> predicate,
            Func<DbSort<T>, DbSort<T>> orderby,
            int top,
            string dbLock,
            bool readOnly)
        {
            return GetListEx<T>(predicate, orderby, null, top, dbLock, readOnly);
        }
        public List<TResult> GetListEx<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            string dbLock,
            bool readOnly)
        {
            return GetListEx(predicate, null, selector, dbLock, readOnly);
        }
        public List<TResult> GetListEx<TResult>(
            Expression<Func<T, bool>> predicate,
            Func<DbSort<T>, DbSort<T>> orderby,
            Expression<Func<T, TResult>> selector,
            string dbLock = DbLock.Default,
            bool readOnly = false
            )
        {
            return GetListEx(predicate, orderby, selector, 0, dbLock, readOnly);
        }
        public List<TResult> GetListEx<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            int top,
            string dbLock = DbLock.Default,
            bool readOnly = false
            )
        {
            return GetListEx(predicate, null, selector, top, dbLock, readOnly);
        }
        public List<TResult> GetListEx<TResult>(
            Expression<Func<T, bool>> predicate,
            Func<DbSort<T>, DbSort<T>> orderby,
            Expression<Func<T, TResult>> selector,
            int top,
            string dbLock,
            bool readOnly)
        {
            if (typeof(TResult) == typeof(T))
            {
                var cmd = SqlBuilder<T>.BuildSelectCommand(predicate,orderby,selector,top,dbLock);
                return GetConnection(readOnly).Query<TResult>(cmd.Sql, cmd.Parameters).ToList();
            }
            else
            {
                throw new NotSupportedException("SQLite Unsupported selector method, T has to be the same as TResult.");
            }
        }
        #endregion

        #region 查询分页数据
        public PageList<T> PageList(
            Expression<Func<T, bool>> predicate,
            int pageIndex,
            int pageSize,
            string dbLock = DbLock.Default,
            bool readOnly = false)
        {
            return PageListEx<T>(predicate, null, null,pageIndex, pageSize, dbLock, readOnly);
        }
        public PageList<T> PageList(
            Expression<Func<T, bool>> predicate,
            Func<DbSort<T>, DbSort<T>> orderby,
            int pageIndex,
            int pageSize,
            string dbLock= DbLock.Default,
            bool readOnly = false)
        {
            return PageListEx<T>(predicate, orderby, null, pageIndex, pageSize, dbLock, readOnly);
        }
        public PageList<TResult> PageListEx<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            int pageIndex,
            int pageSize,
            string dbLock = DbLock.Default,
            bool readOnly = false)
        {
            return PageListEx(predicate,null, selector, pageIndex, pageSize, DbLock.Default, readOnly);
        }
        public PageList<TResult> PageListEx<TResult>(
            Expression<Func<T, bool>> predicate,
            Func<DbSort<T>, DbSort<T>> orderby,
            Expression<Func<T, TResult>> selector,
            int pageIndex,
            int pageSize,
            string dbLock,
            bool readOnly)
        {
            if (typeof(TResult) == typeof(T))
            {
                var cmd = SqlBuilder<T>.BuildPageCommand(predicate, orderby, selector, pageIndex, pageSize, dbLock);
                using (var multi = GetConnection(readOnly).QueryMultiple(cmd.Sql, cmd.Parameters))
                {
                    var total = multi.Read<long>().First();
                    var items = multi.Read<TResult>().ToList();
                    return new PageList<TResult>((int)total, pageSize, pageIndex, items);
                }
            }
            else
            {
                throw new NotSupportedException("SQLite Unsupported selector method, T has to be the same as TResult.");
            }
            
        }


        #endregion

        #region  SQL 查询
        public IEnumerable<TResult>Query<TResult>(
            string sql,
            object parms,
            bool readOnly = false)
        {
            return GetConnection(readOnly).Query<TResult>(sql, parms);
        }
        public List<IEnumerable> Query<T1, T2>(
            string sql,
            object parms,
            bool readOnly = false)
        {
            var readedList = new List<IEnumerable>();
            using (var multi = GetConnection(readOnly).QueryMultiple(sql, parms))
            {
                readedList.Add(multi.Read<T1>());
                readedList.Add(multi.Read<T2>());
                return readedList;
            }
        }
        public List<IEnumerable> Query<T1, T2, T3>(
            string sql,
            object parms,
            bool readOnly = false)
        {
            var readedList = new List<IEnumerable>();
            using (var multi = GetConnection(readOnly).QueryMultiple(sql, parms))
            {
                readedList.Add(multi.Read<T1>());
                readedList.Add(multi.Read<T2>());
                readedList.Add(multi.Read<T3>());
                return readedList;
            }
        }
        public List<IEnumerable> Query<T1, T2, T3, T4>(
            string sql,
            object parms,
            bool readOnly = false)
        {
            var readedList = new List<IEnumerable>();
            using (var multi = GetConnection(readOnly).QueryMultiple(sql, parms))
            {
                readedList.Add(multi.Read<T1>());
                readedList.Add(multi.Read<T2>());
                readedList.Add(multi.Read<T3>());
                readedList.Add(multi.Read<T4>());
                return readedList;
            }
        } 
        public List<IEnumerable> Query<T1, T2, T3, T4, T5>(
            string sql,
            object parms,
            bool readOnly = false)
        {
            var readedList = new List<IEnumerable>();
            using (var multi = GetConnection(readOnly).QueryMultiple(sql, parms))
            {
                readedList.Add(multi.Read<T1>());
                readedList.Add(multi.Read<T2>());
                readedList.Add(multi.Read<T3>());
                readedList.Add(multi.Read<T4>());
                readedList.Add(multi.Read<T5>());
                return readedList;
            }
        }
        public TResult ExecuteScalar<TResult>(
            string sql,
            object parms,
            bool readOnly = false)
        {
            return GetConnection(readOnly).ExecuteScalar<TResult>(sql, parms);
        }

        #endregion
    }
}
