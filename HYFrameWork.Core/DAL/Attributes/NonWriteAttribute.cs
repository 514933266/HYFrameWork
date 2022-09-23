using System;

namespace HYFrameWork.Core
{

    /// <summary>
    /// 自定义数据库特性：忽略，添加该特性后,如没有特殊指定将不作为查询字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class NonWriteAttribute : Attribute
    {
    }
}
