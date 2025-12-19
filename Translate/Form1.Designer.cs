namespace Translate
{
    partial class Form1
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
            this.lblSource = new System.Windows.Forms.Label();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.lblSourceLang = new System.Windows.Forms.Label();
            this.cmbSource = new System.Windows.Forms.ComboBox();
            this.lblTarget = new System.Windows.Forms.Label();
            this.cmbTarget = new System.Windows.Forms.ComboBox();
            this.btnTranslate = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(12, 15);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(74, 13);
            this.lblSource.TabIndex = 0;
            this.lblSource.Text = "Kaynak metin:";
            // 
            // txtSource
            // 
            this.txtSource.Location = new System.Drawing.Point(15, 35);
            this.txtSource.Multiline = true;
            this.txtSource.Name = "txtSource";
            this.txtSource.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSource.Size = new System.Drawing.Size(570, 180);
            this.txtSource.TabIndex = 1;
            // 
            // lblSourceLang
            // 
            this.lblSourceLang.AutoSize = true;
            this.lblSourceLang.Location = new System.Drawing.Point(12, 230);
            this.lblSourceLang.Name = "lblSourceLang";
            this.lblSourceLang.Size = new System.Drawing.Size(59, 13);
            this.lblSourceLang.TabIndex = 2;
            this.lblSourceLang.Text = "Kaynak dil:";
            // 
            // cmbSource
            // 
            this.cmbSource.FormattingEnabled = true;
            this.cmbSource.Items.AddRange(new object[] {
            "tr - Türkçe",
            "en - English",
            "es - Español",
            "fr - Français",
            "de - Deutsch",
            "ru - Русский",
            "zh-CN - 中文 (简体)",
            "ja - 日本語",
            "ko - 한국어",
            "ar - العربية",
            "pt - Português"});
            this.cmbSource.Location = new System.Drawing.Point(90, 227);
            this.cmbSource.Name = "cmbSource";
            this.cmbSource.Size = new System.Drawing.Size(200, 21);
            this.cmbSource.TabIndex = 3;
            this.cmbSource.Text = "tr - Türkçe";
            this.cmbSource.SelectedIndexChanged += new System.EventHandler(this.cmbSource_SelectedIndexChanged);
            // 
            // lblTarget
            // 
            this.lblTarget.AutoSize = true;
            this.lblTarget.Location = new System.Drawing.Point(12, 281);
            this.lblTarget.Name = "lblTarget";
            this.lblTarget.Size = new System.Drawing.Size(52, 13);
            this.lblTarget.TabIndex = 4;
            this.lblTarget.Text = "Hedef dil:";
            this.lblTarget.Click += new System.EventHandler(this.lblTarget_Click);
            // 
            // cmbTarget
            // 
            this.cmbTarget.FormattingEnabled = true;
            this.cmbTarget.Items.AddRange(new object[] {
            "en - English",
            "tr - Türkçe",
            "es - Español",
            "fr - Français",
            "de - Deutsch",
            "ru - Русский",
            "zh-CN - 中文 (简体)",
            "ja - 日本語",
            "ko - 한국어",
            "ar - العربية",
            "pt - Português"});
            this.cmbTarget.Location = new System.Drawing.Point(90, 278);
            this.cmbTarget.Name = "cmbTarget";
            this.cmbTarget.Size = new System.Drawing.Size(200, 21);
            this.cmbTarget.TabIndex = 5;
            // 
            // btnTranslate
            // 
            this.btnTranslate.Location = new System.Drawing.Point(378, 251);
            this.btnTranslate.Name = "btnTranslate";
            this.btnTranslate.Size = new System.Drawing.Size(100, 30);
            this.btnTranslate.TabIndex = 8;
            this.btnTranslate.Text = "Çevir";
            this.btnTranslate.UseVisualStyleBackColor = true;
            this.btnTranslate.Click += new System.EventHandler(this.btnTranslate_Click);
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(12, 345);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(41, 13);
            this.lblResult.TabIndex = 9;
            this.lblResult.Text = "Sonuç:";
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(15, 365);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(570, 220);
            this.txtResult.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 600);
            this.Controls.Add(this.lblSource);
            this.Controls.Add(this.txtSource);
            this.Controls.Add(this.lblSourceLang);
            this.Controls.Add(this.cmbSource);
            this.Controls.Add(this.lblTarget);
            this.Controls.Add(this.cmbTarget);
            this.Controls.Add(this.btnTranslate);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.txtResult);
            this.Name = "Form1";
            this.Text = "Çoklu Dil Destekli Çeviri Uygulaması";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.Label lblSourceLang;
        private System.Windows.Forms.ComboBox cmbSource;
        private System.Windows.Forms.Label lblTarget;
        private System.Windows.Forms.ComboBox cmbTarget;
        private System.Windows.Forms.Button btnTranslate;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.TextBox txtResult;
    }
}

