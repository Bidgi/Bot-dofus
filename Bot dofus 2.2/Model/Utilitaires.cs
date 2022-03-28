using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Tesseract;

namespace Bot_dofus_2._2.Model
{
    class Utilitaires
    {
        #region Methode outil
        /// ✔ permet de transforme un char ex " en \"
        public static string ChangeChar(string paragraphe, List<int> intChar)
        {
            foreach (int charr in intChar) paragraphe.Replace(Char.ConvertFromUtf32(charr), "\\" + Char.ConvertFromUtf32(charr));
            return paragraphe;
        }

        /// ✔ donner la couleur du pixel dans la position donner
        public static Color GetPixel(Point position)
        {
            using (var bitmap = new Bitmap(1, 1))
            {
                using (var graphics = Graphics.FromImage(bitmap)) graphics.CopyFromScreen(position, new Point(0, 0), new Size(1, 1));
                return bitmap.GetPixel(0, 0);
            }
        }

        /// ✔ prend un screen de la taille du rectangle donner
        public static Bitmap CaptureScreenRegion(Rectangle rect)
        {
            Bitmap BMP = new Bitmap(rect.Width, rect.Height, PixelFormat.Format24bppRgb);
            Graphics GFX = System.Drawing.Graphics.FromImage(BMP);
            GFX.CopyFromScreen(rect.X, rect.Y, 0, 0, rect.Size, CopyPixelOperation.SourceCopy);
            return BMP;
        }

