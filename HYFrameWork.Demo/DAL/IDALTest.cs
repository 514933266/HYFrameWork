using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYFrameWork.Test
{
    interface IDALTest
    {
        #region 增加
        void AddTest();

        void AddIfNotExistsTest();
        void AddListTest();

        #endregion

        #region 查询

        void SelectTest();
        void PredicateTest();
        #endregion

        #region 修改

        void UpdateTest();
        #endregion

        #region 删除
        void DeleteTest();
        #endregion

        #region 事务
        /// <summary>
        /// 本地事务
        /// </summary>
        void LoacalTransactionTest();

        /// <summary>
        /// 本地分布式事务
        /// </summary>
        void LoacalDisTransactionTest();
        /// <summary>
        /// 分布式事务
        /// </summary>
        void DistributedTransactionTest();
        #endregion

        #region 存储过程
        void StoreProcedureTest();
        #endregion
    }
}
