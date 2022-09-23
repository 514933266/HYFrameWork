using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HYFrameWork.Core;
using System.Data;

namespace HYFrameWork.WinForm
{
    public static class DatagridviewExtension
    {
        public static void InvokeSelectRow(this DataGridView dgv, int rowIndex)
        {
            dgv.Invoke(new Action(() =>
            {
                dgv.Rows[rowIndex].Selected = true;
            }));
        }

        public static void InvokeCurrentCell(this DataGridView dgv, int rowIndex, int cellIndex)
        {
            dgv.Invoke(new Action(() =>
            {
                dgv.CurrentCell = dgv.Rows[rowIndex].Cells[cellIndex];
            }));
        }

        public static void InvokeDataSource(this DataGridView dgv, object list)
        {
            dgv.Invoke(new Action(() =>
            {
                dgv.DataSource = list;
            }));
        }
        public static void InvokeTag(this DataGridView dgv, object list)
        {
            dgv.Invoke(new Action(() =>
            {
                dgv.Tag = list;
            }));
        }

        public static void InvokeEdit(this DataGridView dgv, int rowIndex, int cellIndex)
        {
            dgv.Invoke(new Action(() =>
            {
                dgv.Rows[rowIndex].Cells[cellIndex].ReadOnly = false;//将当前单元格设为可读
                    dgv.CurrentCell = dgv.Rows[rowIndex].Cells[cellIndex];//获取当前单元格
                    dgv.BeginEdit(true);//将单元格设为编辑状态
            }));
        }

        /// <summary>
        /// 添加新行
        /// </summary>
        public static void InvokeAdd(this DataGridView dgv,params object[] rows)
        {
            dgv.Invoke(new Action(() =>
            {
                dgv.Rows.Add(rows);
            }));
        }
        /// <summary>
        /// 添加新行
        /// </summary>
        public static void InvokeAdd<T>(this DataGridView dgv, List<T> rows)
        {

            dgv.Invoke(new Action(() =>
            {
                foreach (T t in rows)
                {
                    dgv.Rows.Add(ReflectionHelper.GetPropertysValue(t));
                }

            }));
        }
        /// <summary>
        /// 清理表格后再进行数据逐行添加
        /// </summary>
        public static void InvokeClearAdd<T>(this DataGridView dgv, List<T> rows)
        {
            dgv.Invoke(new Action(() =>
            {
                dgv.Rows.Clear();
                foreach (T t in rows)
                {
                    dgv.Rows.Add(ReflectionHelper.GetPropertysValue(t));
                }
            }));
        }
        /// <summary>
        /// 设置数据源
        /// </summary>
        public static void InvokeSource<T>(this DataGridView dgv, List<T> rows)
        {
            dgv.Invoke(new Action(() =>
            {
                List<T> newRows = new List<T>();
                newRows.AddRange(rows);
                dgv.DataSource = newRows;
            }));
        }
        /// <summary>
        /// 修改单元格
        /// </summary>
        public static void InvokeUpdate(this DataGridView dgv, int rowIndex, int cellIndex, object value)
        {
            dgv.Invoke(new Action(() =>
            {
                if (rowIndex >= 0)dgv.Rows[rowIndex].Cells[cellIndex].Value = value;
            }));
        }
        /// <summary>
        /// 清空数据
        /// </summary>
        public static void InvokeClear(this DataGridView dgv)
        {
            dgv.Invoke(new Action(() =>
            {
                try
                {
                    dgv.Rows.Clear();
                }
                catch
                {
                    //出错时转为赋值清空
                    dgv.InvokeDataSource(null);
                }
            }));
        }
        /// <summary>
        /// 转换为DataTable
        /// </summary>
        public static DataTable ToDataTable(this DataGridView dgv)
        {
            return ToDataTable(dgv,null);
        }

        /// <summary>
        /// 遍历DataGridView的行数据
        /// </summary>
        public static void Foreach(this DataGridView dgv, Action<DataGridViewRow> action)
        {
            foreach (DataGridViewRow r in dgv.Rows)
            {
                action(r);
            }
        }

        /// <summary>
        /// 遍历DataGridView的行数据，方法执行成功后跳出
        /// </summary>
        public static void Foreach(this DataGridView dgv, Func<DataGridViewRow,bool> action)
        {
            foreach (DataGridViewRow r in dgv.Rows)
            {
                if (action(r)) break;
            }
        }
        /// <summary>
        /// 将DatagridView转换为DataTable 格式
        /// </summary>
        /// <param name="dgv">Datagridview 对象</param>
        /// <param name="noWriteColumsIndex">集合中对应的Index的列 将不会被添加到表格中</param>
        /// <param name="isHeaderText">时候启用标题文本</param>
        /// <returns></returns>
        public static DataTable ToDataTable(this DataGridView dgv, int[]noWriteColumsIndex,bool isHeaderText=true)
        {
            DataTable dt = new DataTable();
            List<int> writeColums = new List<int>();
            // 列强制转换  
            for (int count = 0; count < dgv.Columns.Count; count++)
            {
                if (!noWriteColumsIndex.CheckIsIn(count))
                {
                    writeColums.Add(count);
                    DataColumn dc = new DataColumn(isHeaderText? dgv.Columns[count].HeaderText:dgv.Columns[count].Name.ToString());
                    dt.Columns.Add(dc);
                }
            }
            // 循环行
            for (int r = 0; r < dgv.Rows.Count; r++)
            {
                DataRow dr = dt.NewRow();
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    var v = dgv.Rows[r].Cells[writeColums[c]].Value;
                    dr[c] = (v==null?"":v.ToString());
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
