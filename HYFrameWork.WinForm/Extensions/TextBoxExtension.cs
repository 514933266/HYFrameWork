using System;
using System.Windows.Forms;
using HYFrameWork.Core;

namespace HYFrameWork.WinForm
{
    public static class TextBoxExtension
    {
        /// <summary>
        /// 追加日志格式文本到输入框（【YYYY年MM月DD日 HH时mm分ss秒】:你好！
        /// </summary>
        /// <param name="txt">输入框控件</param>
        /// <param name="msg">文本</param>
        public static void InvokeAppendLog(this TextBox txt, string msg)
        {
            txt.InvokeAppendText("【" + TimeHelper.GetChineseTickDate(DateTime.Now) + "】：" + msg + "\r\n");
        }

        /// <summary>
        /// 日志显示
        /// </summary>
        public static void InvokeShowLog(this TextBox txt, string msg)
        {
            txt.InvokeText(msg);
        }
        /// <summary>
        /// 清空文本框
        /// </summary>
        public static void InvokeClear(this TextBox txt)
        {
            if (txt.InvokeRequired)
            {
                txt.Invoke(new Action(() =>
                {
                    txt.Clear();
                }));
            }
            else
            {
                txt.Clear();
            }
        }
        /// <summary>
        /// 设置文本值到输入框
        /// </summary>
        /// <param name="txt">输入框控件</param>
        /// <param name="msg">文本</param>
        public static void InvokeText(this TextBox txt, string msg)
        {
            if (txt.InvokeRequired)
            {
                txt.Invoke(new Action(() =>
                {
                    txt.Text = msg;
                }));
            }
            else
            {
                txt.Text = msg;
            }
        }

        /// <summary>
        /// 追加文本到输入框
        /// </summary>
        /// <param name="txt">输入框对象</param>
        /// <param name="msg">文本</param>
        public static void InvokeAppendText(this TextBox txt, string msg)
        {
            if (txt.InvokeRequired)
            {
                txt.Invoke(new Action(() =>
                {
                    txt.AppendText(msg);
                }));
            }
            else
            {
                txt.AppendText(msg);
            }
        }

    }
}
