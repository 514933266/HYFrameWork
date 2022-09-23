namespace HYFrameWork.Core
{
    /// <summary>
    /// 数据库事务类型
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        /// 无事务
        /// </summary>
        None = 0,

        /// <summary>
        /// 本地事务（此类型事务出错时仅能回滚自身操作）
        /// </summary>
        Local = 1,
        /// <summary>
        /// 分布式事务（配合UnitOfWork使用，可以对多个事务进行管理，每个事务的conn必须一致）
        /// </summary>
        LocalDistribute=2,
        /// <summary>
        /// 分布式事务(TransactionScope实现,自动管理代码块，无影响值返回，每个事务的conn不须一致)
        /// </summary>
        Distribute = 3,

        /// <summary>
        /// 补偿事务
        /// </summary>
        Compensate = 4
    }


    /// <summary>
    /// 数据库锁对象
    /// </summary>
    public static class DbLock
    {
        /// <summary>
        /// 默认设置
        /// </summary>
        public const string Default = "";

        /// <summary>
        /// 不添加共享锁和排它锁，可能读到未提交读的数据或“脏数据”
        /// </summary>
        public const string NoLock = "(NOLOCK)";
    }

    /// <summary>
    /// 日期类型
    /// </summary>
    public enum DateEnum
    {
        Year,
        Month,
        Week,
        Day,
        Hours,
        Minutes,
        Seconds
    }
}
