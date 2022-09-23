using HYFrameWork.File;
using System.Windows.Forms;

namespace HYFrameWork.WinForm.Controls
{

    public class DiaLogs
    {
       /// <summary>
        /// 显示一个保存文件的对话框,并返回保存路径
       /// </summary>
       /// <param name="filter">自定义查找文件类型</param>
       /// <param name="isShowAll">是否显示所有文件选项</param>
        public static string ShowSaveFileDialog(string filter, bool isShowAll)
       {
           string fileName = string.Empty;
           if (isShowAll) filter += "|所有文件（*.*）|*.*";
           SaveFileDialog dialog = new SaveFileDialog(){Filter=filter,FilterIndex = 1,RestoreDirectory = true};
           if (dialog.ShowDialog() == DialogResult.OK)
           {
               fileName = dialog.FileName;
               FileHelper.CreateParentDirectory(fileName);
           }
           return fileName;
       }
       /// <summary>
       /// 打开一个文件，获得绝对路径
       /// </summary>
       /// <param name="Filter">显示的文件类型（过滤条件）如：文本文件|*.*|C#文件|*.cs|所有文件|*.*</param>
       /// <returns></returns>
       public static string ShowOpenFileDialog(string Filter)
       {
           string path = string.Empty;
           OpenFileDialog openFileDialog = new OpenFileDialog()
           {
               Filter = Filter,
               FilterIndex = 1
           };
           if (openFileDialog.ShowDialog() == DialogResult.OK)
               path = openFileDialog.FileName;
           return path;
       }
       /// <summary>
       /// 选择一个路径
       /// </summary>
       public static string ShowFolderBrowserDialog()
       {
           string path = string.Empty;
           FolderBrowserDialog selectDicDialog = new FolderBrowserDialog();
           if (selectDicDialog.ShowDialog() == DialogResult.OK)
               path = selectDicDialog.SelectedPath;
           return path;
       }
    }
}
