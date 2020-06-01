///*
// * Sample CommandCapsule for the SpaceClaim API
// */
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Drawing;
//using StructureCreator.Properties;
//using SpaceClaim.Api.V19.Extensibility;
//using SpaceClaim.Api.V19.Geometry;
//using SpaceClaim.Api.V19.Modeler;
//using Point = SpaceClaim.Api.V19.Geometry.Point;
//using System.Windows.Forms;
//using System.IO;
//using System.Linq;
//using SpaceClaim.Api.V19;
//using StructureCreator.UI_extensions;

//namespace StructureCreator
//{
//    class DevelopCreateStructureCapsule : CommandCapsule
//    {
//        public const string CommandName = "ConstructorAddIn.C#.V19.DcreateStruc";

//        public DevelopCreateStructureCapsule()
//            : base(CommandName, Resources.DCreateStructureText, Resources.CreateStructureImage, Resources.DCreateStructureHint)
//        {
//        }

//        protected override void OnInitialize(Command command)
//        {
//            // If your command doesn't modify the document/model, uncomment the following line
//            // to avoid creating an undo step.
//            //command.IsWriteBlock = false;
//        }

//        protected override void OnUpdate(Command command)
//        {
//            command.IsEnabled = Window.ActiveWindow != null;
//        }

//        protected override void OnExecute(Command command, ExecutionContext context, Rectangle buttonRect)
//        {
//            Window window = Window.ActiveWindow;
//            CreateStab(window.Document.MainPart);
//            window.InteractionMode = InteractionMode.Solid;
//        }

//        static void CreateStab(Part part)
//        {
//            Window window = Window.ActiveWindow;
//            int id = 0;
//            List<PointLocation2> pointLocations = CsvDataRead2.ReadData();

//            foreach (var location in pointLocations)
//            {

//                Point startPoint = Point.Create(location.xPoint, location.yPoint, location.zPoint);
//                Point endPoint = Point.Create(location.x2Point, location.y2Point, location.z2Point);
//                if (location.diameter != 0)
//                {

//                    double diameter = location.diameter;
//                    double radi = diameter / 50;
//                    Vector heightVector = endPoint - startPoint;
//                    Frame frame = Frame.Create(startPoint, heightVector.Direction);
//                    Plane plane = Plane.Create(frame);
//                    Body cylinder4 = Body.ExtrudeProfile(new CircleProfile(plane, radi), heightVector.Magnitude);
//                    DesignBody.Create(part, $"Cylinder{id}", cylinder4);
//                    id += 1;
//                }

//            }
//        }
//    }


//    class CsvDataRead2
//    {
//        public static List<PointLocation2> ReadData()
//        {
//            //string filePath = @"D:\BarData.txt";

//            Settings settings = Settings.Default;
//            string path = settings.csv3Path;

//            string filePath = "C:\\Users\\hendr\\Desktop\\TestNeu_csv3_28112019.csv";

//            List<PointLocation2> points = new List<PointLocation2>();

//            List<string> lines = File.ReadAllLines(path).ToList();

//            foreach (var lined in lines)
//            {
//                string[] entries = lined.Split(';');

//                PointLocation2 newPointLocation = new PointLocation2
//                {
//                    xPoint = double.Parse(entries[0]),
//                    yPoint = double.Parse(entries[1]),
//                    zPoint = double.Parse(entries[2]),
//                    x2Point = double.Parse(entries[3]),
//                    y2Point = double.Parse(entries[4]),
//                    z2Point = double.Parse(entries[5]),
//                    diameter = double.Parse(entries[6]),
//                    force = double.Parse(entries[7])
//                };

//                points.Add(newPointLocation);

//            }

//            return points;

//        }

//    }


//    public class PointLocation2
//    {
//        public double xPoint { get; set; }

//        public double yPoint { get; set; }

//        public double zPoint { get; set; }

//        public double x2Point { get; set; }

//        public double y2Point { get; set; }

//        public double z2Point { get; set; }

//        public double diameter { get; set; }

//        public double force { get; set; }

//    }
//}
