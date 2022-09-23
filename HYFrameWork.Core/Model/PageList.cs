using System.Collections.Generic;
using System.Linq;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 实体：分页
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    public class PageList<T>
    {
        /// <summary>
        /// 分页构造
        /// </summary>
        /// <param name="total">数据总量</param>
        /// <param name="pageSize">页数据量</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="items">页数据集合</param>
        public PageList(int total, int pageSize, int pageIndex, IEnumerable<T> items)
        {
            _total = total;
            _pageSize = pageSize;
            _pageIndex = pageIndex;
            _items = items;
            _totalPage = _total % _pageSize == 0 ? _total / _pageSize : _total / _pageSize + 1;
        }

        /// <summary>
        /// 分页构造
        /// </summary>
        /// <param name="data">数据集合</param>
        /// <param name="pageSize">页数</param>
        /// <param name="pageIndex">页码</param>
        public PageList(IEnumerable<T>data,int pageSize,int pageIndex)
        {
            if (data != null)
            {
                _total = data.Count();
                _pageSize = pageSize;
                _pageIndex = pageIndex;
                _items = data.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                _totalPage = _total % _pageSize == 0 ? _total / _pageSize : _total / _pageSize + 1;
            }
        }
        private int _total;
        /// <summary>
        /// 数据总量
        /// </summary>
        public int Total
        {
            get { return _total; }
        }

        private IEnumerable<T> _items;

        /// <summary>
        /// 分页数据集合
        /// </summary>
        public IEnumerable<T> Items
        {
            get { return _items; }
        }

        private int _pageSize;
        /// <summary>
        /// 页数据量
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
        }

        private int _pageIndex;
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex
        {
            get { return _pageIndex; }
        }

        private int _totalPage;

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage
        {
            get { return _totalPage; }
        }
        /// <summary>
        /// 是否有前一页
        /// </summary>
        public bool HasPrev
        {
            get { return _pageIndex > 1; }
        }
        /// <summary>
        /// 是否有后一页
        /// </summary>
        public bool HasNext
        {
            get { return _pageIndex < _totalPage; }
        }
    }

}
