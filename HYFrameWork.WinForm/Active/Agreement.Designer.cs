namespace HYFrameWork.WinForm.Active
{
    partial class Agreement
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Agreement));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btndisagree = new System.Windows.Forms.Button();
            this.btnagree = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.btndisagree);
            this.groupBox1.Controls.Add(this.btnagree);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 290);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(474, 96);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btndisagree
            // 
            this.btndisagree.ForeColor = System.Drawing.Color.Black;
            this.btndisagree.Location = new System.Drawing.Point(283, 20);
            this.btndisagree.Name = "btndisagree";
            this.btndisagree.Size = new System.Drawing.Size(179, 64);
            this.btndisagree.TabIndex = 1;
            this.btndisagree.Text = "不同意此协议";
            this.btndisagree.UseVisualStyleBackColor = true;
            this.btndisagree.Click += new System.EventHandler(this.btndisagree_Click);
            // 
            // btnagree
            // 
            this.btnagree.ForeColor = System.Drawing.Color.Black;
            this.btnagree.Location = new System.Drawing.Point(12, 20);
            this.btnagree.Name = "btnagree";
            this.btnagree.Size = new System.Drawing.Size(179, 64);
            this.btnagree.TabIndex = 0;
            this.btnagree.Text = "我已经阅读同意";
            this.btnagree.UseVisualStyleBackColor = true;
            this.btnagree.Click += new System.EventHandler(this.btnagree_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(474, 290);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // Agreement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Yellow;
            this.ClientSize = new System.Drawing.Size(474, 386);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Agreement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "使用协议";
            this.Load += new System.EventHandler(this.Agreement_Load);
            this.Shown += new System.EventHandler(this.Agreement_Shown);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btndisagree;
        private System.Windows.Forms.Button btnagree;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}