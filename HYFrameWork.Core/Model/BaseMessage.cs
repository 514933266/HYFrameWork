using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 基础响应信息对象
    /// </summary>
    public class BaseMessage
    {

        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 错误类型
        /// </summary>
        public int ErrType { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 数据内容
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 获取Data的具体类型对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>T值</returns>
        public T GetData<T>() where T : class
        {
            return (Data as T);
        }
    }
}
