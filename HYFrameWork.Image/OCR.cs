using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace HYFrameWork.Image
{
    /// <summary>
    ///  本类用于识别验证码,具体使用顺序请根据实际搭配
    ///  本类有待优化
    /// </summary>
   public class OCR
   {
       #region 20161219 整合
       public Bitmap _bp;
       public OCR(Bitmap bp)
        {
            _bp = new Bitmap(bp);    //转换为Format32_bppRgb
        }
       /// <summary>
       /// 将图像边框变成白色
       /// </summary>
        public Bitmap ClearBorder(int width)
        {
            try
            {
                for (int i = 0; i < _bp.Width; i++)
                {
                    for (int j = 0; j < _bp.Height; j++)
                    {
                        if (i < width || i >= _bp.Width - width || j < width || j >= _bp.Height - width)
                            _bp.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                        else
                            continue;
                    }
                }
            }
            catch
            {
                return null;
            }
            return _bp.Clone() as Bitmap;
        }
        #region 获取有色图像中的主题颜色值
        public Color get_major_color()
        {
            //色调的总和
            var sum_hue = 0d;
            //色差的阈值
            var threshold = 30;
            //计算色调总和
            for (int h = 0; h < _bp.Height; h++)
            {
                for (int w = 0; w < _bp.Width; w++)
                {
                    var hue = _bp.GetPixel(w, h).GetHue();
                    sum_hue += hue;
                }
            }
            var avg_hue = sum_hue / (_bp.Width * _bp.Height);

            //色差大于阈值的颜色值
            var rgbs = new List<Color>();
            for (int h = 0; h < _bp.Height; h++)
            {
                for (int w = 0; w < _bp.Width; w++)
                {
                    var color = _bp.GetPixel(w, h);
                    var hue = color.GetHue();
                    //如果色差大于阈值，则加入列表
                    if (Math.Abs(hue - avg_hue) > threshold)
                    {
                        rgbs.Add(color);
                    }
                }
            }
            if (rgbs.Count == 0)
                return Color.Black;
            //计算列表中的颜色均值，结果即为该图片的主色调
            int sum_r = 0, sum_g = 0, sum_b = 0;
            foreach (var rgb in rgbs)
            {
                sum_r += rgb.R;
                sum_g += rgb.G;
                sum_b += rgb.B;
            }
            return Color.FromArgb(sum_r / rgbs.Count,
                sum_g / rgbs.Count,
                sum_b / rgbs.Count);
        }
        #endregion

        #region 灰度

        /// <summary>
       /// 图片灰度化(并去除边框)
       /// Type1:f(i,j)=(R(i,j)+G(i,j)+B(i,j)) /3 平均法
       /// Type2:f(i,j)=0.30*R(i,j)+0.59*G(i,j)+0.11*B(i,j) 加权平均法
       /// Type3:f(i,j)=min(R(i,j),G(i,j),B(i,j)) 最小亮度法
       /// </summary>
       /// <returns></returns>
       public Bitmap ImgToGray(int type)
        {
            int gray = 0;
            try
            {
                for (int i = 0; i < _bp.Width; i++)
                {
                    for (int j = 0; j < _bp.Height; j++)
                    {
                        Color color = _bp.GetPixel(i, j);
                        if (type == 1)
                            gray = (int)((color.R + color.G + color.B) / 3);
                        else if (type == 2)
                            gray = (int)(0.30 * color.R + 0.59 * color.G + 0.11 * color.B);
                        else if (type == 3)
                        {
                            int[] arr = { color.R, color.G, color.B };
                            System.Collections.ArrayList list = new System.Collections.ArrayList(arr);
                            list.Sort();
                            gray = Convert.ToInt32(list[0]); 
                        }
                        _bp.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                    }
                }
            }
            catch
            {
                return null;
            }
            return _bp.Clone() as Bitmap;
        }
       /// <summary>
       /// 使用指针生成灰度图像 只能够在允许不安全代码模式时使用
       /// 修改公共_bp为灰度图
       /// 同时返回8_bp灰度图
       /// </summary>
       /// <returns></returns>
       public  Bitmap ImgToGrayForPtr()
       {
           Rectangle rect = new Rectangle(0, 0, _bp.Width, _bp.Height);
           BitmapData _bpData = _bp.LockBits(rect, ImageLockMode.ReadWrite, _bp.PixelFormat);
           byte temp = 0;
           unsafe
           {
               byte* ptr = (byte*)(_bpData.Scan0);
               for (int i = 0; i < _bpData.Width; i++)
               {
                   for (int j = 0; j < _bpData.Height; j++)
                   {
                       temp = (byte)(0.299 * ptr[2] + 0.587 * ptr[1] + 0.114 * ptr[0]);
                       ptr[0] = ptr[1] = ptr[2] = temp;
                       ptr += 4;//这里的4，是指每个像素的字节数
                   }
                   ptr += _bpData.Stride - _bpData.Width * 4;//偏移量
               }
           }
           _bp.UnlockBits(_bpData);
           
           return _bp.Clone() as Bitmap;
       }
       #endregion

        #region 阈值
       /// <summary>
       /// 得到灰度图像前景背景的临界值 最大类间方差法
       /// otsu使用图像的统计信息进行阈值计算，在场景比较简单的情况下（即背景和目标占图像像素中的大部分）使用最好
       /// </summary>
       /// <returns>前景背景的阈值</returns>
       public int GetDgGrayValue()
       {
           int[] pixelNum = new int[256];           //图象直方图，共256个点
           int n, n1, n2;
           int total;                              //total为总和，累计值
           double m1, m2, sum, csum, fmax, sb;     //sb为类间方差，fmax存储最大方差值
           int k, t, q;
           int threshValue = 1;                      // 阈值
           int step = 1;
           try
           {
               //生成直方图
               for (int i = 0; i < _bp.Width; i++)
               {
                   for (int j = 0; j < _bp.Height; j++)
                   {
                       //返回各个点的颜色，以RGB表示
                       pixelNum[_bp.GetPixel(i, j).R]++;            //相应的直方图加1
                   }
               }
               //直方图平滑化
               for (k = 0; k <= 255; k++)
               {
                   total = 0;
                   for (t = -2; t <= 2; t++)              //与附近2个灰度做平滑化，t值应取较小的值
                   {
                       q = k + t;
                       if (q < 0)                     //越界处理
                           q = 0;
                       if (q > 255)
                           q = 255;
                       total = total + pixelNum[q];    //total为总和，累计值
                   }
                   pixelNum[k] = (int)((float)total / 5.0 + 0.5);    //平滑化，左边2个+中间1个+右边2个灰度，共5个，所以总和除以5，后面加0.5是用修正值
               }
               //求阈值
               sum = csum = 0.0;
               n = 0;
               //计算总的图象的点数和质量矩，为后面的计算做准备
               for (k = 0; k <= 255; k++)
               {
                   sum += (double)k * (double)pixelNum[k];     //x*f(x)质量矩，也就是每个灰度的值乘以其点数（归一化后为概率），sum为其总和
                   n += pixelNum[k];                       //n为图象总的点数，归一化后就是累积概率
               }

               fmax = -1.0;                          //类间方差sb不可能为负，所以fmax初始值为-1不影响计算的进行
               n1 = 0;
               for (k = 0; k < 256; k++)                  //对每个灰度（从0到255）计算一次分割后的类间方差sb
               {
                   n1 += pixelNum[k];                //n1为在当前阈值遍前景图象的点数
                   if (n1 == 0) { continue; }            //没有分出前景后景
                   n2 = n - n1;                        //n2为背景图象的点数
                   if (n2 == 0) { break; }               //n2为0表示全部都是后景图象，与n1=0情况类似，之后的遍历不可能使前景点数增加，所以此时可以退出循环
                   csum += (double)k * pixelNum[k];    //前景的“灰度的值*其点数”的总和
                   m1 = csum / n1;                     //m1为前景的平均灰度
                   m2 = (sum - csum) / n2;               //m2为背景的平均灰度
                   sb = (double)n1 * (double)n2 * (m1 - m2) * (m1 - m2);   //sb为类间方差
                   if (sb > fmax)                  //如果算出的类间方差大于前一次算出的类间方差
                   {
                       fmax = sb;                    //fmax始终为最大类间方差（otsu）
                       threshValue = k;              //取最大类间方差时对应的灰度的k就是最佳阈值
                   }
               }
           }
           catch
           {
               return 0;
           }
           return threshValue;
       }
       #endregion

        #region 二值化
       /// <summary>
       /// 二值化 传入阈值
       /// </summary>
       /// <returns></returns>
       public Bitmap ClearInterference_TwoValue(int Threshold)
       {
           for (int i = 0; i < _bp.Width; i++)
           {
               for (int j = 0; j < _bp.Height; j++)
               {
                   Color color = _bp.GetPixel(i, j);
                   Color newColor = color.R > Threshold ? Color.FromArgb(0, 0, 0) : Color.FromArgb(255, 255, 255);
                   _bp.SetPixel(i, j, newColor);
               }
           }
           return _bp.Clone() as Bitmap;
       }
       /// <summary>
       /// 二值化 传入阈值  灰度图像 白色纯背景使用
       /// </summary>
       /// <returns></returns>
       public Bitmap ClearInterference_TwoValue_2()
       {
           for (int i = 0; i < _bp.Width; i++)
           {
               for (int j = 0; j < _bp.Height; j++)
               {
                   Color color = _bp.GetPixel(i, j);
                   Color newColor = color.R != 255 && color.G != 255 && color.B != 255 ? Color.FromArgb(0, 0, 0) : Color.FromArgb(255, 255, 255);
                   _bp.SetPixel(i, j, newColor);
               }
           }
           return _bp.Clone() as Bitmap;
       }
       /// <summary>
       /// 通过指针二值化 传入阈值
       /// </summary>
       /// <returns></returns>
       public Bitmap ClearInterference_TwoValueForPtr(int Threshold)
       {
           Rectangle rect = new Rectangle(0, 0, _bp.Width, _bp.Height);
           BitmapData _bpData = _bp.LockBits(rect, ImageLockMode.ReadWrite, _bp.PixelFormat);
           unsafe
           {
               byte* data = (byte*)(_bpData.Scan0);
               int step = _bpData.Stride;
               for (int y = 0; y < _bp.Height; y++)
               {
                   for (int x = 0; x < _bp.Width * 3; x += 3)
                   {
                       if (data[y * step + x + 2] > Threshold)
                           data[y * step + x]
                               = data[y * step + x + 1]
                               = data[y * step + x + 2]
                               = 255;
                       else
                           data[y * step + x]
                               = data[y * step + x + 1]
                               = data[y * step + x + 2]
                               = 0;
                   }
               }
               _bp.UnlockBits(_bpData);
           }
           return _bp.Clone() as Bitmap;
       }

       #endregion

        #region 反色

       /// <summary>
       /// 根据色块数量反色图像
       /// </summary>
       /// <returns></returns>
       public Bitmap TrunGrayBitmap()
       {
           int white_count = 0;
           int black_count = 0;
           try
           {
               for (int i = 0; i < _bp.Width; i++)
               {
                   for (int j = 0; j < _bp.Height; j++)
                   {
                       Color color = _bp.GetPixel(i, j);
                       if (color.R == 0 && color.G == 0 && color.B == 0)
                           white_count++;
                       else
                           black_count++;
                   }

               }
               if (white_count > black_count)
               {
                   for (int i = 0; i < _bp.Width; i++)
                   {

                       for (int j = 0; j < _bp.Height; j++)
                       {
                           Color color = _bp.GetPixel(i, j);
                           Color newColor = Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);
                           _bp.SetPixel(i, j, newColor);
                       }

                   }
               }
           }
           catch
           {
               return null;
           }
           return _bp.Clone() as Bitmap;
       }
       /// <summary>
       /// 通过指针处理图像反色，当图像较大时此方法运行速度更高
       /// </summary>
       /// <param name="thispic"></param>
       /// <returns></returns>
       public Bitmap TrunGrayBitmapForPtr()
       {
           Rectangle rect = new Rectangle(0, 0, _bp.Width, _bp.Height);
           BitmapData _bpData = _bp.LockBits(rect, ImageLockMode.ReadWrite, _bp.PixelFormat);
           unsafe 
           {
               byte* pix = (byte*)_bpData.Scan0; 
               for (int i = 0; i < _bpData.Stride * _bpData.Height; i++)
                   pix[i] = (byte)(255 - pix[i]);
           }
           _bp.UnlockBits(_bpData); 
           return _bp.Clone() as Bitmap;
       }
       #endregion

        #region 去躁
       /// <summary>
       /// 3*3中值滤波
       /// 当噪点数量一般或比较零散，建议用中值
       /// </summary>
       /// <returns></returns>
       public Bitmap ClearPoint_med_avg()
       {
           if (_bp != null)
           {
               int x, y;
               byte[] p = new byte[9]; //最小处理窗口3*3
               byte s;
               //byte[] lpTemp=new BYTE[nByteWidth*nHeight];
               int i, j;

               //--!!!!!!!!!!!!!!下面开始窗口为3×3中值滤波!!!!!!!!!!!!!!!!
               for (y = 1; y < _bp.Height - 1; y++) //--第一行和最后一行无法取窗口
               {
                   for (x = 1; x < _bp.Width - 1; x++)
                   {
                       //取9个点的值
                       p[0] = _bp.GetPixel(x - 1, y - 1).R;
                       p[1] = _bp.GetPixel(x, y - 1).R;
                       p[2] = _bp.GetPixel(x + 1, y - 1).R;
                       p[3] = _bp.GetPixel(x - 1, y).R;
                       p[4] = _bp.GetPixel(x, y).R;
                       p[5] = _bp.GetPixel(x + 1, y).R;
                       p[6] = _bp.GetPixel(x - 1, y + 1).R;
                       p[7] = _bp.GetPixel(x, y + 1).R;
                       p[8] = _bp.GetPixel(x + 1, y + 1).R;
                       //计算中值
                       for (j = 0; j < 5; j++)
                       {
                           for (i = j + 1; i < 9; i++)
                           {
                               if (p[j] > p[i])
                               {
                                   s = p[j];
                                   p[j] = p[i];
                                   p[i] = s;
                               }
                           }
                       }
                       _bp.SetPixel(x, y, Color.FromArgb(p[4], p[4], p[4]));    //给有效值付中值
                   }
               }
           }
           return _bp.Clone() as Bitmap;
       }
       /// <summary>
       ///  3*3,5*5 阈值滤波
       /// </summary>
       /// <param name="dgGrayValue">背前景灰色界限,当为二值化图像时，传入0</param>
       /// <param name="MaxNearPoints">最大像素点个数，如果传入4，则当周围像素点个黑色个数小于4个时，认为是噪点</param>
       /// <param name="MaxNearPoints2">最大像素点个数，如果传入4，则当周围像素点个黑色个数大于等于4个时，认为需要填充</param>
       ///<param name="borderWidth">要去除的边框宽度</param>
       /// <returns></returns>
       public Bitmap ClearNoise(int dgGrayValue, int MaxNearPoints, int MaxNearPoints2, int sqareWidth)
       {
           Color piexl;
           int nearDots = 0;
           try
           {
               //逐点判断
               for (int i = 0; i < _bp.Width; i++)
               {
                   for (int j = 0; j < _bp.Height; j++)
                   {
                       piexl = _bp.GetPixel(i, j);
                       if (piexl.R <= dgGrayValue)
                       {
                           nearDots = 0;
                           if (sqareWidth == 3)
                           {
                               if (i == 0 || i == _bp.Width - 1 || j == 0 || j == _bp.Height - 1)
                               {
                                   _bp.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                                   continue;
                               }
                               if (_bp.GetPixel(i - 1, j - 1).R <= dgGrayValue) nearDots++;
                               if (_bp.GetPixel(i, j - 1).R <= dgGrayValue) nearDots++;
                               if (_bp.GetPixel(i + 1, j - 1).R <= dgGrayValue) nearDots++;
                               if (_bp.GetPixel(i - 1, j).R <= dgGrayValue) nearDots++;
                               if (_bp.GetPixel(i + 1, j).R <= dgGrayValue) nearDots++;
                               if (_bp.GetPixel(i - 1, j + 1).R <= dgGrayValue) nearDots++;
                               if (_bp.GetPixel(i, j + 1).R <= dgGrayValue) nearDots++;
                               if (_bp.GetPixel(i + 1, j + 1).R <= dgGrayValue) nearDots++;
                           }
                           else if (sqareWidth == 5)
                           {
                               if (i == 0 || i == _bp.Width - 1 || j == 0 || j == _bp.Height - 1 || i == 1 || i == _bp.Width - 2 || j == 1 || j == _bp.Height - 2)  //边框全去掉
                               {
                                   _bp.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                                   continue;
                               }
                               //内圈9格
                               if (_bp.GetPixel(i - 1, j - 1).R <= dgGrayValue) nearDots++;
                               if (_bp.GetPixel(i, j - 1).R <= dgGrayValue) nearDots++;
                               if (_bp.GetPixel(i + 1, j - 1).R <= dgGrayValue) nearDots++;
                               if (_bp.GetPixel(i - 1, j).R <= dgGrayValue) nearDots++;
                               if (_bp.GetPixel(i + 1, j).R <= dgGrayValue) nearDots++;
                               if (_bp.GetPixel(i - 1, j + 1).R <= dgGrayValue) nearDots++;
                               if (_bp.GetPixel(i, j + 1).R <= dgGrayValue) nearDots++;
                               if (_bp.GetPixel(i + 1, j + 1).R <= dgGrayValue) nearDots++;
                               //外圈16格
                               if (_bp.GetPixel(i - 2, j - 2).R <= dgGrayValue) nearDots++;
                               if (_bp.GetPixel(i - 2, j - 1).R <= dgGrayValue) nearDots++;
                               if (_bp.GetPixel(i - 2, j).R <= dgGrayValue) nearDots++;
                               if (_bp.GetPixel(i - 2, j + 1).R <= dgGrayValue) nearDots++;
                               if (_bp.GetPixel(i - 2, j + 2).R <= dgGrayValue) nearDots++;

                               if (_bp.GetPixel(i - 1, j - 2).R <= dgGrayValue) nearDots++;
                               if (_bp.GetPixel(i, j - 2).R <= dgGrayValue) nearDots++;
                               if (_bp.GetPixel(i + 1, j - 2).R <= dgGrayValue) nearDots++;

                               if (_bp.GetPixel(i - 1, j + 2).R <= dgGrayValue) nearDots++;
                               if (_bp.GetPixel(i, j + 2).R <= dgGrayValue) nearDots++;
                               if (_bp.GetPixel(i + 1, j + 2).R <= dgGrayValue) nearDots++;

                               if (_bp.GetPixel(i + 2, j - 2).R <= dgGrayValue) nearDots++;
                               if (_bp.GetPixel(i + 2, j - 1).R <= dgGrayValue) nearDots++;
                               if (_bp.GetPixel(i + 2, j).R <= dgGrayValue) nearDots++;
                               if (_bp.GetPixel(i + 2, j + 1).R <= dgGrayValue) nearDots++;
                               if (_bp.GetPixel(i + 2, j + 2).R <= dgGrayValue) nearDots++;
                           }

                           if (nearDots < MaxNearPoints)
                               _bp.SetPixel(i, j, Color.FromArgb(255, 255, 255));   //去掉单点 && 粗细小3邻边点
                           else if (nearDots > MaxNearPoints2)
                               _bp.SetPixel(i, j, Color.FromArgb(0, 0, 0));   //补充黑点
                       }
                       else  //背景
                           _bp.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                   }
               }
           }
           catch
           {
               return null;
           }
           return _bp.Clone() as Bitmap;
       }
       /// <summary>
       /// 滤波，去除图像中色素在某个颜色区间的元素
       /// 用于淡色背景图干扰
       /// </summary>
       /// <param name="firColor">第一区间颜色</param>
       /// <param name="secColor">第二区间颜色</param>
       /// <returns></returns>
       public Bitmap ClearBGNoise(Color firColor,Color secColor)
       {
           Color piexl;
           bool v1 = false;
           bool v2 = false;
           bool v3 = false;
           for (int i = 0; i < _bp.Width; i++)
           {
               for (int j = 0; j < _bp.Height; j++)
               {
                   if (i < 4 || i == _bp.Width - 1 || i == _bp.Width - 2 || i == _bp.Width - 3 || i == _bp.Width - 4 || j < 4 || j == _bp.Height - 1 || j == _bp.Height - 2 || j == _bp.Height - 3 || j == _bp.Height - 4)
                   {
                       _bp.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                       continue;
                   }
                   piexl = _bp.GetPixel(i, j);
                   if ((piexl.R - firColor.R >= 0) && (secColor.R - piexl.R >= 0)) v1 = true;
                   if ((piexl.G - firColor.G >= 0) && (secColor.G - piexl.G >= 0)) v2 = true;
                   if ((piexl.B - firColor.B >= 0) && (secColor.B - piexl.B >= 0)) v3 = true;
                   if (v1 && v2 && v3)
                       _bp.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                        v1 = false;
                        v2 = false;
                        v3 = false;
               }
           }
           return _bp.Clone() as Bitmap;
       }


       /// <summary>
       /// 去除干扰线
       /// </summary>
       public Bitmap ClearLine()
       {
           try
           {
               
           }
           catch
           {
               return null;
           }
           return _bp.Clone() as Bitmap;
       
       }
       #endregion

        #region 细化
       /// <summary>
       /// Hilditch细化算法 图形细化
       /// （I）：p 为1，即p不是背景；
        /// （2）：x1,x3,x5,x7不全部为1（否则把p标记删除，图像空心了）；
        /// （3）：x1~x8 中，至少有2个为1（若只有1个为1，则是线段的端点。若没有为1的，则为孤立点）；
        /// （4）：p的8连通联结数为1；
        /// 联结数指在像素p的3*3邻域中，和p连接的图形分量的个数：
       /// （5）假设x3已经标记删除，那么当x3为0时，p的8联通联结数为1；
       /// （6）假设x5已经标记删除，那么当x5为0时，p的8联通联结数为1。
       /// </summary>
       /// <param name="srcImg"></param>
       /// <returns></returns>
       public unsafe Bitmap ToThinner(Bitmap srcImg)
       {
           int[,] arr = GetArrayFormBitmap();
           for (int i = 0; i < _bp.Width; i++)
           {
               for (int j = 0; j < _bp.Height; j++)
               {
                   if (i == 0 || i == _bp.Width - 1 || j == 0 || j == _bp.Height - 1)
                       continue;
               }
           }
           return _bp.Clone() as Bitmap;
       }


       #endregion

        #region 改变图形大小
       /// <summary>
       /// 将图片放大或缩小到指定大小,会裁剪原图指定两边x,y距离后的矩形区域
       /// </summary>
       /// <param name="start">原图的有效区域x,y轴起点长度（距离起点边缘的长度）</param>
       /// <param name="end">裁剪的原图有效区域x,y的距离终点边缘的长度</param>
       /// <param name="newWidth">新宽度</param>
       /// <param name="newHeight">新高度</param>
       /// <returns></returns>
       public Bitmap ChangeBitMapSize(Point start, Point end, int newWidth, int newHeight)
       {
           using (Bitmap _new_bp = new Bitmap(newWidth, newHeight))
           {
               using (Graphics g = Graphics.FromImage(_new_bp))
               {
                   Point p = new Point(0, 0);
                   g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                   //设置高质量,低速度呈现平滑程度  
                   g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                   g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                   //消除锯齿
                   g.SmoothingMode = SmoothingMode.AntiAlias;
                   g.DrawImage(_bp, new Rectangle(0, 0, newWidth, newHeight), new Rectangle(start.X, start.Y, _bp.Width - end.X, _bp.Height - end.Y), GraphicsUnit.Pixel);
               }
               _bp = _new_bp.Clone() as Bitmap;
           }
           return _bp.Clone() as Bitmap;
       }
       #endregion

       /// <summary>
       /// 获取图像直方图
       /// </summary>
       public int[,] GetArrayFormBitmap()
       {
           int[,]arr=new int[_bp.Width,_bp.Height];
           for (int x = 0; x < _bp.Width; x++)
           {
               for (int y = 0; y < _bp.Height; y++)
               {
                   Color color = _bp.GetPixel(x, y);
                   if (color.R > 0)
                       arr[x, y] = 0;
                   else
                       arr[x, y] = 1;
               }
           }
           return arr;
       }
       #endregion
   }
}
