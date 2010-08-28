namespace Inspector
{
    partial class InspectorForm
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
            this.components = new System.ComponentModel.Container();
            this.formcontrols = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.controlprops = new System.Windows.Forms.PropertyGrid();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // formcontrols
            // 
            this.formcontrols.ContextMenuStrip = this.contextMenuStrip1;
            this.formcontrols.Dock = System.Windows.Forms.DockStyle.Left;
            this.formcontrols.Location = new System.Drawing.Point(0, 0);
            this.formcontrols.Name = "formcontrols";
            this.formcontrols.Size = new System.Drawing.Size(388, 382);
            this.formcontrols.TabIndex = 1;
            this.formcontrols.Click += new System.EventHandler(this.formcontrols_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 48);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem1.Text = "Refresh";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(388, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 382);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // controlprops
            // 
            this.controlprops.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlprops.Location = new System.Drawing.Point(391, 0);
            this.controlprops.Name = "controlprops";
            this.controlprops.Size = new System.Drawing.Size(442, 382);
            this.controlprops.TabIndex = 3;
            // 
            // InspectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 382);
            this.Controls.Add(this.controlprops);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.formcontrols);
            this.Name = "InspectorForm";
            this.Text = "Inspector";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView formcontrols;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.PropertyGrid controlprops;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}