namespace Dame
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pic_Spielfeld = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Spielfeld)).BeginInit();
            this.SuspendLayout();
            // 
            // pic_Spielfeld
            // 
            this.pic_Spielfeld.BackColor = System.Drawing.Color.Transparent;
            this.pic_Spielfeld.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pic_Spielfeld.BackgroundImage")));
            this.pic_Spielfeld.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pic_Spielfeld.Location = new System.Drawing.Point(200, 30);
            this.pic_Spielfeld.Name = "pic_Spielfeld";
            this.pic_Spielfeld.Size = new System.Drawing.Size(500, 500);
            this.pic_Spielfeld.TabIndex = 0;
            this.pic_Spielfeld.TabStop = false;
            this.pic_Spielfeld.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.pic_Spielfeld.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pic_Spielfeld_MouseClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.pic_Spielfeld);
            this.Name = "Form1";
            this.Text = "Form1";
            this.LocationChanged += new System.EventHandler(this.Form1_LocationChanged);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pic_Spielfeld)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pic_Spielfeld;
    }
}

