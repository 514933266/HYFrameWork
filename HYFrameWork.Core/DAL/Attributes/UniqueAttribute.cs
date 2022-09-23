using System;

namespace HYFrameWork.Core
{

    /// <summary>
    /// 数据库约束特性：唯一
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class UniqueAttribute : Attribute
    {
    }
}
