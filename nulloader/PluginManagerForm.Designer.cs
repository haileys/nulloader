namespace nulloader
{
    partial class PluginManagerForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.runningList = new System.Windows.Forms.ListView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.disabledList = new System.Windows.Forms.ListView();
            this.restartbutton = new System.Windows.Forms.Button();
            this.restartalert = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.runningList);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(247, 260);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Running Plugins ";
            // 
            // button1
            // 
            this.button1.Image = global::nulloader.Properties.Resources.delete;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(169, 231);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(72, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Disable";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // runningList
            // 
            this.runningList.Location = new System.Drawing.Point(6, 19);
            this.runningList.Name = "runningList";
            this.runningList.Size = new System.Drawing.Size(235, 206);
            this.runningList.TabIndex = 0;
            this.runningList.UseCompatibleStateImageBehavior = false;
            this.runningList.View = System.Windows.Forms.View.List;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.disabledList);
            this.groupBox2.Location = new System.Drawing.Point(265, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(247, 260);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " Disabled Plugins ";
            // 
            // button3
            // 
            this.button3.Image = global::nulloader.Properties.Resources.control_play_blue;
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point(59, 231);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(112, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Temporarily Start";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Image = global::nulloader.Properties.Resources.accept;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(177, 231);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(64, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Enable";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // disabledList
            // 
            this.disabledList.Location = new System.Drawing.Point(6, 19);
            this.disabledList.Name = "disabledList";
            this.disabledList.Size = new System.Drawing.Size(235, 206);
            this.disabledList.TabIndex = 0;
            this.disabledList.UseCompatibleStateImageBehavior = false;
            this.disabledList.View = System.Windows.Forms.View.List;
            // 
            // restartbutton
            // 
            this.restartbutton.Location = new System.Drawing.Point(427, 282);
            this.restartbutton.Name = "restartbutton";
            this.restartbutton.Size = new System.Drawing.Size(85, 23);
            this.restartbutton.TabIndex = 2;
            this.restartbutton.Text = "Restart";
            this.restartbutton.UseVisualStyleBackColor = true;
            this.restartbutton.Visible = false;
            this.restartbutton.Click += new System.EventHandler(this.button4_Click);
            // 
            // restartalert
            // 
            this.restartalert.Image = global::nulloader.Properties.Resources.error;
            this.restartalert.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.restartalert.Location = new System.Drawing.Point(9, 282);
            this.restartalert.Name = "restartalert";
            this.restartalert.Size = new System.Drawing.Size(377, 23);
            this.restartalert.TabIndex = 3;
            this.restartalert.Text = "You need to restart Nullular Grapher before your changes will be applied.";
            this.restartalert.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.restartalert.Visible = false;
            // 
            // PluginManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 313);
            this.Controls.Add(this.restartalert);
            this.Controls.Add(this.restartbutton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PluginManagerForm";
            this.Text = "Plugin Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PluginManagerForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView runningList;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListView disabledList;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button restartbutton;
        private System.Windows.Forms.Label restartalert;
    }
}