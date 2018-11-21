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
            (1928, 1765),
            (1878, 1805),
            (1933, 1557),
            (2042, 1708),
            (2207, 1692),
            (2198, 1486),
            (1897, 2323),
            (1933, 2156),
            (1855, 2112),
            (1891, 2162),
            (1782, 2211),
            (2033, 2001),
            (2109, 1647),
            (1668, 1737),
            (1973, 1637),
            (1735, 1867),
            (2234, 1599),
            (1973, 1637),
            (1433, 3097),
            (1506, 3300),
            (1548, 3017),
            (1763, 3130),
            (2283, 1771),
            (2283, 1771),
            (1608, 3278),
            (1573, 3112),
            (1645, 3070),
            (1565, 3175),
            (1670, 1958),
            (2346, 1953),
            (1690, 2457),
            (1871, 1843),
            (1655, 1914),
            (1433, 3097),
            (1433, 3097),
        };

        private const int CircleRadius = 5;
        static void Main(string[] args)
        {
            var img = new Bitmap(4100, 4100);
            var g = Graphics.FromImage(img);
            g.FillRectangle(Brushes.White, 0, 0, img.Width, img.Height);

            var colorIndex = 0;

            var font = new Font("Arial", 24);

            foreach (var coordinate in coordinates)
            {
                var pen = new Pen(Color.Blue, 5);
                var (x, y) = coordinate;
                y = img.Height - y;
                g.DrawEllipse(pen, x - CircleRadius / 2, y - CircleRadius / 2, CircleRadius, CircleRadius);
                g.DrawString($"{x},{y}", font, pen.Brush, x, y);

            }

            g.Save();
            img.Save("drawing.gif", ImageFormat.Gif);
        }
    }
}
