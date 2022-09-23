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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.VisualStyles;

namespace HYFrameWork.WinForm.Controls
{
	/// <summary>
	/// Summary description for TreeGridView.
	/// </summary>
    [DesignerCategory("code"),
    Designer(typeof(ControlDesigner)),
	ComplexBindingProperties,
    Docking(DockingBehavior.Ask)]
	public class TreeGridView:DataGridView
	{		
		private int _indentWidth;
		private TreeGridNode _root;
		private TreeGridColumn _expandableColumn;
		private bool _disposing;
		internal ImageList _imageList;
		private bool _inExpandCollapse;
        internal bool _inExpandCollapseMouseCapture;
		private System.Windows.Forms.Control hideScrollBarControl;
        private bool _showLines = true;
        private bool _virtualNodes;

        internal VisualStyleRenderer rOpen = new VisualStyleRenderer(VisualStyleElement.TreeView.Glyph.Opened);
		internal VisualStyleRenderer rClosed = new VisualStyleRenderer(VisualStyleElement.TreeView.Glyph.Closed);

        #region Constructor
        public TreeGridView()
		{
			// Control when edit occurs because edit mode shouldn't start when expanding/collapsing
			EditMode = DataGridViewEditMode.EditProgrammatically;
            RowTemplate = new TreeGridNode();
			// This sample does not support adding or deleting rows by the user.
			AllowUserToAddRows = false;
			AllowUserToDeleteRows = false;
			_root = new TreeGridNode(this);
			_root.IsRoot = true;

			// Ensures that all rows are added unshared by listening to the CollectionChanged event.
			base.Rows.CollectionChanged += delegate {};
        }
        #endregion

        #region Keyboard F2 to begin edit support
        protected override void OnKeyDown(KeyEventArgs e)
		{
			// Cause edit mode to begin since edit mode is disabled to support 
			// expanding/collapsing 
			base.OnKeyDown(e);
			if (!e.Handled)
			{
				if (e.KeyCode == Keys.F2 && CurrentCellAddress.X > -1 && CurrentCellAddress.Y >-1)
				{
					if (!CurrentCell.Displayed)
					{
						FirstDisplayedScrollingRowIndex = CurrentCellAddress.Y;
					}
				    SelectionMode = DataGridViewSelectionMode.CellSelect;
					BeginEdit(true);
				}
				else if (e.KeyCode == Keys.Enter && !IsCurrentCellInEditMode)
				{
					SelectionMode = DataGridViewSelectionMode.FullRowSelect;
					CurrentCell.OwningRow.Selected = true;
				}
			}
        }
        #endregion

        #region Shadow and hide DGV properties

        // This sample does not support databinding
		[Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		EditorBrowsable(EditorBrowsableState.Never)]
		public new object DataSource
		{
			get { return null; }
			set { throw new NotSupportedException("The TreeGridView does not support databinding"); }
		}

