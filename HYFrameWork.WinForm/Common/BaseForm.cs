using System.Windows.Forms;
using HYFrameWork.WinForm.Controls;
using HYFrameWork.Core;

namespace HYFrameWork.WinForm
{
    /// <summary>
    /// 窗体父类
    /// </summary>
    public class BaseForm : Form
    {

        /// <summary>
        /// 提示消息
        /// </summary>
        /// <param name="msg"></param>
        protected void ShowAlert(string msg)
        {
            MessageBox.Show(msg);
        }

        /// <summary>
        /// 绑定下拉框
        /// </summary>
        protected void BindCombobox(ComboBox cmb, System.Type enumValue)
        {
            var dict = TypeHelper.ToDic(enumValue);
            var bs = new BindingSource
            {
                DataSource = dict
            };
            cmb.DataSource = bs;
            cmb.DisplayMember = "Key";
            cmb.ValueMember = "Value";
        }

       


    }
}
