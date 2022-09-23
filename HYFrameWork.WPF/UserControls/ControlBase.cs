using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace HYFrameWork.WPF.UserControls
{
    /// <summary>
    /// 必须实现INotifyPropertyChanged的类才可以使用双向属性绑定更新
    /// </summary>
    public class ControlBase :UserControl,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        #region 1.0 字段
        #endregion
        #region 2.0 属性
        #endregion
        #region 3.0 命令
        #endregion
        #region 4.0 方法
        #endregion
    }
}
