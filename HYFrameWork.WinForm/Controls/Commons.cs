using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace HYFrameWork.WinForm.Controls
{
    public class Commons<T> : ISizeabel<T> where T : System.Windows.Forms.Control
    {
        public delegate void ChangeControlSize(T t, int width, int height, int speed);

        /// <summary>
        /// 异步改变控件宽度（渐变）
        /// </summary>
        /// <param name="t">控件类型</param>
        /// <param name="width">要改变的宽度</param>
        /// <param name="speed">步长速度（当为负值执行缩小，反之增大）</param>
        public void BeginChangeWidth(T t, int width, int speed)
        {
            ChangeControlSize _delegate_w = new ChangeControlSize(Change);
            t.BeginInvoke(_delegate_w, new object[] { t, width,0, speed });
        }
        /// <summary>
        /// 异步改变控件宽度（渐变）
        /// </summary>
        /// <param name="t">控件类型</param>
        /// <param name="height">要改变的高度</param>
        /// <param name="speed">步长速度（当为负值执行缩小，反之增大）</param>
        public void BeginChangeHeight(T t, int height, int speed)
       {
           ChangeControlSize _delegate_h = new ChangeControlSize(Change);
           t.BeginInvoke(_delegate_h, new object[] { t,0,height, speed });
       }
        /// <summary>
        /// 异步改变控件宽度（渐变）
        /// </summary>
        /// <param name="t">控件类型</param>
        /// <param name="width">要改变的宽度</param>
        /// <param name="speed">步长速度（当为负值执行缩小，反之增大）</param>
        public void ChangeWidth(T t, int width, int speed)
        {
            ChangeControlSize _delegate_w = new ChangeControlSize(Change);
            t.Invoke(_delegate_w, new object[] { t, width, 0, speed });
        }
        /// <summary>
        /// 异步改变控件宽度（渐变）
        /// </summary>
        /// <param name="t">控件类型</param>
        /// <param name="height">要改变的高度</param>
        /// <param name="speed">步长速度（当为负值执行缩小，反之增大）</param>
        public void ChangeHeight(T t, int height, int speed)
        {
            ChangeControlSize _delegate_h = new ChangeControlSize(Change);
            t.Invoke(_delegate_h, new object[] { t, 0, height, speed });
        }
        private void Change(T t, int width, int height, int speed)
        {
            if (width == 0 && height > 0)
            {
               while(speed > 0 ? t.Height <= height : t.Height >= height)
                {
                    t.Height += speed;
                }
            }
            else if (height == 0 && width > 0)
            {
                while (speed > 0 ? t.Width <= width : t.Width >= width)
                {
                    t.Width += speed;
                }
            }
        }
        /// <summary>
        /// 异步改变控件Size（渐变）
        /// </summary>
        /// <param name="t">控件类型</param>
        /// <param name="width">要改变的宽度</param>
        /// <param name="height">要改变的高度</param>
        /// <param name="speed">步长速度（当为负值执行缩小，反之增大）</param>
        public void BeginChangeSize(T t, int width, int height, int speed)
        {
            BeginChangeWidth(t, width, speed);
            BeginChangeHeight(t, height, speed);
        }
        public void ChangeSize(T t, int width, int height, int speed)
        {
                ChangeWidth(t, width, speed);
                ChangeHeight(t, height, speed);
        }
        /// <summary>
        /// 获取某个窗体下的控件
        /// </summary>
       public System.Windows.Forms.Control GetFormControl(Form form, string cName)
       {
           if (form != null)
               return form.Controls[cName];
           else
               return new System.Windows.Forms.Control();
       }

    }
}
