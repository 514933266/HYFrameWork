using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HYFrameWork.WinForm.Extensions
{
    /// <summary>
    /// 下拉框
    /// </summary>
   public static class ComboBoxExtension
    {
        /// <summary>
        /// 获取选中元素的下标
        /// </summary>
        /// <param name="cBox">下拉框控件</param>
        /// <returns>选中的item的index</returns>
        public static int InvokeSelectedIndex(this ComboBox cBox)
        {
            int selectedIndex = -1;
            if (cBox.InvokeRequired)
            {
                cBox.Invoke(new Action(() =>
                {
                    selectedIndex = cBox.SelectedIndex;
                }));
            }
            else
            {
                selectedIndex = cBox.SelectedIndex;
            }
            return selectedIndex;
        }
        /// <summary>
        /// 获取选中元素的值
        /// </summary>
        /// <param name="cBox">下拉框控件</param>
        /// <returns>选中的item的值</returns>
        public static object InvokeGetSelectedItem(this ComboBox cBox)
        {
            object item = null;
            bool invokeRequired = cBox.InvokeRequired;
            if (invokeRequired)
            {
                cBox.Invoke(new Action(delegate
                {
                    item = cBox.SelectedItem;
                }));
            }
            return item;
        }
    }
}
