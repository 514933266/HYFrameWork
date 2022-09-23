//---------------------------------------------------------------------
// 
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// 
//THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
//KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//PARTICULAR PURPOSE.
//---------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;

namespace HYFrameWork.WinForm.Controls
{
	public class TreeGridNodeCollection : IList<TreeGridNode>, IList
	{
		internal List<TreeGridNode> _list;
		internal TreeGridNode _owner;
		internal TreeGridNodeCollection(TreeGridNode owner)
		{
			_owner = owner;
			_list = new List<TreeGridNode>();
		}

		#region Public Members
		public void Add(TreeGridNode item)
		{
			// The row needs to exist in the child collection before the parent is notified.
			item._grid = _owner._grid;

            bool hadChildren = _owner.HasChildren;
			item._owner = this;

			_list.Add(item);

			_owner.AddChildNode(item);

            // if the owner didn't have children but now does (asserted) and it is sited update it
            if (!hadChildren && _owner.IsSited)
            {
                _owner._grid.InvalidateRow(_owner.RowIndex);
            }
		}

        public TreeGridNode Add(string text)
        {
            TreeGridNode node = new TreeGridNode();
            Add(node);

            node.Cells[0].Value = text;
            return node;
        }

        public TreeGridNode Add(params object[] values)
        {
            TreeGridNode node = new TreeGridNode();
            Add(node);

            int cell = 0;

            if (values.Length > node.Cells.Count )
                throw new ArgumentOutOfRangeException("values");

            foreach (object o in values)
            {
                node.Cells[cell].Value = o;
                cell++;
            }
            return node;
        }

        public void Insert(int index, TreeGridNode item)
        {
            // The row needs to exist in the child collection before the parent is notified.
            item._grid = _owner._grid;
            item._owner = this;

            _list.Insert(index, item);

            _owner.InsertChildNode(index, item);
        }

        public bool Remove(TreeGridNode item)
		{
			// The parent is notified first then the row is removed from the child collection.
			_owner.RemoveChildNode(item);
			item._grid = null;
			return _list.Remove(item);
		}

        public void RemoveAt(int index)
		{
			TreeGridNode row = _list[index];

			// The parent is notified first then the row is removed from the child collection.
			_owner.RemoveChildNode(row);
			row._grid = null;
			_list.RemoveAt(index);
		}

        public void Clear()
		{
			// The parent is notified first then the row is removed from the child collection.
			_owner.ClearNodes();
			_list.Clear();
		}

        public int IndexOf(TreeGridNode item)
        {
            return _list.IndexOf(item);
        }

		public TreeGridNode this[int index]
		{
			get
			{
				return _list[index];
			}
			set
			{
				throw new Exception("The method or operation is not implemented.");
			}
		}

		public bool Contains(TreeGridNode item)
		{
			return _list.Contains(item);
		}

		public void CopyTo(TreeGridNode[] array, int arrayIndex)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int Count
		{
			get{ return _list.Count; }
		}

        public bool IsReadOnly
		{
			get{ return false; }
		}
        #endregion

        #region IList Interface
        void IList.Remove(object value)
        {
            Remove(value as TreeGridNode);
        }


        int IList.Add(object value)
        {
            TreeGridNode item = value as TreeGridNode;
            Add(item);
            return item.Index;
        }

        void IList.RemoveAt(int index)
        {
            RemoveAt(index);
        }


        void IList.Clear()
        {
            Clear();
        }

        bool IList.IsReadOnly
		{
			get { return IsReadOnly;}
		}

		bool IList.IsFixedSize
		{
			get { return false; }
		}

        int IList.IndexOf(object item)
        {
            return IndexOf(item as TreeGridNode);
        }

        void IList.Insert(int index, object value)
        {
            Insert(index, value as TreeGridNode);
        }
        int ICollection.Count
        {
            get { return Count; }
        }
        bool IList.Contains(object value)
        {
            return Contains(value as TreeGridNode);
        }
        void ICollection.CopyTo(Array array, int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }



		#region IEnumerable<ExpandableRow> Members

		public IEnumerator<TreeGridNode> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		#endregion


		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
		#endregion

		#region ICollection Members

		bool ICollection.IsSynchronized
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		object ICollection.SyncRoot
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		#endregion
	}

}
