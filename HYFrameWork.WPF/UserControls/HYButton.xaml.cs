using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HYFrameWork.WPF.UserControls
{
    /// <summary>
    /// HYButton.xaml 的交互逻辑
    /// </summary>
    public partial class HYButton : Button
    {
        static HYButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HYButton), new FrameworkPropertyMetadata(typeof(HYButton)));
        }
        #region 1.0 字段
        public static readonly DependencyProperty MousePressedBorderBrushProperty =
           DependencyProperty.Register("MousePressedBorderBrush", typeof(Brush), typeof(HYButton), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0, 0, 0, 0))));
        public static readonly DependencyProperty MousePressedBackgroundProperty =
            DependencyProperty.Register("MousePressedBackground", typeof(Brush), typeof(HYButton), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0, 0, 0, 0))));
        public static readonly DependencyProperty MouseOverBackgroundProperty =
            DependencyProperty.Register("MouseOverBackground", typeof(Brush), typeof(HYButton), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0, 0, 0, 0))));
        public static readonly DependencyProperty MouseOverForegroundProperty =
            DependencyProperty.Register("MouseOverForeground", typeof(Brush), typeof(HYButton), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0, 0, 0, 0))));
        public static readonly DependencyProperty MouseOverBorderBrushProperty =
            DependencyProperty.Register("MouseOverBorderBrush", typeof(Brush), typeof(HYButton), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0, 0, 0, 0))));
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(HYButton));
        public static readonly DependencyProperty HoverProperty =
            DependencyProperty.Register("Hover", typeof(ImageSource), typeof(HYButton));
        public static readonly DependencyProperty PressedProperty =
          DependencyProperty.Register("Pressed", typeof(ImageSource), typeof(HYButton));
        public static readonly DependencyProperty IsEnableImageProperty =
           DependencyProperty.Register("IsEnableImage", typeof(ImageSource), typeof(HYButton));
        public static readonly DependencyProperty IsImageVisibilityProperty =
         DependencyProperty.Register("IsImageVisibility", typeof(Visibility), typeof(HYButton), new PropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty IsTextVisibilityProperty =
        DependencyProperty.Register("IsTextVisibility", typeof(Visibility), typeof(HYButton), new PropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty RadiusProperty =
           DependencyProperty.Register("Radius", typeof(CornerRadius), typeof(HYButton), new PropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty TextMarginProperty = DependencyProperty.Register(
          "TextMargin", typeof(Thickness), typeof(HYButton), new PropertyMetadata(new Thickness(0, 0, 0, 0)));
        public static readonly DependencyProperty ImageMarginProperty = DependencyProperty.Register(
           "ImageMargin", typeof(Thickness), typeof(HYButton), new PropertyMetadata(new Thickness(0, 0, 0, 0)));
        public static readonly DependencyProperty ImageOrientationProperty = DependencyProperty.Register(
          "ImageOrientation", typeof(Orientation), typeof(HYButton), new PropertyMetadata(Orientation.Vertical));
        public static readonly DependencyProperty ImageWidthProperty = DependencyProperty.Register(
          "ImageWidth", typeof(double), typeof(HYButton), new PropertyMetadata(0.0));
        public static readonly DependencyProperty ImageHeightProperty = DependencyProperty.Register(
          "ImageHeight", typeof(double), typeof(HYButton), new PropertyMetadata(0.0));
        public static readonly DependencyProperty ImageStretchProperty = DependencyProperty.Register(
         "ImageStretch", typeof(Stretch), typeof(HYButton), new PropertyMetadata(Stretch.Uniform));
        public static readonly DependencyProperty ImagePressedMarginProperty = DependencyProperty.Register(
        "ImagePressedMargin", typeof(Thickness), typeof(HYButton), new PropertyMetadata(new Thickness(0, 0, 0, 0)));
        public static readonly DependencyProperty TextPressedMarginProperty = DependencyProperty.Register(
        "TextPressedMargin", typeof(Thickness), typeof(HYButton), new PropertyMetadata(new Thickness(0, 0, 0, 0)));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
       "Text", typeof(string), typeof(HYButton), new PropertyMetadata(""));
        #endregion

        #region 2.0 属性

        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            get {  return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// 按钮按下时 的图片 偏移量
        /// </summary>
        public Thickness ImagePressedMargin
        {
            get { return (Thickness)GetValue(ImagePressedMarginProperty); }
            set { SetValue(ImagePressedMarginProperty, value); }
        }
        /// <summary>
        /// 按钮按下时 的文本 偏移量
        /// </summary>
        public Thickness TextPressedMargin
        {
            get { return (Thickness)GetValue(TextPressedMarginProperty); }
            set { SetValue(TextPressedMarginProperty, value); }
        }
        public Brush MouseOverBorderBrush
        {
            get { return (Brush)GetValue(MouseOverBorderBrushProperty); }
            set { SetValue(MouseOverBorderBrushProperty, value); }
        }
        /// <summary>
        /// 鼠标按下时的边框样式
        /// </summary>
        public Brush MousePressedBorderBrush
        {
            get { return (Brush)GetValue(MousePressedBorderBrushProperty); }
            set { SetValue(MousePressedBorderBrushProperty, value); }
        }
        public Brush MousePressedBackground
        {
            get { return (Brush)GetValue(MousePressedBackgroundProperty); }
            set { SetValue(MousePressedBackgroundProperty, value); }
        }
        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }

        public Brush MouseOverForeground
        {
            get { return (Brush)GetValue(MouseOverForegroundProperty); }
            set { SetValue(MouseOverForegroundProperty, value); }
        }
        public Stretch ImageStretch
        {

            get { return (Stretch)GetValue(ImageStretchProperty); }
            set { SetValue(ImageStretchProperty, value); }
        }
        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }
        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }
        /// <summary>
        /// 指示图标在水平方向还是垂直方向
        /// </summary>
        public Orientation ImageOrientation
        {
            get { return (Orientation)GetValue(ImageOrientationProperty); }
            set { SetValue(ImageOrientationProperty, value); }
        }
        public Thickness ImageMargin
        {
            get { return (Thickness)GetValue(ImageMarginProperty); }
            set { SetValue(ImageMarginProperty, value); }
        }
        public Thickness TextMargin
        {
            get { return (Thickness)GetValue(TextMarginProperty); }
            set { SetValue(TextMarginProperty, value); }
        }
        /// <summary>
        /// 按钮图片是否可见
        /// </summary>
        public Visibility IsImageVisibility
        {
            get { return (Visibility)GetValue(IsImageVisibilityProperty); }
            set { SetValue(IsImageVisibilityProperty, value); }
        }
        /// <summary>
        /// 按钮文本是否可见
        /// </summary>
        public Visibility IsTextVisibility
        {
            get { return (Visibility)GetValue(IsTextVisibilityProperty); }
            set { SetValue(IsTextVisibilityProperty, value); }
        }
        /// <summary>
        /// 指示按钮正常状态下的图标
        /// </summary>
        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
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
        /// 指示鼠标悬浮在按钮上方时的图标
        /// </summary>
        public ImageSource Hover
        {
            get { return (ImageSource)GetValue(HoverProperty); }
            set { SetValue(HoverProperty, value); }
        }

        /// <summary>
        /// 指示鼠标按下按钮时的图标
        /// </summary>
        public ImageSource Pressed
        {
            get { return (ImageSource)GetValue(PressedProperty); }
            set { SetValue(PressedProperty, value); }

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
