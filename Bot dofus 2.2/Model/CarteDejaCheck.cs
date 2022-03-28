using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_dofus_2._2.Model
{
    class CarteDejaCheck
    {
        #region Attribut
        public static List<CarteDejaCheck> CollClasseCarteDejaCheck = new List<CarteDejaCheck>();
        private int _X;
        private int _Y;
        #endregion

        #region Constructeur
        public CarteDejaCheck(int unX, int unY)
        {
            this.X = unX;
            this.Y = unY;
            CollClasseCarteDejaCheck.Add(this);
        }
        #endregion

        #region Get-Set
        public int X { get => _X; set => _X = value; }
        public int Y { get => _Y; set => _Y = value; }
        #endregion

        #region Methode
        /// ✔ permet d'ajouter un map check
        public static void AjouteCheckMap(int x, int y)
        {
            new CarteDejaCheck(x, y);
        }

        /// ✔ permet de verifier sur la carte actuel a deja ete check
        public static bool VerifCheckMap(int x, int y)
        {
            foreach (CarteDejaCheck carteDejaCheck in CollClasseCarteDejaCheck) if (carteDejaCheck.X == x && carteDejaCheck.Y == y) return true;
            return false;
        }
        #endregion
    }
}
