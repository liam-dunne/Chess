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
            Label checkMateBox = new Label();
            checkMateBox.Text = "Checkmate!";
            checkMateBox.TextAlign = ContentAlignment.MiddleCenter;
            checkMateBox.Font = new Font("Comic Sans MS", 72);
            checkMateBox.ForeColor = Color.White;
            checkMateBox.Dock = DockStyle.Fill;
            checkMateBox.Size = new Size(400, 200);
            checkMateBox.Location = new Point((Width / 2 - checkMateBox.Width / 2), (Height / 2 - checkMateBox.Height / 2));
            System.Diagnostics.Debug.WriteLine(checkMateBox.Location);
            System.Diagnostics.Debug.WriteLine(Size);
            Controls.Add(checkMateBox);
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