        /// ✔ cherche un pixel d'une couleur donner dans un rectangle ( shade_variation permet de modifier la presition)
        public static Point PixelSearch(Rectangle rect, Color Pixel_Color, int Shade_Variation)
        {
            Bitmap RegionIn_Bitmap = CaptureScreenRegion(rect);
            BitmapData RegionIn_BitmapData = RegionIn_Bitmap.LockBits(new Rectangle(0, 0, RegionIn_Bitmap.Width, RegionIn_Bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int[] Formatted_Color = new int[3] { Pixel_Color.B, Pixel_Color.G, Pixel_Color.R }; //bgr
            unsafe
            {
                for (int y = 0; y < RegionIn_BitmapData.Height; y++)
                {
                    byte* row = (byte*)RegionIn_BitmapData.Scan0 + (y * RegionIn_BitmapData.Stride);
                    for (int x = 0; x < RegionIn_BitmapData.Width; x++)
                    {
                        if (row[x * 3] >= (Formatted_Color[0] - Shade_Variation) & row[x * 3] <= (Formatted_Color[0] + Shade_Variation)) //blue
                        {
                            if (row[(x * 3) + 1] >= (Formatted_Color[1] - Shade_Variation) & row[(x * 3) + 1] <= (Formatted_Color[1] + Shade_Variation)) //green
                            {
                                if (row[(x * 3) + 2] >= (Formatted_Color[2] - Shade_Variation) & row[(x * 3) + 2] <= (Formatted_Color[2] + Shade_Variation)) //red
                                {
                                    return new Point(x + rect.X, y + rect.Y);
                                }
                            }
                        }
                    }
                }
            }
            return new Point(-1, -1);
        }

        /// ✔ attant que la couleur donner d'un pixel change
        public static void Pixelwait(int pointx, int pointy, string couleur)
        {
            Color pixel2 = Utilitaires.GetPixel(new Point(pointx, pointy));
            while (pixel2.Name != couleur) pixel2 = Utilitaires.GetPixel(new Point(pointx, pointy));
            Thread.Sleep(50);
        }

        /// ✔ permet de fait les conparaisson entre les image dans la methode casedepart()
        public static List<bool> GetHash(Bitmap bmpSource)
        {
            List<bool> lResult = new List<bool>();
            Bitmap bmpMin = new Bitmap(bmpSource, new Size(12, 25));
            for (int j = 0; j < bmpMin.Height; j++) for (int i = 0; i < bmpMin.Width; i++) lResult.Add(bmpMin.GetPixel(i, j).GetBrightness() < 0.5f);
            return lResult;
        }

        /// ✔ ouvrer une boite de dialog 
        public static string Dialog(string text)
        {
            Orthographe boite = new Orthographe();
            boite.label1.Text = text;
            boite.textBox1.Visible = true;
            boite.ShowDialog();
            return boite.textBox1.Text;
        }

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// ✔ permet de simuler un clic
        #region clic
        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        public static void Clic(Point point)
        {
            Cursor.Position = point;
            mouse_event(0x0002 | 0x0004, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
        }
        #endregion

        /// ✔ permet de recuperer l'adresse mac
        public static string AdresseMac()
        {
            string mac = string.Empty;
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"]) mac = mo["MacAddress"].ToString();
                mo.Dispose();
            }
            return mac;
        }
        #endregion

        #region Methode
        /// ✔ permet de modifier les parametre et la taille de l'application (dofus)
        public static void Intialisation()
        {
            var prcdofus = Process.GetProcessesByName("Dofus");
            if (prcdofus.Length > 0) Utilitaires.SetForegroundWindow(prcdofus[0].MainWindowHandle);
            Thread.Sleep(300);
            SendKeys.Send("{Esc}");
            Thread.Sleep(125);
            Utilitaires.Pixelwait(1031, 400, "ffa3c500");
            Utilitaires.Clic(new Point(880, 400));              // point position btn option
            Thread.Sleep(125);
            Utilitaires.Pixelwait(1435, 773, "ffbfe700");
            Utilitaires.Clic(new Point(770, 315));              // point btn resolution basse
            Thread.Sleep(125);
            Utilitaires.Clic(new Point(1300, 480));             // point btn mode creature 
            Thread.Sleep(125);
            Utilitaires.Pixelwait(720, 314, "ffbee700");
            Utilitaires.Clic(new Point(1300, 500));             // point btn mode creature 0
            Thread.Sleep(125);
            Utilitaires.Pixelwait(720, 314, "ff1f1e19");
            Utilitaires.Clic(new Point(500, 220));              // point position btn interface
            Thread.Sleep(125);
            Utilitaires.Pixelwait(564, 218, "ff464840");
            Utilitaires.Clic(new Point(1220, 320));             // point position btn réinitialiser
            Thread.Sleep(125);
            //pixelwait(1378, 320, "ff728a00");
            Thread.Sleep(125);
            Utilitaires.Clic(new Point(1400, 770));             // point position btn fermer
            Thread.Sleep(125);
            Utilitaires.Pixelwait(632, 223, "ffd4f301");
            if ("ffd4f301" == Utilitaires.GetPixel(new Point(634, 217)).Name)
            {
                Utilitaires.Clic(new Point(640, 220));          // point ferme/ouvrir fenetre phorreur
                Thread.Sleep(500);
            }
            Cursor.Position = new Point(0, 0);
        }

        /// ❌ permet de savoir dans quel monde nous somme (0 monde des 12, 1 incarnam, 2 arbre ..)
        public static int World()
        {
            Color pixel = Utilitaires.GetPixel(new Point(617, 301));
            if (pixel.R >= 70 && pixel.G >= 70 && pixel.B >= 70) return 0;
            else return 2;
        }

        /// ✔ dossier tesseract permet de trouver le nombre d'etape
        public static int NombreEtape()
        {
            var bmpScreenshot = new Bitmap(40, 20, PixelFormat.Format24bppRgb);
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(395, 253, 0, 0, new Size(40, 20), CopyPixelOperation.SourceCopy);
            using (var ocr = new TesseractEngine(Directory.GetCurrentDirectory(), "fra"))
            {
                var page = ocr.Process(new Bitmap(bmpScreenshot));
                string[] valuer = page.GetText().Split(new Char[] { '\n' });
                if (valuer[0] == "sa") valuer[0] = "3/4";
                else if (valuer[0] == "Us") valuer[0] = "1/3";
                else if (valuer[0] == "173") valuer[0] = "1/3";
                string[] substrings = valuer[0].Split(new Char[] { '/' });
                if (int.TryParse(substrings[0], out _) == false)
                {
                    string[] etapecorrigers = Utilitaires.Dialog("Entre l'étape (ex : 3/4) : ").Split(new Char[] { '/' });
                    return Convert.ToInt32(etapecorrigers[1]) - Convert.ToInt32(etapecorrigers[0]);
                }
                return Convert.ToInt32(substrings[1]) - Convert.ToInt32(substrings[0]);
            }
        }

        /// ✔ permet de trouve le nombre d'indice
        public static int NombreIndice()
        {
            int nbi = 0, y2 = 334;
            for (int i = 0; i < 10; i++)
            {
                Color verif2 = Utilitaires.GetPixel(new Point(600, y2));
                Cursor.Position = new Point(600, y2);
                Thread.Sleep(150);
                if (verif2 != Utilitaires.GetPixel(new Point(600, y2))) return nbi;
                nbi++;
                y2 += 29;
            }
            Cursor.Position = new Point(0, 0);
            return 0;
        }

        /// ✔ permet de si oui ou non un jalon est valider
        public static bool Jalon(int y)
        {
            Color jalon = Utilitaires.GetPixel(new Point(622, y));
            if (jalon.R >= 100 && jalon.G >= 100) return true;
            else return false;
        }

        /// ✔ dossier num permet de trouver la position X, Y de la maps actuel
        public static List<int> CaseActuel(MySqlConnection mySqlConnection)
        {
            List<bool>[] iHash = new List<bool>[12] { new List<bool>(), new List<bool>(), new List<bool>(), new List<bool>(), new List<bool>(), new List<bool>(), new List<bool>(), new List<bool>(), new List<bool>(), new List<bool>(), new List<bool>(), new List<bool>() };
            for (int n = 0; n < 12; n++)
            {
                string nom = n.ToString();
                if (n == 10) nom = "moin";
                else if (n == 11) nom = "virgule";
                var command = new MySqlCommand("Select bool From image Where type = \'position\' and libelle = \'" + nom + "\' ORDER by libelle, ordre", mySqlConnection);
                List<bool> resultPosition = new List<bool>();
                using (var reader = command.ExecuteReader()) while (reader.Read()) if (reader.GetString(0) != null) resultPosition.Add(reader.GetBoolean(0));
                foreach (var item in resultPosition)
                {
                    if (item) iHash.ElementAt(n).Add(true);
                    else iHash.ElementAt(n).Add(false);
                }
                resultPosition.Clear();
            }                                       // fin de creeation du tableau des list de bool des differente image

            int compteur = 0, compteurvirgule = 0, px = 19;
            string cordonne = "";
            while (compteur <= 7 && compteurvirgule < 2)
            {
                bool trouver = false;
                int trouve = 0, tx = 12;
                while (trouver == false)
                {
                    if (trouve >= 1) tx = 7;
                    if (trouve >= 2) tx = 6;
                    if (trouve >= 3) tx = 8;
                    var bmpScreenshot = new Bitmap(tx, 25, PixelFormat.Format24bppRgb);
                    var gfxScreenshot = Graphics.FromImage(bmpScreenshot);
                    gfxScreenshot.CopyFromScreen(px, 72, 0, 0, new Size(tx, 25), CopyPixelOperation.SourceCopy);
                    for (int i = 0; i < bmpScreenshot.Width; i++)
                    {
                        for (int n = 0; n < bmpScreenshot.Height; n++)
                        {
                            if (bmpScreenshot.GetPixel(i, n) == Color.FromArgb(228, 228, 226) || bmpScreenshot.GetPixel(i, n) == Color.FromArgb(229, 229, 226)) bmpScreenshot.SetPixel(i, n, Color.FromName("white"));
                            else bmpScreenshot.SetPixel(i, n, Color.FromName("black"));
                        }
                    }
                    List<bool> iHashActu = Utilitaires.GetHash(bmpScreenshot);
                    string[,] lesValeur = new string[,] {
                        { iHashActu.Zip(iHash[0], (i, j) => i == j).Count(eq => eq).ToString(), "0", "12" },
                        { iHashActu.Zip(iHash[1], (i, j) => i == j).Count(eq => eq).ToString(), "1", "8" },
                        { iHashActu.Zip(iHash[2], (i, j) => i == j).Count(eq => eq).ToString(), "2", "12" },
                        { iHashActu.Zip(iHash[3], (i, j) => i == j).Count(eq => eq).ToString(), "3", "12" },
                        { iHashActu.Zip(iHash[4], (i, j) => i == j).Count(eq => eq).ToString(), "4", "12" },
                        { iHashActu.Zip(iHash[5], (i, j) => i == j).Count(eq => eq).ToString(), "5", "12" },
                        { iHashActu.Zip(iHash[6], (i, j) => i == j).Count(eq => eq).ToString(), "6", "12" },
                        { iHashActu.Zip(iHash[7], (i, j) => i == j).Count(eq => eq).ToString(), "7", "6" },
                        { iHashActu.Zip(iHash[8], (i, j) => i == j).Count(eq => eq).ToString(), "8", "12" },
                        { iHashActu.Zip(iHash[9], (i, j) => i == j).Count(eq => eq).ToString(), "9", "12" },
                        { iHashActu.Zip(iHash[10], (i, j) => i == j).Count(eq => eq).ToString(), "-", "7" },
                        { iHashActu.Zip(iHash[11], (i, j) => i == j).Count(eq => eq).ToString(), ",", "5" } };
                    for (int i = 0; i < 12; i++)
                    {
                        if (lesValeur[i, 0] == "300")
                        {
                            cordonne += lesValeur[i, 1];
                            px += Convert.ToInt32(lesValeur[i, 2]);
                            if (lesValeur[i, 1] == ",") compteurvirgule++;
                            trouver = true;
                        }
                    }
                    if (trouver == false) trouve++;
                    if (trouver == false && trouve >= 4)
                    {
                        px++;
                        trouve = 0;
                        tx = 12;
                    }
                }
                compteur++;
            }
            string[] cordonner = cordonne.Split(new string[] { "," }, StringSplitOptions.None);
            return new List<int>() { Convert.ToInt32(cordonner[0]), Convert.ToInt32(cordonner[1]) };
        }

        /// ✔ dossier tesseract permet de lire l'indice en cour
        public static string NomIndice(int YNI, MySqlConnection mysqlconnection)
        {
            Thread.Sleep(200);
            var bmpScreenshot = new Bitmap(253, 30, PixelFormat.Format24bppRgb);
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(365, YNI, 0, 0, new Size(253, 30), CopyPixelOperation.SourceCopy);
            var img = new Bitmap(bmpScreenshot);
            using (var ocr = new TesseractEngine(Directory.GetCurrentDirectory(), "fra"))
            {
                var page = ocr.Process(img);
                string[] nomIndice = page.GetText().Split(new string[] { "\n" }, StringSplitOptions.None);
                if(nomIndice.ElementAt(0).Length > 8) if (nomIndice.ElementAt(0).Remove(8) == "Phorreur" || nomIndice.ElementAt(0).Remove(8) == "Pharreur") return "Phorreur";
                if (IndiceLibellerFaux(ChangeChar(nomIndice.ElementAt(0), new List<int>() { 39 }), mysqlconnection)) return IndiceLibellerVrai(ChangeChar(nomIndice.ElementAt(0), new List<int>() { 39 }), mysqlconnection);
                return nomIndice.ElementAt(0);
            }
        }
        /// permet de savoir si le nom de l'indice en faux ou vrai
        public static bool IndiceLibellerFaux(string nomindice, MySqlConnection mysqlconnection)
        {
            var cmd = new MySqlCommand("Select * From indicelibellefaux where LIBELLE_FAUX = \"" + nomindice + "\"", mysqlconnection);
            List<string> result = new List<string>() { null };
            using (var reader = cmd.ExecuteReader()) while (reader.Read()) result.Add(reader.GetString(0));
            if (result.Count > 1) return true;
            else return false;
        }
        /// permet de savoir quel est le vrai nom indice a partir d'un nom d'indice faux
        public static string IndiceLibellerVrai(string nomindice, MySqlConnection mysqlconnection)
        {
            var cmd = new MySqlCommand("Select LIBELLERINDICE From indicelibellefaux inner join indicetrie on indicetrie.ID_indicetrier = indicelibellefaux.id_indicetrier where LIBELLE_FAUX = " + nomindice, mysqlconnection);
            List<string> result = new List<string>() { null };
            using (var reader = cmd.ExecuteReader()) while (reader.Read()) result.Add(reader.GetString(0));
            return result.ElementAt(0);
        }

        /// ✔ dossier fleche permet de derterminer la diretion de l'indice en cour*
        public static string DirectionIndice(int y, MySqlConnection mySqlConnection)
        {
            string direction = null;
            while (direction == null)
            {
                var bmpScreenshot = new Bitmap(18, 18, PixelFormat.Format24bppRgb);
                var gfxScreenshot = Graphics.FromImage(bmpScreenshot);
                gfxScreenshot.CopyFromScreen(344, y, 0, 0, new Size(18, 18), CopyPixelOperation.SourceCopy);
                for (int x = 0; x < bmpScreenshot.Width; x++)
                {
                    for (int n = 0; n < bmpScreenshot.Height; n++)
                    {
                        if (bmpScreenshot.GetPixel(x, n).R <= 27 && bmpScreenshot.GetPixel(x, n).G <= 25 && bmpScreenshot.GetPixel(x, n).B <= 18) bmpScreenshot.SetPixel(x, n, Color.FromName("white"));
                        else bmpScreenshot.SetPixel(x, n, Color.FromName("black"));
                    }
                }
                List<bool> iHashActu = Utilitaires.GetHash(bmpScreenshot);
                string nom = "";
                List<bool>[] iHash = new List<bool>[4] { new List<bool>(), new List<bool>(), new List<bool>(), new List<bool>() };
                for (int i = 0; i < 4; i++)
                {
                    if (i == 0) nom = "Top";
                    if (i == 1) nom = "Bottom";
                    if (i == 2) nom = "Right";
                    if (i == 3) nom = "Left";
                    var command = new MySqlCommand("Select bool From image Where type = \'fleche\' and libelle = \'" + nom + "\' ORDER by libelle, ordre", mySqlConnection);
                    List<bool> resultPosition = new List<bool>();
                    using (var reader = command.ExecuteReader()) while (reader.Read()) resultPosition.Add(reader.GetBoolean(0));
                    foreach (var item in resultPosition)
                    {
                        if (item) iHash.ElementAt(i).Add(true);
                        else iHash.ElementAt(i).Add(false);
                    }
                }
                string[,] lesValeur = new string[,] {
                        { iHashActu.Zip(iHash[0], (i, j) => i == j).Count(eq => eq).ToString(), "top"},
                        { iHashActu.Zip(iHash[1], (i, j) => i == j).Count(eq => eq).ToString(), "bottom"},
                        { iHashActu.Zip(iHash[2], (i, j) => i == j).Count(eq => eq).ToString(), "right"},
                        { iHashActu.Zip(iHash[3], (i, j) => i == j).Count(eq => eq).ToString(), "left"} };
                for (int i = 0; i < 4; i++) if (lesValeur[i, 0] == "300") direction = lesValeur[i, 1];
                y++;
            }
            return direction;
        }

        /// ✔ recherche nombre deplacement sur la BDD
        public static int NouvellePossitionBDD(int x, int y, string nomIndice, string direction, int world, MySqlConnection mysqlconnection)
        {
            List<List<int>> resultXY = new List<List<int>>();
            var commandXY = new MySqlCommand("Select X, Y " +
                "From indiceenregistre " +
                "Inner Join indicetrie On indiceenregistre.ID_INDICETRIER = indicetrie.ID_INDICETRIER " +
                "Inner Join position on position.id_possition = indiceenregistre.id_possition " +
                "Where LIBELLERINDICE = \"" + nomIndice + "\" "+
                "And WORLD = "+ world, mysqlconnection);
            using (var reader = commandXY.ExecuteReader()) while (reader.Read()) resultXY.Add(new List<int>() { reader.GetInt32(0), reader.GetInt32(1) });
            int newP = 0, newX = 0;
            for (int i = 0; i < resultXY.Count(); i++)
            {
                if (resultXY.ElementAt(i) != null) 
                {
                    int xPosition = Convert.ToInt32(resultXY.ElementAt(i).ElementAt(0));
                    int yPosition = Convert.ToInt32(resultXY.ElementAt(i).ElementAt(1));
                    for (int n = 1; n <= 10; n++)
                    {
                        switch (direction)
                        {
                            case "top":
                                if (x == xPosition) if (yPosition == y - n) { newP = y; newX = yPosition; }
                                break;
                            case "right":
                                if (y == yPosition) if (xPosition == x + n) { newP = x; newX = xPosition; }
                                break;
                            case "bottom":
                                if (x == xPosition) if (yPosition == y + n) { newP = y; newX = yPosition; }
                                break;
                            case "left":
                                if (y == yPosition) if (xPosition == x - n) { newP = x; newX = xPosition; }
                                break;
                        }
                        if (newP != 0) break;
                    }
                    if (newP != 0) break;
                }
            }
            return Math.Abs(Math.Abs(newP) - Math.Abs(newX));
        }

        /// ✔ recherche nombre deplacement sur le site web*
        public static int NouvellePossitionWeb(int x, int y, string direction, string nomIndice, int world, MySqlConnection mysqlconnection)
        {
            List<string> resultNumeroIndice = new List<string>();
            var commandNumeroIndice = new MySqlCommand("Select NUMEROINDICE From indicetrie Where LIBELLERINDICE = \"" + nomIndice + "\" ", mysqlconnection);
            using (var reader = commandNumeroIndice.ExecuteReader()) while (reader.Read()) resultNumeroIndice.Add(reader.GetString(0));
            if (resultNumeroIndice.Count.Equals(0))
            {
                commandNumeroIndice = new MySqlCommand("Select NUMEROINDICE From indicetrie Inner Join indicelibellefaux On indicetrie.id_indicetrier = indicelibellefaux.id_indicetrier Where LIBELLERINDICE = \"" + nomIndice + "\" Or LIBELLE_FAUX = \"" + nomIndice + "\"", mysqlconnection);
                using (var reader = commandNumeroIndice.ExecuteReader()) while (reader.Read()) resultNumeroIndice.Add(reader.GetString(0));
            }
            Thread.Sleep(500);
            string codehtml = (new WebClient()).DownloadString("https://dofus-map.com/huntTool/getData.php?x=" + x + "&y=" + y + "&direction=" + direction + "&world=" + world + "&language=fr");
            if (codehtml != "{\"error\":20}" && codehtml != "")
            {
                string[] split = codehtml.Split(new Char[] { '[', 'n' });
                Array.Clear(split, 0, 3);
                foreach (string i in split)
                {
                    if (i != null)
                    {
                        string[] split1 = i.Split(new Char[] { ',' });
                        string[] split2 = split1.ElementAt(0).Split(new Char[] { ':' });
                        if (resultNumeroIndice.Count() > 0 && resultNumeroIndice.ElementAt(0) != "" && resultNumeroIndice.ElementAt(0) != null)
                        {
                            if (Convert.ToInt32(split2[1]) == Convert.ToInt32(resultNumeroIndice.ElementAt(0)))
                            {
                                string[] split3 = split1.ElementAt(3).Split(new Char[] { ':', '}' });
                                string[] splitx = split1.ElementAt(1).Split(new Char[] { ':' });
                                string[] splity = split1.ElementAt(2).Split(new Char[] { ':' });
                                if (int.TryParse(split2.ElementAt(1), out _) == true && int.TryParse(split3.ElementAt(1), out _) == true && int.TryParse(resultNumeroIndice.ElementAt(0), out _) == true)
                                {
                                    if (IndiceEnregistrerPressent(resultNumeroIndice.ElementAt(0), splitx.ElementAt(1), splity.ElementAt(1), mysqlconnection) == false) IndiceEnregistrerAdd(resultNumeroIndice.ElementAt(0), splitx.ElementAt(1), splity.ElementAt(1), world, mysqlconnection);
                                    return Convert.ToInt32(split3.ElementAt(1));
                                }
                            }
                        }
                    }
                }
            }
            return 0;
        }

        /// ✔ permet de calculer le nombre de deplacement a effectuer
        public static int NouvellePossition(int x, int y, int world, string direction, string nomIndice, MySqlConnection mysqlconnection)
        {
        rechercheNombreDeplcament:
            int nbd = NouvellePossitionBDD(x, y, nomIndice, direction, world, mysqlconnection);
            if (nbd == 0) nbd = NouvellePossitionWeb(x, y, direction, nomIndice, world, mysqlconnection);
            if (nbd == 0)
            {
                string indicescorriger = Utilitaires.Dialog("Entre la bonne orthographe de l'indice :");
                indicescorriger = Utilitaires.ChangeChar(indicescorriger, new List<int>() { 39 });
                var command = new MySqlCommand("INSERT INTO indicelibellefaux (ID_INDICETRIER, LIBELLE_FAUX) VALUES ('" + ChercheIDIndiceTrier(indicescorriger, mysqlconnection) + "','\"" + nomIndice + "\"')", mysqlconnection);
                command.ExecuteNonQuery();
                nomIndice = indicescorriger;
                goto rechercheNombreDeplcament;
            }
            return nbd;
        }

        /// ✔ permet de savoir id_indicetrier d'un indice a partir de son nom
        public static int ChercheIDIndiceTrier(string nomIndice, MySqlConnection mysqlconnection)
        {
            List<int> result = new List<int>();
            var cmd = new MySqlCommand("Select ID_INDICETRIER From indicetrie Where LIBELLERINDICE = \"" + nomIndice + "\"", mysqlconnection);
            using (var reader = cmd.ExecuteReader()) while (reader.Read()) result.Add(reader.GetInt32(0));
            return result.ElementAt(0);
        }

        /// ✔ permet de se deplacer
        public static void Deplacement(string direction, int x, int y, int world, int nbDeplacement, MySqlConnection mysqlconnection, string nomIndice)
        {
            Stopwatch timer = new Stopwatch();
            for (int i = 0; i < nbDeplacement; i++)
            {
            newMapSpe:
                timer.Start();
                Point map = ClicCarteSpe(x, y, world, direction, mysqlconnection);
                int Xtest = x, Ytest = y;
                switch (direction)
                {
                    case "top":
                        if (map.X != -1) Clic(map);
                        else Clic(new Point(1000, 24));
                        break;
                    case "right":
                        if (map.X != -1) Clic(map);
                        else Clic(new Point(1612, 510));
                        break;
                    case "bottom":
                        if (map.X != -1) Clic(map);
                        else Clic(new Point(1000, 933));
                        break;
                    case "left":
                        if (map.X != -1) Clic(map);
                        else Clic(new Point(310, 510));
                        break;
                }
                Console.WriteLine("zone 1");
                while (x == Xtest && y == Ytest)
                {
                    Thread.Sleep(40);
                    List<int> cordonneint = CaseActuel(mysqlconnection);
                    Xtest = cordonneint.ElementAt(0);
                    Ytest = cordonneint.ElementAt(1);
                    Console.WriteLine(Xtest + ", " + Ytest);
                    if (timer.ElapsedMilliseconds >= 15000)
                    {
                        timer.Stop();
                        AjouteCarteSpe(x, y, direction, mysqlconnection);
                        goto newMapSpe;
                    }
                }
                timer.Stop();
                timer.Reset();
                x = Xtest;
                y = Ytest;
                Console.WriteLine("zone 2");
            }
        }

        /// ✔ permet de recuper le point ou il faut clicer
        public static Point ClicCarteSpe(int x, int y, int world,string direction, MySqlConnection mysqlconnection)
        {
            Point points = new Point(-1, -1);
            List<string> resultPossition = new List<string>();
            var commandID_Position = new MySqlCommand("Select ID_POSSITION From position Where X = \"" + x + "\" And Y = \"" + y + "\" And world = \"" + world.ToString() + "\"", mysqlconnection);
            using (var reader = commandID_Position.ExecuteReader()) while (reader.Read()) resultPossition.Add(reader.GetString(0));
            List<string> resultDirection = new List<string>();
            var commandID_Direction = new MySqlCommand("Select ID_DIRECTION From direction Where LIBELLERDIRECTION = \"" + direction + "\"", mysqlconnection);
            using (var reader = commandID_Direction.ExecuteReader()) while (reader.Read()) resultDirection.Add(reader.GetString(0));
            List<string> resultID_CLIC = new List<string>();
            var commandID_CLIC = new MySqlCommand("Select XCLIC, YCLIC From cartespe Inner Join clic On clic.id_clic = cartespe.id_clic Where ID_POSSITION = \"" + resultPossition.ElementAt(0) + "\" and ID_direction = \"" + resultDirection + "\"", mysqlconnection);
            using (var reader = commandID_CLIC.ExecuteReader()) while (reader.Read()) resultID_CLIC.Add(reader.GetString(0));
            if (resultID_CLIC.Count() > 1) if (int.TryParse(resultID_CLIC.ElementAt(0), out int clicX) && int.TryParse(resultID_CLIC.ElementAt(1), out int clicY)) points = new Point(clicX, clicY);
            return points;
        }

        /// ✔ permet d'ajouter une carte special au ficher
        public static void AjouteCarteSpe(int x, int y, string direction, MySqlConnection mysqlconnection)
        {
            Dialog dialogue = new Dialog();
            dialogue.ShowDialog();
            var commandClic = new MySqlCommand("INSERT INTO clic (, XCLIC, YCLIC) VALUES ('" + dialogue.textBox1.Text + "','" + dialogue.textBox2.Text + "')", mysqlconnection);
            commandClic.ExecuteNonQuery();
            var commandID_Clic = new MySqlCommand("Select ID_CLIC From clic Where XCLIC = \"" + dialogue.textBox1.Text + "\" And YCLIC = \"" + dialogue.textBox2.Text + "\"", mysqlconnection);
            List<string> resultClic = new List<string>();
            using (var reader = commandID_Clic.ExecuteReader()) while (reader.Read()) resultClic.Add(reader.GetString(0));
            var commandID_Possition = new MySqlCommand("Select ID_POSSITION From position Where X = \"" + x + "\" And Y = \"" + y + "\"", mysqlconnection);
            List<string> resultPositon = new List<string>();
            using (var reader = commandID_Possition.ExecuteReader()) while (reader.Read()) resultPositon.Add(reader.GetString(0));
            var command = new MySqlCommand("INSERT INTO catespe (ID_DIRECTION, ID_POSSITION, ID_CLIC) VALUES ('" + direction + "','" + resultPositon.ElementAt(0) + "','" + resultClic.ElementAt(0) + "')", mysqlconnection);
            command.ExecuteNonQuery();
        }

        /// ✔ permet de trouver un phorreur sur la maps en cour
        public static bool Phorreur(MySqlConnection mysqlconnection)
        {
            var command = new MySqlCommand("Select color From colorphorreur", mysqlconnection);
            List<string> result = new List<string>();
            using (var reader = command.ExecuteReader()) while (reader.Read()) result.Add(reader.GetString(0));
            foreach (var CP in result)
            {
                Color ColorPhorreur = ColorTranslator.FromHtml("#" + CP);
                Point trouver = Utilitaires.PixelSearch(new Rectangle(350, 40, 1223, 872), ColorPhorreur, 1);
                if (trouver.X != -1)
                {
                    Thread.Sleep(50);
                    Cursor.Position = new Point(trouver.X, trouver.Y);
                    Thread.Sleep(200);
                    Color verifCP = Utilitaires.GetPixel(trouver);
                    if (ColorPhorreur.R != verifCP.R && ColorPhorreur.B != verifCP.B && ColorPhorreur.G != verifCP.G)
                    {
                        var commandNom = new MySqlCommand("Select color From colorphorreurnom", mysqlconnection);
                        List<string> resultNom = new List<string>();
                        using (var reader = commandNom.ExecuteReader()) while (reader.Read()) resultNom.Add(reader.GetString(0));
                        foreach (var CPN in resultNom)
                        {
                            Color ColorPhorreurNom = ColorTranslator.FromHtml("#" + CPN);
                            Point verifCPN = Utilitaires.PixelSearch(new Rectangle(trouver.X - 100, trouver.Y - 90, trouver.X + 200, trouver.Y + 140), ColorPhorreurNom, 0);
                            if (verifCPN.X != -1) return true;
                        }
                    }
                }
            }
            return false;
        }

        /// ✔ permet de savoir si l'indice est deja present dans la base de donner SQL
        public static bool IndiceEnregistrerPressent(string numindice, string x, string y, MySqlConnection mysqlconnection)
        {
            var command = new MySqlCommand("Select * From indiceenregistre Inner Join indicetrie On indicetrie.id_indicetrier = indiceenregistre.id_indicetrier Inner Join position On position.id_possition = indiceenregistre.id_possition Where NUMEROINDICE = \"" + numindice + "\" And  X = \"" + x + "\" And Y = \"" + y + "\"", mysqlconnection);
            using (var reader = command.ExecuteReader()) while (reader.Read()) return true;
            return false;
        }

        /// ✔ permet d'ajout un indice a la base de donner SQL
        public static void IndiceEnregistrerAdd(string numindice, string x, string y, int world, MySqlConnection mysqlconnection)
        {
            List<string> result = new List<string>();
            var commandID_Possition = new MySqlCommand("Select ID_POSSITION From position Where X = \"" + x + "\" And Y = \"" + y + "\" And world = \"" + world.ToString() + "\"", mysqlconnection);
            using (var reader = commandID_Possition.ExecuteReader()) while (reader.Read()) result.Add(reader.GetString(0));
            var command = new MySqlCommand("INSERT INTO indiceenregistre (ID_INDICETRIER, ID_POSSITION) VALUES ('" + numindice + "','" + result.ElementAt(0) + "')", mysqlconnection);
            command.ExecuteNonQuery();
        }
        #endregion


    }
}