using System;
using System.Windows.Forms;

namespace HYFrameWork.WinForm
{
  public static  class PictureBoxExtension
    {
      public static void InvokeVisible(this PictureBox pic, bool visible)
      {
          if (pic.InvokeRequired)
          {
              pic.Invoke(new Action(() =>
              {
                  pic.Visible = visible;
              }));
          }
          else
          {
              pic.Visible = visible;
          }
      }
    }
}
