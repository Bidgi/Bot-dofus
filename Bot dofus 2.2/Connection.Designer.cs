namespace Bot_dofus_2._2
{
    partial class Connection
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
            this.txtPseudo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnConnection = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMDP = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.LabelCreeCompte = new System.Windows.Forms.Label();
            this.labelErreur2 = new System.Windows.Forms.Label();
            this.labelErreur = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtPseudo
            // 
            this.txtPseudo.Location = new System.Drawing.Point(28, 74);
            this.txtPseudo.Name = "txtPseudo";
            this.txtPseudo.Size = new System.Drawing.Size(222, 20);
            this.txtPseudo.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Nom d\'utilisateur :";
            // 
            // BtnConnection
            // 
            this.BtnConnection.Location = new System.Drawing.Point(28, 191);
            this.BtnConnection.Name = "BtnConnection";
            this.BtnConnection.Size = new System.Drawing.Size(222, 23);
            this.BtnConnection.TabIndex = 3;
            this.BtnConnection.Text = "Ce connecter";
            this.BtnConnection.UseVisualStyleBackColor = true;
            this.BtnConnection.Click += new System.EventHandler(this.BtnConnection_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 136);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Mot de passe :";
            // 
            // txtMDP
            // 
            this.txtMDP.Location = new System.Drawing.Point(28, 152);
            this.txtMDP.Name = "txtMDP";
            this.txtMDP.Size = new System.Drawing.Size(222, 20);
            this.txtMDP.TabIndex = 2;
            this.txtMDP.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Adresse email :";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(28, 113);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(222, 20);
            this.txtEmail.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MV Boli", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(40, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(198, 34);
            this.label4.TabIndex = 7;
            this.label4.Text = "Chest Hunter";
            // 
            // LabelCreeCompte
            // 
            this.LabelCreeCompte.AutoSize = true;
            this.LabelCreeCompte.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelCreeCompte.Location = new System.Drawing.Point(162, 175);
            this.LabelCreeCompte.Name = "LabelCreeCompte";
            this.LabelCreeCompte.Size = new System.Drawing.Size(88, 13);
            this.LabelCreeCompte.TabIndex = 8;
            this.LabelCreeCompte.Text = "Crée un compte :";
            this.LabelCreeCompte.Click += new System.EventHandler(this.LabelCreeCompte_Click);
            // 
            // labelErreur2
            // 
            this.labelErreur2.AutoSize = true;
            this.labelErreur2.ForeColor = System.Drawing.Color.Red;
            this.labelErreur2.Location = new System.Drawing.Point(12, 234);
            this.labelErreur2.Name = "labelErreur2";
            this.labelErreur2.Size = new System.Drawing.Size(35, 13);
            this.labelErreur2.TabIndex = 10;
            this.labelErreur2.Text = "label1";
            this.labelErreur2.Visible = false;
            // 
            // labelErreur
            // 
            this.labelErreur.AutoSize = true;
            this.labelErreur.ForeColor = System.Drawing.Color.Red;
            this.labelErreur.Location = new System.Drawing.Point(12, 221);
            this.labelErreur.Name = "labelErreur";
            this.labelErreur.Size = new System.Drawing.Size(35, 13);
            this.labelErreur.TabIndex = 9;
            this.labelErreur.Text = "label1";
            this.labelErreur.Visible = false;
            // 
            // Connection
            // 
            this.AcceptButton = this.BtnConnection;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 296);
            this.Controls.Add(this.labelErreur2);
            this.Controls.Add(this.labelErreur);
            this.Controls.Add(this.LabelCreeCompte);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMDP);
            this.Controls.Add(this.BtnConnection);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPseudo);
            this.Name = "Connection";
            this.Text = "Connection";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Connection_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPseudo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnConnection;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMDP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label LabelCreeCompte;
        private System.Windows.Forms.Label labelErreur2;
        private System.Windows.Forms.Label labelErreur;
    }
}