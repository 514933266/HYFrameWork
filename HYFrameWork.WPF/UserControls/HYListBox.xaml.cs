using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HYFrameWork.WPF.UserControls
{
   public partial class HYListBox:ListBox
    {
        public HYListBox()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(HYListBox), new FrameworkPropertyMetadata(typeof(HYListBox)));
        }

        #region 1.0 字段
        public static readonly DependencyProperty RadiusProperty =
          DependencyProperty.Register("Radius", typeof(CornerRadius), typeof(HYListBox), new PropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty MouseOverBorderBrushProperty =
            DependencyProperty.Register("MouseOverBorderBrush", typeof(Brush), typeof(HYListBox), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0, 0, 0, 0))));
        public static readonly DependencyProperty ItemOrientationProperty = DependencyProperty.Register(
         "ItemOrientation", typeof(Orientation), typeof(HYListBox), new PropertyMetadata(Orientation.Vertical));
        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register(
         "ItemWidth", typeof(double), typeof(HYListBox), new PropertyMetadata(0.0));
        public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(
          "ItemHeight", typeof(double), typeof(HYListBox), new PropertyMetadata(0.0));
        #endregion

        #region 2.0 属性
        /// <summary>
        /// 指示按钮圆角
        /// </summary>
        public CornerRadius Radius
        {
            get { return (CornerRadius)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        public Brush MouseOverBorderBrush
        {
            get { return (Brush)GetValue(MouseOverBorderBrushProperty); }
            set { SetValue(MouseOverBorderBrushProperty, value); }
        }
        /// <summary>
        /// 指示图标在水平方向还是垂直方向
        /// </summary>
        public Orientation ItemOrientation
        {
            get { return (Orientation)GetValue(ItemOrientationProperty); }
            set { SetValue(ItemOrientationProperty, value); }
        }
        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }
        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }
        #endregion

        #region 3.0 命令
        #endregion

        #region 4.0 方法
        #endregion
    }
}
