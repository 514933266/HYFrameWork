using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 单元事务
    /// </summary>
    public class UnitTransaction : IUnitTransaction,IDisposable
    {
        #region 字段/属性/构造

        private bool _commited;
        private int _effected = 0;
        private Exception _excetion;
        private readonly UnitOfWork _uow;
        private IDbConnection _conn;//连接对象
        private IDbTransaction _tran;//事务对象
        private readonly List<UnitAction> _actionList = new List<UnitAction>();
        
        public Exception Excetion { get { return _excetion; } }

        public bool Commited { get { return _commited; } }

        /// <summary>
        /// 构造：初始单元事务
        /// </summary>
        /// <param name="uow"></param>
        public UnitTransaction(UnitOfWork uow)
        {
            _uow = uow;
        }
        #endregion

        #region 注册事务

        public void Register(Func<IDbTransaction, int> action, IDbConnection conn)
        {
            _actionList.Add(new UnitAction(action, conn));
        }

        #endregion

        #region 提交事务
        public long Commit(TransactionType transactionType = TransactionType.Local)
        {
            if (_commited && transactionType== TransactionType.LocalDistribute)
            {
                ConfirmLocalDistributeCommit();
            }
            else if (_commited)
            {
                throw new InvalidOperationException("禁止重复提交事务!");
            }
            else
            {
                _commited = true;
                switch (transactionType)
                {
                    case TransactionType.Local: CommitForLocalTran(_actionList); break;
                    case TransactionType.Distribute: CommitForDistributedTran(_actionList); break;
                    case TransactionType.LocalDistribute: CommitForLocalDistributedTran(_actionList); break;
                    default:
                        throw new Exception("不支持的事务类型！");
                }
            }
            return _effected;
        }

        /// <summary>
        /// 提交事务(LocalDistribute时使用，作用为确认事务结果并最终提交)
        /// </summary>
        /// <returns>事务方法执行后的int返回总值</returns>
        public void ConfirmLocalDistributeCommit()
        {
            if (_tran != null&&_tran.Connection!=null) _tran.Commit();
        }

        //本地事务提交
        private void CommitForLocalTran(List<UnitAction> actionList)
        {
            var conn = actionList.First(a => a.Conn != null).Conn;
            if (conn.State != ConnectionState.Open) conn.Open();
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    actionList.ForEach(a =>
                    {
                        _effected += a.Action(tran);
                    });
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    _effected = 0;
                    _excetion = ex;
                    _uow.Excetion = ex;
                    tran.Rollback();
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        //本地分布式事务提交 该方法不会提交tran，需要等待工作单元确认或者手动提交
        private void CommitForLocalDistributedTran(List<UnitAction> actionList)
        {
            var action = actionList.First(a => a.Conn != null);
            if (action != null)
            {
                _conn = action.Conn;
                if (_conn.State != ConnectionState.Open) _conn.Open();
                _tran = _conn.BeginTransaction();
                _effected += actionList.Sum(item => item.Action(_tran));
            }
        }
        //分布式事务提交
        private void CommitForDistributedTran(List<UnitAction> actionList)
        {
            using (var scope = new TransactionScope())
            {
                actionList.ForEach(a =>
                {
                    a.Action(null);
                });
                scope.Complete();
            }
            _effected = 1;
        }


        #endregion

        public new void Dispose()
        {
            if (_tran != null)_tran.Dispose();
            if (_conn != null) _conn.Close();
        }

        public void RollBack()
        {
            if (_tran != null)_tran.Rollback();
            if (_conn != null) _conn.Close();
        }
    }

}
