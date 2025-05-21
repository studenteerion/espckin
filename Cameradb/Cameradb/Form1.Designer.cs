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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.descrizione = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // ip_txt
            // 
            this.ip_txt.Location = new System.Drawing.Point(77, 116);
            this.ip_txt.Name = "ip_txt";
            this.ip_txt.Size = new System.Drawing.Size(100, 20);
            this.ip_txt.TabIndex = 0;
            this.ip_txt.TextChanged += new System.EventHandler(this.ip_txt_TextChanged);
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
            this.coordinate_txt.Location = new System.Drawing.Point(436, 116);
            this.coordinate_txt.Name = "coordinate_txt";
            this.coordinate_txt.Size = new System.Drawing.Size(100, 20);
            this.coordinate_txt.TabIndex = 3;
            // 
            // aggiungi_btn
            // 
            this.aggiungi_btn.Location = new System.Drawing.Point(77, 398);
            this.aggiungi_btn.Name = "aggiungi_btn";
            this.aggiungi_btn.Size = new System.Drawing.Size(124, 41);
            this.aggiungi_btn.TabIndex = 4;
            this.aggiungi_btn.Text = "AGGIUNGI";
            this.aggiungi_btn.UseVisualStyleBackColor = true;
            this.aggiungi_btn.Click += new System.EventHandler(this.aggiungi_btn_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(407, 398);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(129, 41);
            this.button1.TabIndex = 6;
            this.button1.Text = "MODIFICA";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(77, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "ipaddress";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(195, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "nome";
            // 
            // descrizione
            // 
            this.descrizione.AutoSize = true;
            this.descrizione.Location = new System.Drawing.Point(319, 96);
            this.descrizione.Name = "descrizione";
            this.descrizione.Size = new System.Drawing.Size(60, 13);
            this.descrizione.TabIndex = 9;
            this.descrizione.Text = "descrizione";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(449, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "coordinate";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(80, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(448, 39);
            this.label3.TabIndex = 11;
            this.label3.Text = "GESTIONE FOTOCAMERE";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(236, 398);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(110, 41);
            this.button2.TabIndex = 12;
            this.button2.Text = "ELIMINA";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(77, 152);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(491, 218);
            this.dataGridView1.TabIndex = 13;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(820, 487);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.descrizione);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.aggiungi_btn);
            this.Controls.Add(this.coordinate_txt);
            this.Controls.Add(this.descrizione_txt);
            this.Controls.Add(this.nome_txt);
            this.Controls.Add(this.ip_txt);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ip_txt;
        private System.Windows.Forms.TextBox nome_txt;
        private System.Windows.Forms.TextBox descrizione_txt;
        private System.Windows.Forms.TextBox coordinate_txt;
        private System.Windows.Forms.Button aggiungi_btn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label descrizione;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}

