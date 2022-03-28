using Bot_dofus_2._2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bot_dofus_2._2
{
    public partial class Connection : Form
    {
        public Connection()
        {
            InitializeComponent();
        }
        private void Connection_Load(object sender, EventArgs e)
        {
            GestionBDD.OpenConnection();
        }
        private void LabelCreeCompte_Click(object sender, EventArgs e)
        {
            CreeCompte creeCompte = new CreeCompte();
            creeCompte.Show();
            this.Visible = false;
        }   
        private void BtnConnection_Click(object sender, EventArgs e)
        {
            if (GestionBDD.TestConnection(txtPseudo.Text, txtEmail.Text, txtMDP.Text, Utilitaires.AdresseMac()))
            {
                Accueil accueil = new Accueil();
                accueil.Show();
                this.Visible = false;
            }
            else
            {
                labelErreur.Text = "Vos donnée sont incorrect.";
                labelErreur.Visible = true;
                labelErreur2.Text = "Veuille vérifier vos donnée.";
                labelErreur2.Visible = true;
            }
        }
    }
}
