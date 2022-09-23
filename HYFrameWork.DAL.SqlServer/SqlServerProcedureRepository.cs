using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Dapper;
using HYFrameWork.Core;

namespace HYFrameWork.DAL.SqlServer
{
   partial class SqlServerRepository<T> : IRepository<T>
    {
        public int StoreProcedure(string procedure, DbParameter[] parms)
        {
            return GetConnection(false).Query<int>(procedure, parms, null, true, null, CommandType.StoredProcedure).FirstOrDefault();
        }

        public IEnumerable<TResult> QueryStoreProcedure<TResult>(string procedure, DbParameter[] parms)
        {
            DynamicParameters dyParms = new DynamicParameters();
            parms.ForEach(p =>
            {
                dyParms.Add(p.ParameterName, p.Value, p.DbType, p.Direction, p.Size);
            });
            var result = GetConnection(false).Query<TResult>(procedure, dyParms, null, true, null, CommandType.StoredProcedure);
            parms.ForEach(p =>
            {
                p.Value = dyParms.Get<object>(p.ParameterName);
            });
            return result;
        }
    }
}
