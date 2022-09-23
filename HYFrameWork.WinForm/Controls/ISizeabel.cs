using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HYFrameWork.WinForm
{
    /// <summary>
    /// 变大
    /// </summary>
    public interface ISizeabel<T> where T : System.Windows.Forms.Control
    {
       void ChangeWidth(T t,int width,int speed);
       void ChangeHeight(T t, int height, int speed);
    }
}
