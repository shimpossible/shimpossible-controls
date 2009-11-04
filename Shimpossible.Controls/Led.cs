using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Shimpossible.Controls
{
    public class LED : Control
    {
        int innerWidth;
        int innerHeight;
        Color offColor;
        bool val = true;

        public LED() : base()
        {
            ForeColor = Color.Lime;
            offColor = ControlPaint.Dark(ForeColor);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        [System.ComponentModel.DefaultValue(typeof(Color), "Lime")]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                offColor = ControlPaint.Dark(ForeColor);
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            int offsetX = (int)Math.Min(10, Math.Max(2, this.Width * 0.05));
            int offsetY = (int)Math.Min(10,Math.Max(2, this.Height * 0.05));
            innerWidth = this.Width - offsetX*2;
            innerHeight = this.Height - offsetY*2;

            Graphics g = e.Graphics;

            Color col1 = ControlPaint.LightLight(this.BackColor);
            Color col2 = ControlPaint.DarkDark(this.BackColor);
            Brush brush = new LinearGradientBrush(Point.Empty, new Point(0, this.Height), col2, col1);

            // background shadding
            Rectangle rec = this.ClientRectangle;
            g.FillEllipse(brush, rec);

            GraphicsPath gp = new GraphicsPath();

            // 10% increase in size
            Rectangle tmp = new Rectangle(offsetX,offsetY,innerWidth, innerHeight);
            tmp.Inflate(innerWidth / 10, innerHeight/10);
            gp.AddEllipse(tmp);

            PathGradientBrush brush2 = new PathGradientBrush(gp);
            brush2.CenterPoint = new Point((innerWidth+offsetX)/2+1, innerHeight * 3 / 4);
            brush2.CenterColor = Value?this.ForeColor:this.offColor;

            brush2.SurroundColors = new Color[] { Color.Black };
            brush2.FocusScales = new PointF(0.2f, 0.2f);
            brush2.WrapMode = WrapMode.TileFlipXY;

            rec = new Rectangle(offsetX, offsetY, innerWidth, innerHeight);            
            g.FillEllipse(brush2, rec);


            LinearGradientBrush highLight = new LinearGradientBrush(
                new Point(0, innerHeight / 25 + offsetY),
                new Point(0, innerHeight / 25 + offsetY + innerHeight * 4 / 10),
                Color.FromArgb(200, Color.White), Color.Transparent);

            g.FillEllipse(highLight, 
                new Rectangle(
                    innerWidth / 5 + offsetX, innerHeight / 25 + offsetY, 
                    innerWidth*6/10, innerHeight*4/10)
                );
            base.OnPaint(e);
        }

        [System.ComponentModel.DefaultValue(true)]
        public bool Value
        {
            get { return val; }
            set { val = value;

            this.Invalidate();
            } 
        }
    }
}
