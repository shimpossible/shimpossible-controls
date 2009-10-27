using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Shimpossible.Controls.Guage
{
    /// <summary>
    /// Factory class to return predefined paths
    /// </summary>
    public class PredefinedShapes
    {
        /// <summary>
        /// Triangle pointing to the left
        /// </summary>
        /// <param name="width">Height of triangle</param>
        /// <param name="height">Length of triangle</param>
        /// <returns></returns>
        static public GraphicsPath Needle(int width, int height)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLine(0, width, height, 0);
            path.AddLine(height, 0, 0, -width);
            path.AddLine(0, -width, 0, width);
            return path;

        }

        static public GraphicsPath Trapazoid(int width, int height)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLine(width * .5f, height * -.5f, width * .5f, height * .5f);
            path.AddLine(width * .5f, height * .5f, width * -.5f, height * .2f);
            path.AddLine(width * -.5f, height * .2f, width * -.5f, height * -.2f);
            path.AddLine(width * -.5f, height * -.2f, width * .5f, height * -.5f);
            return path;
        }

        static public GraphicsPath Rectangle(int width, int height)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLine(width * .5f, height * -.5f, width * .5f, height * .5f);
            path.AddLine(width * .5f, height * .5f, width * -.5f, height * .5f);
            path.AddLine(width * -.5f, height * .5f, width * -.5f, height * -.5f);
            path.AddLine(width * -.5f, height * -.5f, width * .5f, height * -.5f);

            return path;

        }

    }
}
