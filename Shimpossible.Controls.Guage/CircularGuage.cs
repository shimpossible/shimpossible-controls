using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
namespace Shimpossible.Controls.Guage
{    
    public class CircularGuage : Control
    {
        double value;

        public CircularGuage()
        {
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, true);
            this.BackColor = Color.Transparent;
        }

        public double Value
        {
            get { return value; }
            set 
            { 
                this.value = value;
                //this.Invalidate();
                if(Parent!=null)
                Parent.Invalidate(this.Bounds, true);
            }
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (value > 30) value = 0;
            double max = 30;
            double min = 0;
            double delta = max - min;

            float centerX = this.Width / 2;
            float centerY = this.Height / 2;
            float radius = Math.Min(centerX, centerY)-30;
            float startAngle = 135;
            float sweepAngle = 270;
            int ticks = 15;
            float step = sweepAngle / ticks;
            float currAngle = startAngle;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            

            // Clear the background color
            SolidBrush b = new SolidBrush(Color.FromArgb(64, Color.Gray));
            g.FillRectangle(b, new Rectangle(Point.Empty, this.Size));


            // translate to the CENTER
            g.TranslateTransform(centerX, centerY);
            Matrix centerMatrix = g.Transform;
            CircularAxis axis = new CircularAxis(radius);
            axis.OnPaint(e);
            // restore
            g.Transform = centerMatrix;

            // draw the pointer
            CircularPointer pointer = new CircularPointer(radius);
            pointer.Paint(g, value);

            base.OnPaint(e);
        }
    }

    /// <summary>
    /// The Needle
    /// </summary>
    public class CircularPointer
    {
        float sweepAngle = 270;
        float startAngle = 135;
        double min=0, max=30;
        float radius = 90;
        int width=10;

        int capWidth = 30;

        public CircularPointer(float r)
        {
            radius = r;
        }
        public void Paint(Graphics g, double value)
        {
            double valueRange = (max - min);
            
            GraphicsState state = g.Save();
            float vAngle = (float)(value * sweepAngle / valueRange);

            Brush shadow = new SolidBrush(Color.FromArgb(50, Color.Black));
            Brush capBrush =
                new LinearGradientBrush(
                    new PointF(-capWidth * 0.707f, -capWidth * 0.707f),
                    new PointF(capWidth * 0.707f, capWidth * 0.707f), Color.LightGray, Color.Black);

            Brush brush =
                new LinearGradientBrush(new PointF(0, width), new PointF(0, -width), Color.White, Color.Red);
            GraphicsPath path = new GraphicsPath();
            GraphicsPath capPath = new GraphicsPath();
            
            path.AddLine(0, width, radius, 0);
            path.AddLine(radius, 0, 0, -width);
            path.AddLine(0, -width, 0, width);

            capPath.AddArc(capWidth * -0.5f, capWidth * -0.5f, capWidth, capWidth, 0, 360);
            Region needleRegion = new Region(path);
            needleRegion.Union(capPath);


            // shadow
            Matrix old = g.Transform;
            // shadow offset
            g.TranslateTransform(2, 2);
            g.RotateTransform(startAngle + vAngle);
            g.FillRegion(shadow, needleRegion);
            // restore matrix
            g.Transform = old;

            g.RotateTransform(startAngle + vAngle);
            // draw needle
            g.FillPath(brush, path);            
            g.DrawPath(Pens.Black, path);

            g.Restore(state);
            // draw cap
            g.FillPath(capBrush, capPath);
            g.DrawPath(Pens.Black, capPath);
            //g.DrawLine(Pens.White, Point.Empty, new PointF(radius, 0));
        }
    }

    public class CircularTicks
    {
        float startAngle = 135, sweepAngle = 270;
        int width = 20;
        int height = 20;
        int ticks = 10;
        AxisAlign align = AxisAlign.Inside;        
        double min = 0, max = 30;
        float radius = 100;

        public float Radius
        {
            set { radius = value; }
        }
        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        public int Ticks
        {
            get { return ticks; }
            set { ticks = value; }
        }

        public void Paint(Graphics g)
        {
            double tickValueScale = (max - min) / ticks;
            double angleScale = sweepAngle / ticks;

            LinearGradientBrush tickBrush = new LinearGradientBrush(Point.Empty, new Point(0, width),
                Color.White, Color.LightGray);
            tickBrush.SetSigmaBellShape(.5f, 1);

            GraphicsPath tickPath = new GraphicsPath();
            tickPath.AddLine(width * .5f, height * -.5f, width * .5f, height * .5f);
            tickPath.AddLine(width * .5f, height * .5f, width * -.5f, height * .2f);
            tickPath.AddLine(width * -.5f, height * .2f, width * -.5f, height * -.2f);
            tickPath.AddLine(width * -.5f, height * -.2f, width * .5f, height * -.5f);

            Matrix startM = g.Transform;
            for (int i = 0; i <= ticks; i++)
            {
                float currAngle = (float)(i * angleScale + this.startAngle);

                // restore position
                g.Transform = startM;

                // move to position
                g.RotateTransform(currAngle);
                g.TranslateTransform(radius, 0);


                // draw major tick mark                
                g.FillPath(tickBrush, tickPath);
                g.DrawPath(new Pen(Color.FromArgb(128, 128, 128)), tickPath);
            }
        }
    }
    /// <summary>
    /// The major and minor tick marks
    /// </summary>
    public class CircularAxis
    {
        double min=0, max=30;
        float startAngle=135, sweepAngle=270;
        int ticks = 10;

        float radius = 100;
        int width = 4;
        Color color = Color.CornflowerBlue;

        Font font = new Font("Arial", 12, FontStyle.Bold);
        AxisAlign textAlign = AxisAlign.Inside;

        bool rotateText = false;

        
        CircularTicks major = new CircularTicks();
        CircularTicks minor = new CircularTicks();
        public CircularAxis(float r)
        {
            minor.Radius = major.Radius = radius = r;

            major.Height = 8;
            major.Width = 14;

            minor.Ticks = 50;
            minor.Width = 7;
            minor.Height = 5;
            
        }
        public void OnPaint(PaintEventArgs e)
        {
            double tickValueScale = (max - min) / ticks;
            double angleScale = sweepAngle / ticks;
            double textRadius = radius; // radius where text will show
            Graphics  g=e.Graphics;

            Pen p = new Pen(color, width);
            g.DrawArc(p, new RectangleF(- radius, - radius, 2*radius, 2*radius), startAngle, sweepAngle);


            Matrix pre = g.Transform;
            minor.Paint(e.Graphics);
            g.Transform = pre;
            major.Paint(e.Graphics);
            g.Transform = pre;

            Brush textBrush = new SolidBrush(Color.White);

            // save
            Matrix old = g.Transform;

            for (int i = 0; i <= ticks; i++)
            {
                float currAngle = (float)(i * angleScale + this.startAngle);
                string tickText = (tickValueScale * i + min).ToString();
                SizeF textSize = g.MeasureString(tickText, font);

                textRadius = radius +Math.Max(textSize.Width/2, textSize.Height/2);


                // restore
                g.Transform = old;

                //
                // rotate the text
                //
                float textAngle = currAngle + 90;
                if (textAngle > 180) textAngle -= 360;
                if (textAngle < -180) textAngle += 360;


                // move to position
                g.RotateTransform(currAngle);
                g.TranslateTransform(radius,0);


                // outside
                switch (textAlign)
                {
                    case AxisAlign.Outside:
                        g.TranslateTransform(+Math.Max(textSize.Width / 2, textSize.Height / 2), 0);
                        break;
                    case AxisAlign.Inside:
                        g.TranslateTransform(-Math.Max(textSize.Width / 2, textSize.Height / 2) - major.Height/2, 0);
                        break;
                    // default is cross
                }

                if (rotateText == false)
                {
                    g.RotateTransform(-currAngle);
                }

                // draw the text
                //g.DrawRectangle(Pens.Wheat, new Rectangle((int)-textSize.Width / 2, (int)-textSize.Height / 2, (int)textSize.Width, (int)textSize.Height));
                g.DrawString(tickText, font, textBrush, new PointF(-textSize.Width / 2,  -textSize.Height / 2));

            }
        }
    }


    public enum AxisAlign
    {
        Cross,
        Inside,
        Outside,
    }
}
