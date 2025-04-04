namespace Cameradb
{
    partial class Form1
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.ip_txt = new System.Windows.Forms.TextBox();
            this.nome_txt = new System.Windows.Forms.TextBox();
            this.descrizione_txt = new System.Windows.Forms.TextBox();
            this.coordinate_txt = new System.Windows.Forms.TextBox();
            this.aggiungi_btn = new System.Windows.Forms.Button();
            this.cameraListBox = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ip_txt
            // 
            this.ip_txt.Location = new System.Drawing.Point(77, 116);
            this.ip_txt.Name = "ip_txt";
            this.ip_txt.Size = new System.Drawing.Size(100, 20);
            this.ip_txt.TabIndex = 0;
            // 
            // nome_txt
            // 
            this.nome_txt.Location = new System.Drawing.Point(198, 116);
            this.nome_txt.Name = "nome_txt";
            this.nome_txt.Size = new System.Drawing.Size(100, 20);
            this.nome_txt.TabIndex = 1;
            // 
            // descrizione_txt
            // 
            this.descrizione_txt.Location = new System.Drawing.Point(319, 116);
            this.descrizione_txt.Name = "descrizione_txt";
            this.descrizione_txt.Size = new System.Drawing.Size(100, 20);
            this.descrizione_txt.TabIndex = 2;
            // 
            // coordinate_txt
            // 
            this.coordinate_txt.Location = new System.Drawing.Point(449, 116);
            this.coordinate_txt.Name = "coordinate_txt";
            this.coordinate_txt.Size = new System.Drawing.Size(100, 20);
            this.coordinate_txt.TabIndex = 3;
            // 
            // aggiungi_btn
            // 
            this.aggiungi_btn.Location = new System.Drawing.Point(622, 116);
            this.aggiungi_btn.Name = "aggiungi_btn";
            this.aggiungi_btn.Size = new System.Drawing.Size(100, 20);
            this.aggiungi_btn.TabIndex = 4;
            this.aggiungi_btn.Text = "AGGIUNGI";
            this.aggiungi_btn.UseVisualStyleBackColor = true;
            this.aggiungi_btn.Click += new System.EventHandler(this.aggiungi_btn_Click);
            // 
            // cameraListBox
            // 
            this.cameraListBox.FormattingEnabled = true;
            this.cameraListBox.Location = new System.Drawing.Point(533, 187);
            this.cameraListBox.Name = "cameraListBox";
            this.cameraListBox.Size = new System.Drawing.Size(189, 173);
            this.cameraListBox.TabIndex = 5;
            this.cameraListBox.SelectedIndexChanged += new System.EventHandler(this.cameraListBox_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(109, 220);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cameraListBox);
            this.Controls.Add(this.aggiungi_btn);
            this.Controls.Add(this.coordinate_txt);
            this.Controls.Add(this.descrizione_txt);
            this.Controls.Add(this.nome_txt);
            this.Controls.Add(this.ip_txt);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ip_txt;
        private System.Windows.Forms.TextBox nome_txt;
        private System.Windows.Forms.TextBox descrizione_txt;
        private System.Windows.Forms.TextBox coordinate_txt;
        private System.Windows.Forms.Button aggiungi_btn;
        private System.Windows.Forms.ListBox cameraListBox;
        private System.Windows.Forms.Button button1;
    }
}

