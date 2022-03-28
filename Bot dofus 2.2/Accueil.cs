using Bot_dofus_2._2.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Tesseract;

namespace Bot_dofus_2._2
{
    public partial class Accueil : Form
    {
        public Accueil() { InitializeComponent(); }

        private void Button1_Click(object sender, EventArgs e)
        {
            Main();
        }

        /// methode principale groupant toute les autre methode
        private void Main()
        {
            var mysqlconnection = new MySqlConnection("server=localhost;userid=root;database=chesthunter 2.2");
            try { mysqlconnection.Open(); }                                                                                                 // ✔ ouverture de la base de donnee
            catch { MessageBox.Show("Connection a la base de donner impossible"); this.Close(); }
            Utilitaires.Intialisation();                                                                                                    // ✔ intialise l'interface
            int worldmaps = 0; // World();                                                                                                  // ❌ recharche du mods actuel
            int nombreEtape = Utilitaires.NombreEtape();                                                                                    // ✔ recherche du nom d'etape
            txtConsole.AppendText("\r\n Nombre total d'étape : " + nombreEtape.ToString());
            for (int e = 0; e < nombreEtape; e++)
            {
                txtConsole.AppendText("\r\n Etape actuel : " + e.ToString());
                Thread.Sleep(100);
                int nombreIndice = Utilitaires.NombreIndice();                                                                              // ✔ recherche du nombre d'indice
                txtConsole.AppendText("\r\n  Nombre total d'indice : " + nombreIndice.ToString());
                for (int i = 0; i < nombreIndice; i++)
                {
                    txtConsole.AppendText("\r\n  Indice actuel : " + i.ToString());
                    if (Utilitaires.Jalon(325 + (i * 29)) == false)                                                                         // ✔ check si le jalon de l'indice actuel est valider ou non
                    {
                        string direction = Utilitaires.DirectionIndice(314 + (i * 29), mysqlconnection);                                    // ✔ recherche de la direction de l'indice
                        txtConsole.AppendText("\r\n  Direction : " + direction);
                        Thread.Sleep(100);
                        string nomIndice = Utilitaires.NomIndice(315 + (i * 29), mysqlconnection);                                          // ✔ recherche du nom de l'indice
                        txtConsole.AppendText("\r\n  Nom de l'indice : " + nomIndice);
                        List<int> listXY = Utilitaires.CaseActuel(mysqlconnection);                                                         // ✔ recherche de x, y de la carte actuel
                    up:
                        txtConsole.AppendText("\r\n Position actuel : X = " + listXY.ElementAt(0)  + " Y = " + listXY.ElementAt(1));
                        if (nomIndice == "Phorreur")                                                                                        // ✔ si l'indice et un phorreur
                        {
                            txtConsole.AppendText("\r\n  Indice phorreur");
                            bool phorreurTrouver = false;
                            int compteurDeplacement = 0;
                            Utilitaires.Clic(new Point(640, 220));                                                                          // ✔ fermeture le la fenetre de chasse
                            while (phorreurTrouver != true)
                            {
                                listXY = Utilitaires.CaseActuel(mysqlconnection);                                                           // ✔ recherche de x, y de la carte actuel
                                Utilitaires.Deplacement(direction, listXY.ElementAt(0), listXY.ElementAt(1), worldmaps, 1, mysqlconnection, nomIndice);
                                phorreurTrouver = Utilitaires.Phorreur(mysqlconnection);                                                    // recherche un phorreur sur la carte actuel
                                txtConsole.AppendText("\r\n Fin de la recherche du phorreur");
                                if (phorreurTrouver)
                                {
                                    Utilitaires.Clic(new Point(640, 220));
                                    if (CarteDejaCheck.VerifCheckMap(listXY.ElementAt(0), listXY.ElementAt(1)) == false)                    // check si la carte actuel n'a pas deja ete valider
                                    {
                                        txtConsole.AppendText("\r\n  Validation du jalon");
                                        Thread.Sleep(400);
                                        Utilitaires.Clic(new Point(625, 330 + (i * 29)));                                                   // ajoute a la classe CarteDejaCheck une nouvelle carte check
                                        Thread.Sleep(500);
                                    }
                                    else goto up;
                                }
                                else
                                {
                                    compteurDeplacement++;
                                    if (compteurDeplacement >= 10)                                                                          // ✔ check si il y a eu 10 deplacement si oui changement de direction a l'enverse pour retest la cherche d'un phorreur
                                    {
                                        switch (direction)
                                        {
                                            case "left":
                                                direction = "right";
                                                break;
                                            case "right":
                                                direction = "left";
                                                break;
                                            case "top":
                                                direction = "bottom";
                                                break;
                                            case "bottom":
                                                direction = "top";
                                                break;
                                        }
                                        compteurDeplacement = 0;
                                    }
                                }
                            }
                        }
                        else
                        {
                            txtConsole.AppendText("\r\n  Indice normale");
                            listXY = Utilitaires.CaseActuel(mysqlconnection);                                                               // ✔ recherche de x, y de la carte actuel
                            int nombreDeplacement = Utilitaires.NouvellePossition(listXY.ElementAt(0), listXY.ElementAt(1), worldmaps, direction, nomIndice, mysqlconnection);  // ✔ recherche du nombre de deplacement a effectuer pour arriver a l'indice
                            txtConsole.AppendText("\r\n  Nombre de déplacement à effectuer : " + nombreDeplacement);
                            Utilitaires.Deplacement(direction, listXY.ElementAt(0), listXY.ElementAt(1), worldmaps, nombreDeplacement, mysqlconnection, "");
                            if (CarteDejaCheck.VerifCheckMap(listXY.ElementAt(0), listXY.ElementAt(1)) == false)                                                                // check si la carte actuel n'a pas deja ete valider
                            {
                                txtConsole.AppendText("\r\n Validation du jalon");
                                Point PointJalon = new Point(625, 330 + (i * 29));
                                Utilitaires.Clic(PointJalon);
                                CarteDejaCheck.AjouteCheckMap(listXY.ElementAt(0), listXY.ElementAt(1));                                                                        // ajoute a la classe CarteDejaCheck une nouvelle carte check
                                Thread.Sleep(500);
                            }
                            else goto up;
                        }
                        Thread.Sleep(1000);
                    }
                    txtConsole.AppendText("\r\n  Verification de l'indice actuel");
                }
                txtConsole.AppendText("\r\n  Verification étape");
                Point verife = new Point(610, 336 + (nombreIndice * 29));                                                                // ✔ validation du jalon une fois l'indice trouver
                Utilitaires.Clic(verife);
            }
            mysqlconnection.Close();
        }
    }
}