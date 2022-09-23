using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 基础消息类型
    /// </summary>
    public enum BaseErrType
    {
        /// <summary>
        /// 服务器异常
        /// </summary>
        [Description("服务器异常")]
        ServerError = -20002,
        /// <summary>
        /// 请求类型错误
        /// </summary>
        [Description("请求类型错误")]
        RequestTypeError = -20001,
        /// <summary>
        /// 不允许的操作
        /// </summary>
        [Description("不允许的操作")]
        NotAllowed=-10013,
        /// <summary>
        /// 已被封禁
        /// </summary>
        [Description("已被封禁")]
        HasBeenBanned = -10012,
        /// <summary>
        /// 账号或密码错误
        /// </summary>
        [Description("账号或密码错误")]
        AccountOrPasswordError = -10011,
        /// <summary>
        /// 密码错误
        /// </summary>
        [Description("密码错误")]
        PasswordError = -10010,
        /// <summary>
        /// 权限不足
        /// </summary>
        [Description("权限不足")]
        PowerNotEnough = -10009,
        /// <summary>
        /// 超出限制
        /// </summary>
        [Description("超出限制")]
        LengthOverflow = -10008,
        /// <summary>
        /// 空值
        /// </summary>
        [Description("空值")]
        Empty = -10007,
        /// <summary>
        /// 秘钥失效
        /// </summary>
        [Description("秘钥失效")]
        TokenInvalid = -10006,
        /// <summary>
        /// 秘钥错误
        /// </summary>
        [Description("秘钥错误")]
        TokenError = -10005,
        /// <summary>
        /// 时间戳失效
        /// </summary>
        [Description("时间戳失效")]
        TimeStampInvalid = -10004,
        /// <summary>
        /// 信息不匹配
        /// </summary>
        [Description("信息不匹配")]
        NotMatch = -10003,
        /// <summary>
        /// 信息不存在
        /// </summary>
        [Description("信息不存在")]
        NotFound = -10002,
        /// <summary>
        /// 信息已存在
        /// </summary>
        [Description("信息已存在")]
        Exists = -10001,
        /// <summary>
        /// 数据异常
        /// </summary>
        [Description("数据异常")]
        DataError = -10000,

        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        Fail = 0,
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 1,
    }
}
