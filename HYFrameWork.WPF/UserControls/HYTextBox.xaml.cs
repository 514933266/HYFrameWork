using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HYFrameWork.WPF.UserControls
{
    /// <summary>
    /// HYListBox.xaml 的交互逻辑
    /// </summary>
    public partial class HYTextBox : TextBox
    {
        static HYTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HYTextBox), new FrameworkPropertyMetadata(typeof(HYTextBox)));
        }

        #region 1.0 字段
        public static readonly DependencyProperty TextIsEnableProperty =
            DependencyProperty.Register("TextIsEnable", typeof(bool), typeof(HYTextBox), new PropertyMetadata(true));
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(HYTextBox));
        public static readonly DependencyProperty HoverProperty =
            DependencyProperty.Register("Hover", typeof(ImageSource), typeof(HYTextBox));
        public static readonly DependencyProperty PressedProperty =
        DependencyProperty.Register("Pressed", typeof(ImageSource), typeof(HYTextBox));
        public static readonly DependencyProperty IsEnableImageProperty =
            DependencyProperty.Register("IsEnableImage", typeof(ImageSource), typeof(HYTextBox));
        public static readonly DependencyProperty MouseOverBackgroundProperty =
            DependencyProperty.Register("MouseOverBackground", typeof(Brush), typeof(HYTextBox), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0, 0, 0, 0))));
        public static readonly DependencyProperty MouseOverBorderBrushProperty =
            DependencyProperty.Register("MouseOverBorderBrush", typeof(Brush), typeof(HYTextBox), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0, 0, 0, 0))));
        public static readonly DependencyProperty RadiusProperty =
          DependencyProperty.Register("Radius", typeof(CornerRadius), typeof(HYTextBox), new PropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty ImageMarginProperty = DependencyProperty.Register(
           "ImageMargin", typeof(Thickness), typeof(HYTextBox), new PropertyMetadata(new Thickness(0, 0, 0, 0)));
        public static readonly DependencyProperty ImageWidthProperty = DependencyProperty.Register(
          "ImageWidth", typeof(double), typeof(HYTextBox), new PropertyMetadata(0.0));
        public static readonly DependencyProperty ImageHeightProperty = DependencyProperty.Register(
          "ImageHeight", typeof(double), typeof(HYTextBox), new PropertyMetadata(0.0));
        public static readonly DependencyProperty ImageStretchProperty = DependencyProperty.Register(
         "ImageStretch", typeof(Stretch), typeof(HYTextBox), new PropertyMetadata(Stretch.Uniform));
        public static readonly DependencyProperty WaterRemarkProperty = DependencyProperty.Register(
         "WaterRemark", typeof(string), typeof(HYTextBox), new PropertyMetadata("请输入内容..."));
        public static readonly DependencyProperty TextBoxWidthProperty = DependencyProperty.Register(
          "TextBoxWidth", typeof(double), typeof(HYTextBox), new PropertyMetadata(200.0));
        public static readonly DependencyProperty TextBoxHeightProperty = DependencyProperty.Register(
          "TextBoxHeight", typeof(double), typeof(HYTextBox), new PropertyMetadata(24.0));
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
         "Command", typeof(ICommand), typeof(HYTextBox));
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
        "CommandParameter", typeof(object), typeof(HYTextBox));
        
        #endregion

        #region 2.0 属性
        /// <summary>
        /// 文本按钮按下的命令（用于MVVM）
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        /// <summary>
        /// 命令的参数
        /// </summary>
        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        /// <summary>
        /// 是否启用文本输入
        /// </summary>
        public bool TextIsEnable
        {
            get { return (bool)GetValue(TextIsEnableProperty); }
            set { SetValue(TextIsEnableProperty, value); }
        }
        /// <summary>
        /// 文本框高度
        /// </summary>
        public double TextBoxHeight
        {
            get { return (double)GetValue(TextBoxHeightProperty); }
            set { SetValue(TextBoxHeightProperty, value); }
        }
        /// <summary>
        /// 文本框宽度
        /// </summary>
        public double TextBoxWidth
        {
            get { return (double)GetValue(TextBoxWidthProperty); }
            set { SetValue(TextBoxWidthProperty, value); }
        }
        /// <summary>
        /// HYTextBox注册的水印属性
        /// </summary>
        public string WaterRemark
        {
            get { return (string)GetValue(WaterRemarkProperty); }
            set { SetValue(WaterRemarkProperty, value); }
        }
        /// <summary>
        /// 文本框旁边图标（Icon）的Margin
        /// </summary>
        public Thickness ImageMargin
        {
            get {return (Thickness)GetValue(ImageMarginProperty); }
            set { SetValue(ImageMarginProperty, value); }
        }
        /// <summary>
        /// 文本框旁边图标（Icon）的Stretch
        /// </summary>
        public Stretch ImageStretch
        {

            get { return (Stretch)GetValue(ImageStretchProperty); }
            set { SetValue(ImageStretchProperty, value); }
        }
        /// <summary>
        /// 文本框旁边图标（Icon）的Height
        /// </summary>
        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }
        /// <summary>
        /// 文本框旁边图标（Icon）的Width
        /// </summary>
        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }
        /// <summary>
        /// 控件边框的鼠标Hover样式
        /// </summary>
        public Brush MouseOverBorderBrush
        {
            get { return (Brush)GetValue(MouseOverBorderBrushProperty); }
            set { SetValue(MouseOverBorderBrushProperty, value); }
        }
        /// <summary>
        /// 控件鼠标Hover背景颜色
        /// </summary>
        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }
        /// <summary>
        /// 控件的Icon
        /// </summary>
        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        /// <summary>
        /// 指示鼠标悬浮在按钮上方时的图标
        /// </summary>
        public ImageSource Hover
        {
            get { return (ImageSource)GetValue(HoverProperty); }
            set { SetValue(HoverProperty, value); }
        }
        /// <summary>
        /// 指示鼠标按下在按钮上方时的图标
        /// </summary>
        public ImageSource Pressed
        {
            get { return (ImageSource)GetValue(PressedProperty); }
            set { SetValue(PressedProperty, value); }
        }
        
        /// <summary>
        /// 按钮不可点击时的图片
        /// </summary>
        public ImageSource IsEnableImage
        {
            get { return (ImageSource)GetValue(IsEnableImageProperty); }
            set { SetValue(IsEnableImageProperty, value); }
        }
        /// <summary>
        /// 指示按钮圆角
        /// </summary>
        public CornerRadius Radius
        {
            get { return (CornerRadius)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        #endregion

        #region 3.0 命令
        #endregion

        #region 4.0 方法
        #endregion
    }
}
