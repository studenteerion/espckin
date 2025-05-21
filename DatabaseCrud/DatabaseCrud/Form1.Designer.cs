namespace DatabaseCrud
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.targa = new System.Windows.Forms.Label();
            this.textBoxtarga = new System.Windows.Forms.TextBox();
            this.nomeprof = new System.Windows.Forms.Label();
            this.textBoxnome = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.accesso = new System.Windows.Forms.Label();
            this.textBoxaccesso = new System.Windows.Forms.TextBox();
            this.id = new System.Windows.Forms.Label();
            this.textBoxid = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxmail = new System.Windows.Forms.TextBox();
            this.mail = new System.Windows.Forms.Label();
            this.cognome = new System.Windows.Forms.Label();
            this.textBoxcognome = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(32, 74);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(718, 411);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(770, 74);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(212, 68);
            this.button1.TabIndex = 1;
            this.button1.Text = "Carica Dati";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(770, 335);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(212, 77);
            this.button2.TabIndex = 2;
            this.button2.Text = "Elimina";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Georgia", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(25, 500);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(240, 38);
            this.label1.TabIndex = 3;
            this.label1.Text = "Modifica Dati";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // targa
            // 
            this.targa.AutoSize = true;
            this.targa.Location = new System.Drawing.Point(29, 554);
            this.targa.Name = "targa";
            this.targa.Size = new System.Drawing.Size(35, 13);
            this.targa.TabIndex = 4;
            this.targa.Text = "Targa";
            // 
            // textBoxtarga
            // 
            this.textBoxtarga.Location = new System.Drawing.Point(70, 551);
            this.textBoxtarga.Name = "textBoxtarga";
            this.textBoxtarga.Size = new System.Drawing.Size(100, 20);
            this.textBoxtarga.TabIndex = 5;
            this.textBoxtarga.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // nomeprof
            // 
            this.nomeprof.AutoSize = true;
            this.nomeprof.Location = new System.Drawing.Point(176, 554);
            this.nomeprof.Name = "nomeprof";
            this.nomeprof.Size = new System.Drawing.Size(88, 13);
            this.nomeprof.TabIndex = 6;
            this.nomeprof.Text = "Nome Professore";
            // 
            // textBoxnome
            // 
            this.textBoxnome.Location = new System.Drawing.Point(267, 551);
            this.textBoxnome.Name = "textBoxnome";
            this.textBoxnome.Size = new System.Drawing.Size(100, 20);
            this.textBoxnome.TabIndex = 7;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(770, 206);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(212, 66);
            this.button3.TabIndex = 8;
            this.button3.Text = "Modifica";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // accesso
            // 
            this.accesso.AutoSize = true;
            this.accesso.Location = new System.Drawing.Point(373, 554);
            this.accesso.Name = "accesso";
            this.accesso.Size = new System.Drawing.Size(48, 13);
            this.accesso.TabIndex = 9;
            this.accesso.Text = "Accesso";
            this.accesso.Click += new System.EventHandler(this.label4_Click);
            // 
            // textBoxaccesso
            // 
            this.textBoxaccesso.Location = new System.Drawing.Point(418, 551);
            this.textBoxaccesso.Name = "textBoxaccesso";
            this.textBoxaccesso.Size = new System.Drawing.Size(100, 20);
            this.textBoxaccesso.TabIndex = 10;
            this.textBoxaccesso.TextChanged += new System.EventHandler(this.textBoxaccesso_TextChanged);
            // 
            // id
            // 
            this.id.AutoSize = true;
            this.id.Location = new System.Drawing.Point(524, 554);
            this.id.Name = "id";
            this.id.Size = new System.Drawing.Size(18, 13);
            this.id.TabIndex = 11;
            this.id.Text = "ID";
            // 
            // textBoxid
            // 
            this.textBoxid.Location = new System.Drawing.Point(548, 551);
            this.textBoxid.Name = "textBoxid";
            this.textBoxid.Size = new System.Drawing.Size(100, 20);
            this.textBoxid.TabIndex = 12;
            this.textBoxid.TextChanged += new System.EventHandler(this.textBoxid_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(654, 554);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 13);
            this.label6.TabIndex = 13;
            // 
            // textBoxmail
            // 
            this.textBoxmail.Location = new System.Drawing.Point(857, 551);
            this.textBoxmail.Name = "textBoxmail";
            this.textBoxmail.Size = new System.Drawing.Size(184, 20);
            this.textBoxmail.TabIndex = 14;
            this.textBoxmail.TextChanged += new System.EventHandler(this.textBoxmail_TextChanged);
            // 
            // mail
            // 
            this.mail.AutoSize = true;
            this.mail.Location = new System.Drawing.Point(825, 554);
            this.mail.Name = "mail";
            this.mail.Size = new System.Drawing.Size(26, 13);
            this.mail.TabIndex = 15;
            this.mail.Text = "Mail";
            this.mail.Click += new System.EventHandler(this.label7_Click);
            // 
            // cognome
            // 
            this.cognome.AutoSize = true;
            this.cognome.Location = new System.Drawing.Point(661, 554);
            this.cognome.Name = "cognome";
            this.cognome.Size = new System.Drawing.Size(52, 13);
            this.cognome.TabIndex = 16;
            this.cognome.Text = "Cognome";
            // 
            // textBoxcognome
            // 
            this.textBoxcognome.Location = new System.Drawing.Point(719, 551);
            this.textBoxcognome.Name = "textBoxcognome";
            this.textBoxcognome.Size = new System.Drawing.Size(100, 20);
            this.textBoxcognome.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Georgia", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(34, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(555, 41);
            this.label2.TabIndex = 18;
            this.label2.Text = "CRUD GESTIONE DATABASE";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1204, 706);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxcognome);
            this.Controls.Add(this.cognome);
            this.Controls.Add(this.mail);
            this.Controls.Add(this.textBoxmail);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxid);
            this.Controls.Add(this.id);
            this.Controls.Add(this.textBoxaccesso);
            this.Controls.Add(this.accesso);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBoxnome);
            this.Controls.Add(this.nomeprof);
            this.Controls.Add(this.textBoxtarga);
            this.Controls.Add(this.targa);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label targa;
        private System.Windows.Forms.TextBox textBoxtarga;
        private System.Windows.Forms.Label nomeprof;
        private System.Windows.Forms.TextBox textBoxnome;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label accesso;
        private System.Windows.Forms.TextBox textBoxaccesso;
        private System.Windows.Forms.Label id;
        private System.Windows.Forms.TextBox textBoxid;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxmail;
        private System.Windows.Forms.Label mail;
        private System.Windows.Forms.Label cognome;
        private System.Windows.Forms.TextBox textBoxcognome;
        private System.Windows.Forms.Label label2;
    }
}

