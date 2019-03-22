using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using IRuettae.Core.Models;

namespace IRuettae.ResultFixer
{
    public class ResultDrawer
    {
        private static readonly Color[] Colors = new[]
        {
            Color.FromArgb(230, 25, 75),
            Color.FromArgb(60, 180, 75),
            Color.FromArgb(255, 225, 25),
            Color.FromArgb(0, 130, 200),
            Color.FromArgb(245, 130, 48),
            Color.FromArgb(145, 30, 180),
            Color.FromArgb(70, 240, 240),
            Color.FromArgb(240, 50, 230),
            Color.FromArgb(210, 245, 60),
            Color.FromArgb(0, 128, 128),
            Color.FromArgb(170, 110, 40),
            Color.FromArgb(128, 0, 0),
            Color.FromArgb(170, 255, 195),
            Color.FromArgb(128, 128, 0),
            Color.FromArgb(0, 0, 128),
            Color.FromArgb(0, 0, 0)
        };

        private const int CircleRadius = 10;

        public static void SvgResult(string path, OptimizationResult result, (int x, int y)[] coordinates)
        {
            var svg = new XElement("svg", new XAttribute("viewBox", "0 0 4000 4000"));
            var colorIndex = 0;
            var routeGroup = 0;
            foreach (var route in result.NonEmptyRoutes)
            {
                routeGroup++;
                //var color = ColorTranslator.ToHtml(Colors[colorIndex]);
                var g = new XElement("g", new XAttribute("class", $"grp-{colorIndex}"), new XAttribute("data-group", routeGroup));
                colorIndex = (colorIndex + 1) % Colors.Length;
                int? lastX = null;
                int? lastY = null;
                
                foreach (var waypoint in route.Waypoints.OrderBy(wp => wp.StartTime))
                {
                    var (x, y) = coordinates[waypoint.VisitId == -1 ? 0 : waypoint.VisitId + 1];
                    y = 4000 - y;
                    g.Add(SvgCircle(x, y, waypoint.VisitId == -1 ? "Depot" : $""));

                    if (lastX.HasValue)
                    {
                        g.Add(SvgLine(x, y, lastX.Value, lastY.Value));
                    }

                    lastX = x;
                    lastY = y;
                }
                svg.Add(g);
            }
            File.WriteAllText(path, svg.ToString());

        }

        private static XElement SvgCircle(int x, int y, string hoverText)
        {
            var circle = new XElement("circle");
            circle.Add(new XAttribute("cx", x));
            circle.Add(new XAttribute("cy", y));
            circle.Add(new XAttribute("r", 30));
            circle.Add(new XAttribute("data-hover", hoverText));
            circle.Add(new XAttribute("class", "entry"));
            
            return circle;
        }

        private static XElement SvgLine(int x, int y, int x2, int y2)
        {
            var line = new XElement("line");
            line.Add(new XAttribute("x1", x));
            line.Add(new XAttribute("x2", x2));
            line.Add(new XAttribute("y1", y));
            line.Add(new XAttribute("y2", y2));
            line.Add(new XAttribute("path", y2));
            return line;
        }


        public static void DrawResult(string path, OptimizationResult result, (int x, int y)[] coordinates)
        {

            var img = new Bitmap(500, 500);
            var scaleFactor = img.Width / 4000d;
            var g = Graphics.FromImage(img);
            g.FillRectangle(Brushes.White, 0, 0, img.Width, img.Height);

            var colorIndex = 0;

            var font = new Font("Arial", 16);

            foreach (var route in result.NonEmptyRoutes)
            {
                var pen = new Pen(Colors[colorIndex], 2);
                colorIndex = (colorIndex + 1) % Colors.Length;
                int? lastX = null;
                int? lastY = null;
                foreach (var waypoint in route.Waypoints.OrderBy(wp => wp.StartTime))
                {
                    var (x, y) = coordinates[waypoint.VisitId == -1 ? 0 : waypoint.VisitId + 1];
                    x = (int)(x * scaleFactor);
                    y = (int)(y * scaleFactor);
                    y = img.Height - y;
                    g.FillEllipse(pen.Brush, x - CircleRadius / 2, y - CircleRadius / 2, CircleRadius, CircleRadius);
                    if (waypoint.VisitId == -1)
                    {
                        g.DrawString($"Startpunkt", font, pen.Brush, x, y);
                    }
                    //g.DrawString($"{waypoint.VisitId}", font, pen.Brush, x, y);
                    if (lastX.HasValue)
                    {
                        g.DrawLine(pen, x, y, lastX.Value, lastY.Value);
                    }

                    lastX = x;
                    lastY = y;
                }
            }

            g.Save();
            img.Save(path + ".gif", ImageFormat.Gif);

        }
    }
}
