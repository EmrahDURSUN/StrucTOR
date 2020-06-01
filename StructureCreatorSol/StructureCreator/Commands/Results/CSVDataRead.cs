using System.IO;
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using StructureCreator.Properties;
using SpaceClaim.Api.V19.Extensibility;
using System.Runtime.Remoting.Lifetime;
using System.Linq;
using System.Globalization;

namespace StructureCreator
{/// <summary>
/// Return all beams of csv file in collection 
/// </summary>
    class CsvDataRead
    {
        public static List<PointLocation> ReadData()
        {
            //string filePath = @"D:\BarData.txt";
            string filePath = Settings.Default.csv3Path;

            List<PointLocation> points = new List<PointLocation>();

            List<string> lines = File.ReadAllLines(filePath).ToList();

            foreach (var lined in lines)
            {
                string[] entries = lined.Split(';');

                PointLocation newPointLocation = new PointLocation
                {
                    xPoint = double.Parse(entries[0], new CultureInfo("de-DE")),
                    yPoint = double.Parse(entries[1], new CultureInfo("de-DE")),
                    zPoint = double.Parse(entries[2], new CultureInfo("de-DE")),
                    x2Point = double.Parse(entries[3], new CultureInfo("de-DE")),
                    y2Point = double.Parse(entries[4], new CultureInfo("de-DE")),
                    z2Point = double.Parse(entries[5], new CultureInfo("de-DE")),
                    diameter = double.Parse(entries[6], new CultureInfo("de-DE")),
                    force = double.Parse(entries[7], new CultureInfo("de-DE"))
                };

                points.Add(newPointLocation);

            }

            return points;

        }

    }
}