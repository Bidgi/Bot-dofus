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
    public partial class Dialog : Form
    {
        public Dialog() { InitializeComponent(); }

        private void BtnValider_Click(object sender, EventArgs e)
        {
            textBox1.Text = MousePosition.X.ToString();
            textBox2.Text = MousePosition.Y.ToString();
            this.Close();
        }
    }
}