		[Browsable(false),
	    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		EditorBrowsable(EditorBrowsableState.Never)]
		public new object DataMember
		{
			get { return null; }
			set { throw new NotSupportedException("The TreeGridView does not support databinding"); }
		}

        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		EditorBrowsable(EditorBrowsableState.Never)]
        public new DataGridViewRowCollection Rows
        {
            get { return base.Rows; }
        }

        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        EditorBrowsable(EditorBrowsableState.Never)]
        public new bool VirtualMode
        {
            get { return false; }
            set { throw new NotSupportedException("The TreeGridView does not support virtual mode"); }
        }

        // none of the rows/nodes created use the row template, so it is hidden.
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        EditorBrowsable(EditorBrowsableState.Never)]
        public new DataGridViewRow RowTemplate
        {
            get { return base.RowTemplate; }
            set { base.RowTemplate = value; }
        }

        #endregion

        #region Public methods
        [Description("Returns the TreeGridNode for the given DataGridViewRow")]
        public TreeGridNode GetNodeForRow(DataGridViewRow row)
        {
            return row as TreeGridNode;
        }

        [Description("Returns the TreeGridNode for the given DataGridViewRow")]
        public TreeGridNode GetNodeForRow(int index)
        {
            return GetNodeForRow(base.Rows[index]);
        }
        #endregion

        #region Public properties
        [Category("Data"),
		Description("The collection of root nodes in the treelist."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
		public TreeGridNodeCollection Nodes
		{
			get
			{
				return _root.Nodes;
			}
		}

		public new TreeGridNode CurrentRow
		{
			get
			{
				return base.CurrentRow as TreeGridNode;
			}
		}

        [DefaultValue(false),
        Description("Causes nodes to always show as expandable. Use the NodeExpanding event to add nodes.")]
        public bool VirtualNodes
        {
            get { return _virtualNodes; }
            set { _virtualNodes = value; }
        }
	
		public TreeGridNode CurrentNode
		{
			get
			{
				return CurrentRow;
			}
		}

        [DefaultValue(true)]
        public bool ShowLines
        {
            get { return _showLines; }
            set { 
                if (value != _showLines) {
                    _showLines = value;
                    Invalidate();
                } 
            }
        }
	
		public ImageList ImageList
		{
			get { return _imageList; }
			set { 
				_imageList = value; 
				//TODO: should we invalidate cell styles when setting the image list?
			
			}
		}

        public new int RowCount
        {
            get { return Nodes.Count; }
            set
            {
                for (int i = 0; i < value; i++)
                    Nodes.Add(new TreeGridNode());

            }
        }
        #endregion

        #region Site nodes and collapse/expand support
        protected override void OnRowsAdded(DataGridViewRowsAddedEventArgs e)
        {
            base.OnRowsAdded(e);
            // Notify the row when it is added to the base grid 
            int count = e.RowCount - 1;
            TreeGridNode row;
            while (count >= 0)
            {
                row = base.Rows[e.RowIndex + count] as TreeGridNode;
                if (row != null)
                {
                    row.Sited();
                }
                count--;
            }
        }

		internal protected void UnSiteAll()
		{
			UnSiteNode(_root);
		}

		internal protected virtual void UnSiteNode(TreeGridNode node)
		{
            if (node.IsSited || node.IsRoot)
			{
				// remove child rows first
				foreach (TreeGridNode childNode in node.Nodes)
				{
					UnSiteNode(childNode);
				}

				// now remove this row except for the root
				if (!node.IsRoot)
				{
					base.Rows.Remove(node);
					// Row isn't sited in the grid anymore after remove. Note that we cannot
					// Use the RowRemoved event since we cannot map from the row index to
					// the index of the expandable row/node.
					node.UnSited();
				}
			}
		}

		internal protected virtual bool CollapseNode(TreeGridNode node)
		{
			if (node.IsExpanded)
			{
				CollapsingEventArgs exp = new CollapsingEventArgs(node);
				OnNodeCollapsing(exp);

				if (!exp.Cancel)
				{
					LockVerticalScrollBarUpdate(true);
                    SuspendLayout();
                    _inExpandCollapse = true;
                    node.IsExpanded = false;

					foreach (TreeGridNode childNode in node.Nodes)
					{
						Debug.Assert(childNode.RowIndex != -1, "Row is NOT in the grid.");
						UnSiteNode(childNode);
					}

					CollapsedEventArgs exped = new CollapsedEventArgs(node);
					OnNodeCollapsed(exped);
					//TODO: Convert this to a specific NodeCell property
                    _inExpandCollapse = false;
                    LockVerticalScrollBarUpdate(false);
                    ResumeLayout(true);
                    InvalidateCell(node.Cells[0]);

				}

				return !exp.Cancel;
			}
		    // row isn't expanded, so we didn't do anything.				
		    return false;
		}

		internal protected virtual void SiteNode(TreeGridNode node)
		{
			//TODO: Raise exception if parent node is not the root or is not sited.
			int rowIndex = -1;
			TreeGridNode currentRow;
			node._grid = this;

			if (node.Parent != null && node.Parent.IsRoot == false)
			{
				// row is a child
				Debug.Assert(node.Parent != null && node.Parent.IsExpanded);

				if (node.Index > 0)
				{
					currentRow = node.Parent.Nodes[node.Index - 1];
				}
				else
				{
					currentRow = node.Parent;
				}
			}
			else
			{
				// row is being added to the root
				if (node.Index > 0)
				{
					currentRow = node.Parent.Nodes[node.Index - 1];
				}
				else
				{
					currentRow = null;
				}

			}

			if (currentRow != null)
			{
				while (currentRow.Level >= node.Level)
				{
					if (currentRow.RowIndex < base.Rows.Count - 1)
					{
						currentRow = base.Rows[currentRow.RowIndex + 1] as TreeGridNode;
						Debug.Assert(currentRow != null);
					}
					else
						// no more rows, site this node at the end.
						break;

				}
				if (currentRow == node.Parent)
					rowIndex = currentRow.RowIndex + 1;
				else if (currentRow.Level < node.Level)
					rowIndex = currentRow.RowIndex;
				else
					rowIndex = currentRow.RowIndex + 1;
			}
			else
				rowIndex = 0;


			Debug.Assert(rowIndex != -1);
			SiteNode(node, rowIndex);

			Debug.Assert(node.IsSited);
			if (node.IsExpanded)
			{
				// add all child rows to display
				foreach (TreeGridNode childNode in node.Nodes)
				{
					//TODO: could use the more efficient SiteRow with index.
					SiteNode(childNode);
				}
			}
		}


		internal protected virtual void SiteNode(TreeGridNode node, int index)
		{
			if (index < base.Rows.Count)
			{
				base.Rows.Insert(index, node);
			}
			else
			{
				// for the last item.
				base.Rows.Add(node);
			}
		}

		internal protected virtual bool ExpandNode(TreeGridNode node)
		{
            if (!node.IsExpanded || _virtualNodes)
			{
				ExpandingEventArgs exp = new ExpandingEventArgs(node);
				OnNodeExpanding(exp);

				if (!exp.Cancel)
				{
					LockVerticalScrollBarUpdate(true);
                    SuspendLayout();
                    _inExpandCollapse = true;
                    node.IsExpanded = true;

					//TODO Convert this to a InsertRange
					foreach (TreeGridNode childNode in node.Nodes)
					{
						Debug.Assert(childNode.RowIndex == -1, "Row is already in the grid.");

						SiteNode(childNode);
						//this.BaseRows.Insert(rowIndex + 1, childRow);
						//TODO : remove -- just a test.
						//childNode.Cells[0].Value = "child";
					}

					ExpandedEventArgs exped = new ExpandedEventArgs(node);
					OnNodeExpanded(exped);
					//TODO: Convert this to a specific NodeCell property
                    _inExpandCollapse = false;
                    LockVerticalScrollBarUpdate(false);
                    ResumeLayout(true);
                    InvalidateCell(node.Cells[0]);
				}

				return !exp.Cancel;
			}
		    // row is already expanded, so we didn't do anything.
		    return false;
		}

        protected override void OnMouseUp(MouseEventArgs e)
        {
            // used to keep extra mouse moves from selecting more rows when collapsing
            base.OnMouseUp(e);
            _inExpandCollapseMouseCapture = false;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            // while we are expanding and collapsing a node mouse moves are
            // supressed to keep selections from being messed up.
            if (!_inExpandCollapseMouseCapture)
                base.OnMouseMove(e);

        }
        #endregion

        #region Collapse/Expand events
        public event ExpandingEventHandler NodeExpanding;
        public event ExpandedEventHandler NodeExpanded;
        public event CollapsingEventHandler NodeCollapsing;
        public event CollapsedEventHandler NodeCollapsed;

        protected virtual void OnNodeExpanding(ExpandingEventArgs e)
        {
            if (NodeExpanding != null)
            {
                NodeExpanding(this, e);
            }
        }
        protected virtual void OnNodeExpanded(ExpandedEventArgs e)
        {
            if (NodeExpanded != null)
            {
                NodeExpanded(this, e);
            }
        }
        protected virtual void OnNodeCollapsing(CollapsingEventArgs e)
        {
            if (NodeCollapsing != null)
            {
                NodeCollapsing(this, e);
            }

        }
        protected virtual void OnNodeCollapsed(CollapsedEventArgs e)
        {
            if (NodeCollapsed != null)
            {
                NodeCollapsed(this, e);
            }
        }
        #endregion

        #region Helper methods
        protected override void Dispose(bool disposing)
        {
            _disposing = true;
            base.Dispose(Disposing);
            UnSiteAll();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // this control is used to temporarly hide the vertical scroll bar
            hideScrollBarControl = new System.Windows.Forms.Control();
            hideScrollBarControl.Visible = false;
            hideScrollBarControl.Enabled = false;
            hideScrollBarControl.TabStop = false;
            // control is disposed automatically when the grid is disposed
            Controls.Add(hideScrollBarControl);
        }

        protected override void OnRowEnter(DataGridViewCellEventArgs e)
        {
            // ensure full row select
            base.OnRowEnter(e);
            if (SelectionMode == DataGridViewSelectionMode.CellSelect ||
                (SelectionMode == DataGridViewSelectionMode.FullRowSelect &&
                base.Rows[e.RowIndex].Selected == false))
            {
                SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                base.Rows[e.RowIndex].Selected = true;
            }
        }
        
		private void LockVerticalScrollBarUpdate(bool lockUpdate/*, bool delayed*/)
		{
            // Temporarly hide/show the vertical scroll bar by changing its parent
            if (!_inExpandCollapse)
            {
                if (lockUpdate)
                {
                    VerticalScrollBar.Parent = hideScrollBarControl;
                }
                else
                {
                    VerticalScrollBar.Parent = this;
                }
            }
        }

        protected override void OnColumnAdded(DataGridViewColumnEventArgs e)
        {
            if (typeof(TreeGridColumn).IsAssignableFrom(e.Column.GetType()))
            {
                if (_expandableColumn == null)
                {
                    // identify the expanding column.			
                    _expandableColumn = (TreeGridColumn)e.Column;
                }
            }

            // Expandable Grid doesn't support sorting. This is just a limitation of the sample.
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;

            base.OnColumnAdded(e);
        }

        private static class Win32Helper
        {
            public const int WM_SYSKEYDOWN = 0x0104,
                             WM_KEYDOWN = 0x0100,
                             WM_SETREDRAW = 0x000B;

            [DllImport("USER32.DLL", CharSet = CharSet.Auto)]
            public static extern IntPtr SendMessage(HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam);

            [DllImport("USER32.DLL", CharSet = CharSet.Auto)]
            public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, int lParam);

            [DllImport("USER32.DLL", CharSet = CharSet.Auto)]
            public static extern bool PostMessage(HandleRef hwnd, int msg, IntPtr wparam, IntPtr lparam);

        }
        #endregion

        private void InitializeComponent()
        {
            ((ISupportInitialize)(this)).BeginInit();
            SuspendLayout();
            // 
            // TreeGridView
            // 
            RowTemplate.Height = 23;
            ((ISupportInitialize)(this)).EndInit();
            ResumeLayout(false);

        }


    }
}
