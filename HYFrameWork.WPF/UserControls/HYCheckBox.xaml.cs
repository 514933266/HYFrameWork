using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HYFrameWork.WPF.UserControls
{
    /// <summary>
    /// HYCheckBox.xaml 的交互逻辑
    /// </summary>
    public partial class HYCheckBox : CheckBox
    {
        DependencyObject _dependencyObject = new DependencyObject();
        public HYCheckBox()
        {
        }

        public static readonly DependencyProperty RadiusProperty =
          DependencyProperty.Register("Radius", typeof(CornerRadius), typeof(HYCheckBox), new PropertyMetadata(new CornerRadius(0)));

        public static readonly DependencyProperty TickColorProperty =
         DependencyProperty.Register("TickColor", typeof(Color), typeof(HYCheckBox), new PropertyMetadata(Color.FromArgb(0, 0, 0, 0)));

        public static readonly DependencyProperty ContentProperty =
        DependencyProperty.Register("Content", typeof(object), typeof(HYCheckBox));

        public static readonly DependencyProperty MouseOverBorderBrushProperty =
           DependencyProperty.Register("MouseOverBorderBrush", typeof(Brush), typeof(HYCheckBox));
        #region 属性
        public Brush MouseOverBorderBrush
        {
            get { return (Brush)_dependencyObject.GetValue(MouseOverBorderBrushProperty); }
            set { _dependencyObject.SetValue(MouseOverBorderBrushProperty, value); }
        }
        /// <summary>
        /// 圆角
        /// </summary>
        public CornerRadius Radius
        {
            get { return (CornerRadius)_dependencyObject.GetValue(RadiusProperty); }
            set { _dependencyObject.SetValue(RadiusProperty, value); }
        }
        /// <summary>
        /// 勾勾颜色
        /// </summary>
        public Color TickColor
        {
            get { return (Color)_dependencyObject.GetValue(TickColorProperty); }
            set { _dependencyObject.SetValue(TickColorProperty, value); }
        }
        /// <summary>
        /// 选择框的内容
        /// </summary>
        public object Content
        {
            get { return (object)_dependencyObject.GetValue(ContentProperty); }
            set { _dependencyObject.SetValue(ContentProperty, value); }
        }
        #endregion
    }
}
