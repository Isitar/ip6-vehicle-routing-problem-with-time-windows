using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateDrawer
{
    class Program
    {
        private static readonly (int x, int y)[] coordinates = {
            (1392,1946),
            (1535,1302),
            (1402,1176),
            (2897,2658),
            (3208,2214),
            (2560,2692),
            (1599,2024),
            (733,773),
            (1028,1127),
            (1141,1547),
            (2325,2758),
            (956,1510),
            (2002,1142),
            (1205,1338),
            (2814,2446),
            (1273,590),
            (2237,941),
            (1638,964),
            (3098,2820),
            (2683,2190),
            (1092,2024)
        };

        private const int CircleRadius = 5;
        static void Main(string[] args)
        {
            var img = new Bitmap(4100, 4100);
            var g = Graphics.FromImage(img);
            g.FillRectangle(Brushes.White, 0, 0, img.Width, img.Height);

            var font = new Font("Arial", 24);

            foreach (var coordinate in coordinates)
            {
                var pen = new Pen(Color.Blue, 5);
                var (x, y) = coordinate;
                y = img.Height - y;
                g.FillEllipse(pen.Brush, x - CircleRadius / 2, y - CircleRadius / 2, CircleRadius, CircleRadius);
                g.DrawString($"{x},{y}", font, pen.Brush, x, y);

            }

            g.Save();
            img.Save("drawing.gif", ImageFormat.Gif);
        }
    }
}
