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
            this.btn_newRound = new System.Windows.Forms.Button();
            this.lbl_spieler1 = new System.Windows.Forms.Label();
            this.lbl_spieler2 = new System.Windows.Forms.Label();
            this.pnl_whichPlayer = new System.Windows.Forms.Panel();
            this.lbl_whichPlayer = new System.Windows.Forms.Label();
            this.txt_console = new System.Windows.Forms.TextBox();
            this.lbl_console = new System.Windows.Forms.Label();
            this.lbl_schlagzwang = new System.Windows.Forms.Label();
            this.rdb_schlagzwangEin = new System.Windows.Forms.RadioButton();
            this.rdb_schlagzwangAus = new System.Windows.Forms.RadioButton();
            this.pnl_schlagzwang = new System.Windows.Forms.Panel();
            this.txt_schlagzwang = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Spielfeld)).BeginInit();
            this.pnl_whichPlayer.SuspendLayout();
            this.pnl_schlagzwang.SuspendLayout();
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
            // btn_newRound
            // 
            this.btn_newRound.Location = new System.Drawing.Point(33, 31);
            this.btn_newRound.Name = "btn_newRound";
            this.btn_newRound.Size = new System.Drawing.Size(125, 53);
            this.btn_newRound.TabIndex = 2;
            this.btn_newRound.Text = "Neues Spiel starten";
            this.btn_newRound.UseVisualStyleBackColor = true;
            this.btn_newRound.Click += new System.EventHandler(this.btn_newRound_Click);
            // 
            // lbl_spieler1
            // 
            this.lbl_spieler1.AutoSize = true;
            this.lbl_spieler1.BackColor = System.Drawing.Color.White;
            this.lbl_spieler1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_spieler1.Location = new System.Drawing.Point(41, 36);
            this.lbl_spieler1.Name = "lbl_spieler1";
            this.lbl_spieler1.Size = new System.Drawing.Size(71, 20);
            this.lbl_spieler1.TabIndex = 3;
            this.lbl_spieler1.Text = "Spieler 1";
            // 
            // lbl_spieler2
            // 
            this.lbl_spieler2.AutoSize = true;
            this.lbl_spieler2.BackColor = System.Drawing.Color.Red;
            this.lbl_spieler2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_spieler2.Location = new System.Drawing.Point(41, 64);
            this.lbl_spieler2.Name = "lbl_spieler2";
            this.lbl_spieler2.Size = new System.Drawing.Size(71, 20);
            this.lbl_spieler2.TabIndex = 4;
            this.lbl_spieler2.Text = "Spieler 2";
            // 
            // pnl_whichPlayer
            // 
            this.pnl_whichPlayer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_whichPlayer.Controls.Add(this.lbl_whichPlayer);
            this.pnl_whichPlayer.Controls.Add(this.lbl_spieler1);
            this.pnl_whichPlayer.Controls.Add(this.lbl_spieler2);
            this.pnl_whichPlayer.Location = new System.Drawing.Point(33, 116);
            this.pnl_whichPlayer.Name = "pnl_whichPlayer";
            this.pnl_whichPlayer.Size = new System.Drawing.Size(125, 99);
            this.pnl_whichPlayer.TabIndex = 5;
            // 
            // lbl_whichPlayer
            // 
            this.lbl_whichPlayer.AutoSize = true;
            this.lbl_whichPlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_whichPlayer.Location = new System.Drawing.Point(4, 4);
            this.lbl_whichPlayer.Name = "lbl_whichPlayer";
            this.lbl_whichPlayer.Size = new System.Drawing.Size(116, 20);
            this.lbl_whichPlayer.TabIndex = 5;
            this.lbl_whichPlayer.Text = "Wer ist dran?";
            // 
            // txt_console
            // 
            this.txt_console.Location = new System.Drawing.Point(33, 388);
            this.txt_console.Multiline = true;
            this.txt_console.Name = "txt_console";
            this.txt_console.Size = new System.Drawing.Size(125, 69);
            this.txt_console.TabIndex = 6;
            // 
            // lbl_console
            // 
            this.lbl_console.AutoSize = true;
            this.lbl_console.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_console.Location = new System.Drawing.Point(31, 366);
            this.lbl_console.Name = "lbl_console";
            this.lbl_console.Size = new System.Drawing.Size(126, 20);
            this.lbl_console.TabIndex = 7;
            this.lbl_console.Text = "Ereignisanzeige:";
            // 
            // lbl_schlagzwang
            // 
            this.lbl_schlagzwang.AutoSize = true;
            this.lbl_schlagzwang.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_schlagzwang.Location = new System.Drawing.Point(4, 5);
            this.lbl_schlagzwang.Name = "lbl_schlagzwang";
            this.lbl_schlagzwang.Size = new System.Drawing.Size(115, 20);
            this.lbl_schlagzwang.TabIndex = 8;
            this.lbl_schlagzwang.Text = "Schlagzwang";
            // 
            // rdb_schlagzwangEin
            // 
            this.rdb_schlagzwangEin.AutoSize = true;
            this.rdb_schlagzwangEin.Location = new System.Drawing.Point(12, 33);
            this.rdb_schlagzwangEin.Name = "rdb_schlagzwangEin";
            this.rdb_schlagzwangEin.Size = new System.Drawing.Size(40, 17);
            this.rdb_schlagzwangEin.TabIndex = 9;
            this.rdb_schlagzwangEin.TabStop = true;
            this.rdb_schlagzwangEin.Text = "Ein";
            this.rdb_schlagzwangEin.UseVisualStyleBackColor = true;
            // 
            // rdb_schlagzwangAus
            // 
            this.rdb_schlagzwangAus.AutoSize = true;
            this.rdb_schlagzwangAus.Location = new System.Drawing.Point(61, 33);
            this.rdb_schlagzwangAus.Name = "rdb_schlagzwangAus";
            this.rdb_schlagzwangAus.Size = new System.Drawing.Size(43, 17);
            this.rdb_schlagzwangAus.TabIndex = 10;
            this.rdb_schlagzwangAus.TabStop = true;
            this.rdb_schlagzwangAus.Text = "Aus";
            this.rdb_schlagzwangAus.UseVisualStyleBackColor = true;
            // 
            // pnl_schlagzwang
            // 
            this.pnl_schlagzwang.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_schlagzwang.Controls.Add(this.txt_schlagzwang);
            this.pnl_schlagzwang.Controls.Add(this.lbl_schlagzwang);
            this.pnl_schlagzwang.Controls.Add(this.rdb_schlagzwangAus);
            this.pnl_schlagzwang.Controls.Add(this.rdb_schlagzwangEin);
            this.pnl_schlagzwang.Location = new System.Drawing.Point(33, 247);
            this.pnl_schlagzwang.Name = "pnl_schlagzwang";
            this.pnl_schlagzwang.Size = new System.Drawing.Size(125, 94);
            this.pnl_schlagzwang.TabIndex = 11;
            // 
            // txt_schlagzwang
            // 
            this.txt_schlagzwang.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txt_schlagzwang.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_schlagzwang.Location = new System.Drawing.Point(5, 57);
            this.txt_schlagzwang.Multiline = true;
            this.txt_schlagzwang.Name = "txt_schlagzwang";
            this.txt_schlagzwang.Size = new System.Drawing.Size(115, 32);
            this.txt_schlagzwang.TabIndex = 11;
            this.txt_schlagzwang.Text = "Nur teilweise Funktionsfähig.";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.pnl_schlagzwang);
            this.Controls.Add(this.lbl_console);
            this.Controls.Add(this.txt_console);
            this.Controls.Add(this.pnl_whichPlayer);
            this.Controls.Add(this.btn_newRound);
            this.Controls.Add(this.pic_Spielfeld);
            this.Name = "Form1";
            this.Text = "Dame";
            this.LocationChanged += new System.EventHandler(this.Form1_LocationChanged);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pic_Spielfeld)).EndInit();
            this.pnl_whichPlayer.ResumeLayout(false);
            this.pnl_whichPlayer.PerformLayout();
            this.pnl_schlagzwang.ResumeLayout(false);
            this.pnl_schlagzwang.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pic_Spielfeld;
        private System.Windows.Forms.Button btn_newRound;
        private System.Windows.Forms.Label lbl_spieler1;
        private System.Windows.Forms.Label lbl_spieler2;
        private System.Windows.Forms.Panel pnl_whichPlayer;
        private System.Windows.Forms.Label lbl_whichPlayer;
        private System.Windows.Forms.TextBox txt_console;
        private System.Windows.Forms.Label lbl_console;
        private System.Windows.Forms.Label lbl_schlagzwang;
        private System.Windows.Forms.RadioButton rdb_schlagzwangEin;
        private System.Windows.Forms.RadioButton rdb_schlagzwangAus;
        private System.Windows.Forms.Panel pnl_schlagzwang;
        private System.Windows.Forms.TextBox txt_schlagzwang;
    }
}

