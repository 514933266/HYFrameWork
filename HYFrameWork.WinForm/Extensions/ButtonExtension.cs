using System;
using System.Windows.Forms;

namespace HYFrameWork.WinForm
{
    public static class ButtonExtension
    {
        public static void InvokeText(this Button txt, string msg)
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
    }
}
