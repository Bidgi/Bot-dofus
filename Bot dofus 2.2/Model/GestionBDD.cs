using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bot_dofus_2._2.Model
{
    public class GestionBDD
    {
        #region Attributs
        /// <summary>
        /// creation de la connection a la base de donnee
        /// </summary>
        public static MySqlConnection mySqlConnection = new MySqlConnection("server=localhost;userid=root;database=chesthunter 2.2");
        #endregion

        #region Connection
        /// <summary>
        /// test l'ouverture de la connection a la base et c'est si impossible renvoie un message d'erreur
        /// </summary>
        public static void OpenConnection() { try { mySqlConnection.Open(); } catch { MessageBox.Show("le serveur de donner n'est pas accesible"); } }

        /// <summary>
        /// verifie les identifien et mot de passe pour valider la connection d'un prof
        /// </summary>
        /// <param name="Nom"> nom du prof </param>
        /// <param name="mdp"> mdp du prof </param>
        /// <returns></returns>
        public static bool TestConnection(string PSEUDO, string email, string mdp, string mac)
        {
            List<List<string>> result = new List<List<string>>();
            var cmd = new MySqlCommand("SELECT MDP_MEMBRE, EMAIL_MEMBRE, PSEUDO_MEMBRE, ADRESSE_MAC FROM membre INNER JOIN mac on mac.id_mac = membre.id_mac", mySqlConnection);
            using (var reader = cmd.ExecuteReader()) while (reader.Read()) result.Add(new List<string>() { reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3) });
            foreach (List<string> unMembre in result) if (unMembre.ElementAt(0) == mdp && unMembre.ElementAt(1) == email && unMembre.ElementAt(2) == PSEUDO && unMembre.ElementAt(3) == mac) return true;
            return false;
        }
        public static bool CreeNouveauMembre(string mdp, string email, string pseudo, string statu, string premium, string mac)
        {
            try
            {
                var cmd = new MySqlCommand("INSERT INTO mac VALUE (" + mac + ")", mySqlConnection);
                cmd.ExecuteNonQuery();
                List<string> result = new List<string>();
                cmd = new MySqlCommand("select id_mac from mac where adresse_mac = \'"+ mac + "\'");
                using (var reader = cmd.ExecuteReader()) while (reader.Read()) result.Add(reader.GetString(0));
                cmd = new MySqlCommand("INSERT INTO membre VALUE (" + mdp + "," + email + "," + pseudo + "," + DateTime.Now + "," + statu + "," + premium + "," + result.ElementAt(0) + ")", mySqlConnection);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch { return false; }
        }
        #endregion
    }
}