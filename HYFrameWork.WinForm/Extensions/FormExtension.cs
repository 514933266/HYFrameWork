using System;
using System.Windows.Forms;

namespace HYFrameWork.WinForm
{
    public static class FormExtension
    {
        public static void InvokeText(this Form form, string msg)
        {
            if (form.InvokeRequired)
            {
                form.Invoke(new Action(() =>
                {
                    form.Text = msg;
                }));
            }
            else
            {
                form.Text = msg;
            }
        }
    }
}
