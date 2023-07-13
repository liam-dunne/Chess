using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Official_Chess_Actual
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            BackColor = Color.FromArgb(41, 44, 51);
            TextBox checkMateBox = new TextBox();
            checkMateBox.Text = "Checkmate!";
            checkMateBox.TextAlign = HorizontalAlignment.Center;
            checkMateBox.Location = new Point((this.Width / 2 - checkMateBox.Width / 2), (this.Height / 2 - checkMateBox.Height));
            System.Diagnostics.Debug.WriteLine(checkMateBox.Location);
            System.Diagnostics.Debug.WriteLine(this.Size);
            Controls.Add(checkMateBox);
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
