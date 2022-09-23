using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HYFrameWork.WinForm.Active
{
    public partial class Agreement : Form
    {
        public Agreement()
        {
            InitializeComponent();
            InitializeComponent2();

            //formTimer = new Timer() { Interval = 100 };
            //formTimer.Tick += new EventHandler(formTimer_Tick);
            //base.Opacity = 0;
            this.Text ="皓月非凡网络科技-使用协议(QQ:2063162649|736229812)";
        }

        private void Agreement_Load(object sender, EventArgs e)
        {
        }
        void InitializeComponent2()
        {
            //图像配置区
            Bitmap bp = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            int fontSize = 10;
            int[] margin = { 9, 10, 9, 30 };//内容的magin
            int lineHeight = margin[0] + fontSize + margin[2];//文本框原始高度
            int rowHeight = 0;//行高
            int rowWidth = bp.Width - margin[1] - margin[3];//行宽
            int rowLocationX = margin[3];//行的X起点
            int lineLocationX_end = bp.Width - margin[3];//线的终点X起始位置
            int rowLocationY = 0;//行的Y起点
            int lineLocationY_end = 0;//线的Y终点
            int borderHeight = 1;//表格线的高度
            int strWidth = 0;//字符内容所占的像素
            int lineCount = 0;//每行内容所占的行数
            Graphics g = Graphics.FromImage(bp);
            Font font =null;
            Color color = Color.IndianRed;
            SolidBrush solidBrush = new SolidBrush(color);
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            Pen pen = new Pen(solidBrush, 1);
            pen.DashStyle = DashStyle.Custom;
            pen.DashPattern = new float[] { 5f, 1f };
            Graphics vGraphics = CreateGraphics();
            RectangleF container;
            //内容绘制
            List<string> list = TableMsg();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                    font = new Font("宋体", 13, FontStyle.Bold);
                else
                    font = new Font("宋体", fontSize, FontStyle.Regular);
                SizeF vSizeF = vGraphics.MeasureString(list[i], font);
                strWidth = Convert.ToInt32(Math.Ceiling(vSizeF.Width));
                lineCount = strWidth / (bp.Width - margin[1] - margin[3]);

               

                rowLocationY = (i == 0 ? 0 : lineLocationY_end + borderHeight);
                rowHeight = lineHeight + (fontSize + margin[0]) * lineCount;
                lineLocationY_end = i == 0 ? lineHeight : rowLocationY + rowHeight;
                //绘制文本的矩形大小（用于设定文字水平居中/垂直居中）
                if (i < 2)
                {
                    format.Alignment = StringAlignment.Center;
                    container = new RectangleF(new PointF(20, rowLocationY), new SizeF(rowWidth, rowHeight));
                }
                else
                {
                    format.Alignment = StringAlignment.Near;
                    container = new RectangleF(new PointF(rowLocationX, rowLocationY), new SizeF(rowWidth, rowHeight));
                }
                g.DrawString(list[i].ToString(), font, solidBrush, container, format);
                g.DrawLine(pen, new Point(rowLocationX, lineLocationY_end), new Point(lineLocationX_end, lineLocationY_end));
            }
            this.pictureBox1.Image = bp;
        }
        /// <summary>
        /// 设定文本内容
        /// </summary>
        private List<string> TableMsg() {
            List<string> list = new List<string>();
            list.Add("一旦使用本软件，即代表同意以下协议\r\n");
            list.Add("本软件不会捆绑木马、病毒，但网络复杂多变,使用请进行安全检查\r\n");
            list.Add("★1、本软件为专业付费定制,可用于技术交流，请勿用于商业买卖,我们将保留追究法律责任的权利\r\n");
            list.Add("★2、使用者对使用结果承担所有责任,如出现任何不良后果,作者概不负责,同时不承担任何法律责任\r\n");
            list.Add("★3、软件正式交付后,所有程序自身漏洞,均可永久免费维护\r\n");
            list.Add("★4、如有发现本软件对您有任何侵犯嫌疑,请联系我们,我们将在第一时间予以修改和删除,确保您的权益\r\n");
            list.Add("★5、本软件由皓月非凡网络科技制作,最终解释权归作者所有\r\n");
            list.Add("★6、官方网站：www.haoyuefeifan.com\r\n");
            
            return list;
        }

        private void btndisagree_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }
        private void btnagree_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
        #region 窗体逐渐显示
        private Timer formTimer = null;

        /// <summary>
        /// 获取Opacity属性
        /// </summary>
        [DefaultValue(0)]
        [Browsable(false)]
        public new double Opacity
        {
            get { return base.Opacity; }
            set { base.Opacity = 0; }
        }
        private void formTimer_Tick(object sender, EventArgs e)
        {
            if (this.Opacity >= 1)
                formTimer.Stop();
            else
                base.Opacity += 0.2;
        }
        private void Agreement_Shown(object sender, EventArgs e)
        {
            //formTimer.Start();
        }


        #endregion

        //跳转
        private void skinPictureBox1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.haoyuefeifan.com");
        }
    }
}
