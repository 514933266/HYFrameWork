using System;
using System.Windows.Forms;

namespace HYFrameWork.WinForm
{
    /// <summary>
    /// 标签扩展类
    /// </summary>
    public static class LabelExtension
    {
        public static void InvokeText(this Label lbl, string text)
        {
            if (lbl.InvokeRequired)
            {
                lbl.Invoke(new Action(() =>
                {
                    lbl.Text = text;
                }));
            }
            else
            {
                lbl.Text = text;
            }
        }
    }
}
