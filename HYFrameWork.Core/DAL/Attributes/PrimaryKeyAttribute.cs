using System;

namespace HYFrameWork.Core
{

    /// <summary>
    /// 数据库约束特性：主键
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute
    {
    }
}
