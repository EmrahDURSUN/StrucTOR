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

namespace StructureCreator
{
    public class ImportingFun
    {
        public static List<PointTarget> LoadSampleData()
        {
            string filePath = @"D:\BarDatas.txt";

            List<PointTarget> points = new List<PointTarget>();

            List<string> lines = File.ReadAllLines(filePath).ToList();

            foreach (var lined in lines)
            {
                string[] entries = lined.Split(',');

                PointTarget newPointTarget = new PointTarget
                {
                    xPoint = double.Parse(entries[0]),
                    yPoint = double.Parse(entries[1]),
                    zPoint = double.Parse(entries[2]),
                    x2Point = double.Parse(entries[3]),
                    y2Point = double.Parse(entries[4]),
                    z2Point = double.Parse(entries[5])
                };

                points.Add(newPointTarget);

            }

            return points;

        }
    }
}
