using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Shimpossible.Controls
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            this.circularGuage1.Value+=0.3;

            if (this.circularGuage1.Value > 100)
            {
                this.circularGuage1.Value = 0;
                this.circularGuage1.SweepAngle -= 10;
                if (this.circularGuage1.SweepAngle < 90)
                    this.circularGuage1.SweepAngle = 270;
            }
        }
    }
}
