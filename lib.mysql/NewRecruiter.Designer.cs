namespace fp.lib.mysql
{
    partial class NewRecruiter
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
            this.recruiters = new System.Windows.Forms.ListBox();
            this.jobsUrl = new System.Windows.Forms.CheckBox();
            this.filter = new System.Windows.Forms.TextBox();
            this.parseDef = new System.Windows.Forms.CheckBox();
            this.includeActive = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // recruiters
            // 
            this.recruiters.FormattingEnabled = true;
            this.recruiters.ItemHeight = 31;
            this.recruiters.Location = new System.Drawing.Point(33, 114);
            this.recruiters.Name = "recruiters";
            this.recruiters.Size = new System.Drawing.Size(597, 500);
            this.recruiters.TabIndex = 0;
            // 
            // jobsUrl
            // 
            this.jobsUrl.AutoSize = true;
            this.jobsUrl.Location = new System.Drawing.Point(649, 536);
            this.jobsUrl.Name = "jobsUrl";
            this.jobsUrl.Size = new System.Drawing.Size(156, 36);
            this.jobsUrl.TabIndex = 1;
            this.jobsUrl.Text = "Jobs Url";
            this.jobsUrl.UseVisualStyleBackColor = true;
            // 
            // filter
            // 
            this.filter.Location = new System.Drawing.Point(33, 70);
            this.filter.Name = "filter";
            this.filter.Size = new System.Drawing.Size(597, 38);
            this.filter.TabIndex = 2;
            // 
            // parseDef
            // 
            this.parseDef.AutoSize = true;
            this.parseDef.Location = new System.Drawing.Point(649, 578);
            this.parseDef.Name = "parseDef";
            this.parseDef.Size = new System.Drawing.Size(178, 36);
            this.parseDef.TabIndex = 3;
            this.parseDef.Text = "Parse Def";
            this.parseDef.UseVisualStyleBackColor = true;
            // 
            // includeActive
            // 
            this.includeActive.AutoSize = true;
            this.includeActive.Location = new System.Drawing.Point(649, 494);
            this.includeActive.Name = "includeActive";
            this.includeActive.Size = new System.Drawing.Size(131, 36);
            this.includeActive.TabIndex = 4;
            this.includeActive.Text = "Active";
            this.includeActive.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(649, 114);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(140, 73);
            this.button1.TabIndex = 5;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(649, 205);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(140, 73);
            this.button2.TabIndex = 6;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // NewRecruiter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 648);
            this.ControlBox = false;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.includeActive);
            this.Controls.Add(this.parseDef);
            this.Controls.Add(this.filter);
            this.Controls.Add(this.jobsUrl);
            this.Controls.Add(this.recruiters);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "NewRecruiter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "NewRecruiter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox recruiters;
        private System.Windows.Forms.CheckBox jobsUrl;
        private System.Windows.Forms.TextBox filter;
        private System.Windows.Forms.CheckBox parseDef;
        private System.Windows.Forms.CheckBox includeActive;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}