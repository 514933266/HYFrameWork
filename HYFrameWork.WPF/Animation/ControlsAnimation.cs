using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HYFrameWork.WPF.Animation
{
    /// <summary>
    /// Grid 行宽 列高修改类
    /// </summary>
    public class ControlsAnimation 
    {
        static ControlsAnimation()
        {
         
        }
        /// <summary>
        /// 设置控件的【大小、背景】动画效果，
        /// 为0时 高度、宽度默认当前大小
        /// </summary>
        /// <param name="control">要设置动画的控件</param>
        /// <param name="height">要变化的高度</param>
        /// <param name="height">要变化的宽度</param>
        /// <param name="time">动画执行的时间</param>
        public static void DoubleAnimation(object control, double toHeight, double toWidth,double time)
        {
            Type type = control.GetType();
            switch (type.Name)
            {
                case "Image":
                    {
                        //更多控件的情况，模仿编写既是
                    }
                    break;
                case "StackPanel":
                    {
                        StackPanel _stackpanel = (StackPanel)control;
                        if (toHeight >= 0)
                        {
                            DoubleAnimation _heightAnimation = new DoubleAnimation(_stackpanel.ActualHeight, toHeight, new Duration(TimeSpan.FromSeconds(time)));
                            _stackpanel.BeginAnimation(Border.HeightProperty, _heightAnimation, HandoffBehavior.Compose);
                        }
                        if (toWidth >= 0)
                        {
                            DoubleAnimation _widthAnimation = new DoubleAnimation(_stackpanel.ActualWidth, toWidth, new Duration(TimeSpan.FromSeconds(time)));
                            _stackpanel.BeginAnimation(Border.WidthProperty, _widthAnimation, HandoffBehavior.Compose);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
