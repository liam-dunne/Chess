using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Official_Chess_Actual
{
    internal class Checkmate
    {
        public static void CheckMate()
        {           
            Form1.ActiveForm.Hide();
            Form2 form2 = new Form2();
            form2.Show();
        }
    }
}
