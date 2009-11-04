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
        double value=0;
        double min = 0, max = 100, range=100;
        float centerX, centerY;
        float radius = 100;

        float startAngle = 135, sweepAngle = 270;

        CircularPointer pointer;    // shows angle
        CircularAxis axis;          // draw ticks and circle

        public CircularGuage()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, true);

            GraphicsPath path = new GraphicsPath();
            path.AddArc(centerX-radius, centerY-radius, centerX+radius, centerY+radius, 0, 360);
            //this.Region = new Region(path);

            pointer = new CircularPointer(radius);            
            axis = new CircularAxis(radius);
            axis.Font = this.Font;
            axis.Align = AxisAlign.Inside;

            this.BackColor = SystemColors.ControlDark;
        }

        public CircularAxis Axis
        {
            get { return axis; }
            set
            {
                axis = value;
                // sync up the values
                axis.Font = this.Font;
                axis.Max = this.Max;
                axis.Min = this.Min;
                axis.Radius = this.Radius;
            }
        }
        public float StartAngle
        {
            get { return startAngle; }
            set { startAngle = value;
            axis.StartAngle = value;
            }
        }
        public float SweepAngle
        {
            get { return sweepAngle; }
            set { 
                sweepAngle = value;
                axis.SweepAngle = value;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            centerX = this.Width / 2;
            centerY = this.Height / 2;
            // for testing we use this value
            this.Radius = Math.Min(centerX, centerY) - 30;

            GraphicsPath path = new GraphicsPath();
            path.AddArc(centerX - radius, centerY - radius, radius*2, radius*2, 0, 360);
            //this.Region = new Region(path);

            this.Invalidate();
        }

        public float Radius
        {
            get { return radius; }
            set
            {
                radius = value;
                pointer.Radius = axis.Radius = this.Radius;
            }
        }
        public double Min
        {
            get { return min; }
            set { min = value;
            axis.Min = value;
            range = max - min;
            }
        }

        public double Max
        {
            get { return max; }
            set { max = value;
            axis.Max = value;
            range = max - min;
            }
        }

        private float CalcAngle(double value)
        {
            return (float)(value * sweepAngle / range) + startAngle;
        }

        public double Value
        {
            get { return value; }
            set 
            { 
                this.value = value;                
                this.Refresh();
            }
        }        

        
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                axis.Font = value;
            }
        }
        

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;            
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;

            // Clear the background color
            SolidBrush b = new SolidBrush(this.BackColor);
            g.FillRectangle(b, new Rectangle(Point.Empty, this.Size));
            
            // translate to the CENTER
            g.TranslateTransform(centerX, centerY);
            Matrix centerMatrix = g.Transform;
            
            // draw axis and tick marks
            axis.OnPaint(g);
            // restore
            g.Transform = centerMatrix;

            // draw the pointer/needle            
            pointer.Paint(g, CalcAngle(value) );

            //base.OnPaint(e);
        }
    }

    /// <summary>
    /// The Needle
    /// </summary>
    public class CircularPointer
    {
        int shadowOffset = 5;
        float radius = 90;
        int width=10;   // width of needle        
        int capWidth = 30;

        public CircularPointer(float r)
        {
            radius = r;
        }
        
        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        /// <summary>
        /// Pixel offset for shadow
        /// set to 0 to disable shadow
        /// </summary>
        public int ShadowOffset
        {
            get { return shadowOffset; }
            set { shadowOffset = value; }
        }
        /// <summary>
        /// Radius of Pointer Cap
        /// </summary>
        public int CapRadius
        {
            get { return capWidth; }
            set { capWidth = value; }
        }

        public void Paint(Graphics g, float angle)
        {
            Matrix centerMatrix = g.Transform;

            //double valueRange = (max - min);
            //float vAngle = (float)(value * sweepAngle / valueRange);

            Brush shadow = new SolidBrush(Color.FromArgb(50, Color.Black));
            Brush capBrush =
                new LinearGradientBrush(
                    new PointF(-capWidth * 0.707f, -capWidth * 0.707f),
                    new PointF(capWidth * 0.707f, capWidth * 0.707f), Color.LightGray, Color.Black);

            Brush brush =
                new LinearGradientBrush(new PointF(0, width), new PointF(0, -width), Color.White, Color.Red);
            GraphicsPath path = PredefinedShapes.Needle(width, (int)radius);
            GraphicsPath capPath = new GraphicsPath();
            
            
            capPath.AddArc(capWidth * -0.5f, capWidth * -0.5f, capWidth, capWidth, 0, 360);
            Region needleRegion = new Region(path);
            needleRegion.Union(capPath);


            // shadow
            Matrix old = g.Transform;
            // shadow offset
            g.TranslateTransform(shadowOffset, shadowOffset);
            g.RotateTransform(angle);
            g.FillRegion(shadow, needleRegion);
            // restore matrix
            g.Transform = old;

            g.RotateTransform(angle);
            // draw needle
            g.FillPath(brush, path);            
            g.DrawPath(Pens.Black, path);

            g.Transform = centerMatrix;
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
        Color color = Color.White;
        // shape to draw tick mark
        GraphicsPath tickPath;
        LinearGradientBrush tickBrush;

        public CircularTicks()
        {
            BuildTickPath();
        }

        public AxisAlign Align
        {
            get
            {
                return align;
            }
            set
            {
                align = value;
            }
        }
        public float StartAngle
        {
            get { return startAngle; }
            set
            {
                startAngle = value;
            }
        }
        public float SweepAngle
        {
            get { return sweepAngle; }
            set
            {
                sweepAngle = value;
            }
        }

        protected void BuildTickPath()
        {
            tickBrush = new LinearGradientBrush(Point.Empty, new Point(0, width),
                color, Color.FromArgb( color.R/2, color.G/2, color.B/2) );
            tickBrush.SetSigmaBellShape(.5f, 1);

            tickPath = PredefinedShapes.Rectangle(width, height);
        }
        public float Radius
        {
            set { radius = value; }
        }
        public int Height
        {
            get { return height; }
            set 
            { 
                height = value;
                BuildTickPath();
            }
        }
        public int Width
        {
            get { return width; }
            set 
            { 
                width = value;
                BuildTickPath();
            }
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

            Matrix startM = g.Transform;

            float actualRad = radius;
            switch (align)
            {
                case AxisAlign.Inside:
                    actualRad = radius - Width/2;
                    break;
                case AxisAlign.Outside:
                    actualRad = radius + Width/2;
                    break;                
            }

            for (int i = 0; i <= ticks; i++)
            {
                float currAngle = (float)(i * angleScale + this.startAngle);

                // restore position
                g.Transform = startM;

                // move to position
                g.RotateTransform(currAngle);
                g.TranslateTransform(actualRad, 0);


                // draw major tick mark                
                g.FillPath(tickBrush, tickPath);
                g.DrawPath(new Pen(Color.FromArgb(128, 128, 128)), tickPath);   // outline tick
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

        bool dirty = true;
        bool rotateText = false;
        Bitmap imageCache;
        
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

        public float StartAngle
        {
            get { return startAngle; }
            set { startAngle = value;
                
            dirty = true;
            }
        }
        public float SweepAngle
        {
            get { return sweepAngle; }
            set { 
                sweepAngle = value;
                major.SweepAngle = value;
                minor.SweepAngle = value;
            dirty = true;
            }
        }


        public double Max
        {
            get { return max; }
            set { max = value; }
        }
        public double Min
        {
            get { return min; }
            set { min = value; }
        }

        /// <summary>
        /// Where to draw the text
        /// </summary>
        public AxisAlign Align
        {
            get { return textAlign; }
            set { 
                textAlign = value;
                dirty = true;
            }
        }

        public float Radius
        {
            get {
                return radius;
            }
            set
            {
                minor.Radius = major.Radius = radius = value;
                dirty = true;
            }
        }

        /// <summary>
        /// How many major ticks to show.
        /// This is where text values show also
        /// </summary>
        public int MajorTicks
        {
            get { return major.Ticks; }
            set { 
                major.Ticks = value;
                dirty = true;
            }

        }

        /// <summary>
        /// How many minor ticks to display between each major
        /// </summary>
        public int MinorTicks
        {
            get { return minor.Ticks / major.Ticks; }
            set
            {
                minor.Ticks = major.Ticks * value;
                dirty = true;
            }
        }

        public Color AxisColor
        {
            get { return color; }
            set { 
                color = value;
                dirty = true;
            }

        }
        /// <summary>
        /// Rotate text so it always points out from center
        /// Text always points up if this is false
        /// </summary>
        public bool RotateText
        {
            get { return rotateText; }
            set { 
                rotateText = value;
                dirty = true;
            }

        }

        public Font Font
        {
            get { return font; }
            set
            {
                font = value;
                dirty = true;
            }
        }
        public void OnPaint(Graphics grfx)
        {
            if (dirty)
            {
                imageCache = new Bitmap((int)(radius * 2) + major.Width, (int)(radius * 2) + major.Width, grfx);
                using (Graphics g = Graphics.FromImage(imageCache))
                {
                    // dont set cleartype or fonts look messed up.. 
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.TranslateTransform(imageCache.Width * 0.5f, imageCache.Height * 0.5f);
                    BuildImage(g);
                }                
            }

            // draw the cached image
            grfx.DrawImage(imageCache, imageCache.Width * -0.5f, imageCache.Height * -0.5f);
        }

        /// <summary>
        /// Draws the Axis/scale to the selected Graphics object
        /// This is meant to cache the image to a bitmap
        /// for quicker display later
        /// </summary>
        /// <param name="g"></param>
        public void BuildImage(Graphics g)
        {
            double tickValueScale = (max - min) / ticks;
            double angleScale = sweepAngle / ticks;
            double textRadius = radius; // radius where text will show

            Pen p = new Pen(color, width);
            g.DrawArc(p, new RectangleF(-radius, -radius, 2*radius, 2*radius), startAngle, sweepAngle);


            Matrix pre = g.Transform;
            minor.Paint(g);
            g.Transform = pre;
            major.Paint(g);
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

                float shift = 0;
                // outside
                switch (textAlign)
                {
                    case AxisAlign.Cross:
                        shift = 0;
                        break;
                    case AxisAlign.Outside:
                        shift = Math.Max(textSize.Width / 2, textSize.Height / 2);
                        if (major.Align == AxisAlign.Cross)     shift += major.Width / 2;                        
                        if (major.Align == AxisAlign.Outside)   shift += major.Width;                        
                        break;
                    case AxisAlign.Inside:
                        shift = -Math.Max(textSize.Width / 2, textSize.Height / 2);
                        if (major.Align == AxisAlign.Cross) shift -= major.Width / 2;
                        if (major.Align == AxisAlign.Inside) shift -= major.Width;
                        break;
                }

                g.TranslateTransform(shift, 0);

                if (rotateText == false)
                {
                    g.RotateTransform(-currAngle);
                }

                // draw the text
                //g.DrawRectangle(Pens.Wheat, new Rectangle((int)-textSize.Width / 2, (int)-textSize.Height / 2, (int)textSize.Width, (int)textSize.Height));
                g.DrawString(tickText, font, textBrush, new PointF(-textSize.Width / 2,  -textSize.Height / 2));

            }

            dirty = false;
        }

        
    }


    public enum AxisAlign
    {
        Cross,
        Inside,
        Outside,
    }
}
