using Bot_dofus_2._2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bot_dofus_2._2
{
    public partial class CreeCompte : Form
    {
        public CreeCompte()
        {
            InitializeComponent();
        }
        private void BtValider_Click(object sender, EventArgs e)
        {
            if(txtMDP.Text == txtMDPverif.Text && txtEmail.Text == txtEmailVerif.Text)
            {
                if (GestionBDD.CreeNouveauMembre(txtMDP.Text, txtEmail.Text, txtPseudo.Text, "membre", "true", Utilitaires.AdresseMac()))
                {
                    Connection connection = new Connection();
                    connection.Show();
                    this.Close();
                }
            }
            else
            {
                labelErreur.Text = "Vos donnée sont incorrect.";
                labelErreur.Visible = true;
                labelErreur2.Text =  "Veuille vérifier vos donnée.";
                labelErreur2.Visible = true;
            }
        }
    }
}
