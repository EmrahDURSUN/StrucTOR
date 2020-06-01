/*
 * Sample CommandCapsule for the SpaceClaim API
 */
using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Extensibility;
using StructureCreator.Properties;
using SpaceClaim.Api.V19.Geometry;
using System.Windows.Forms;
using SpaceClaim.Api.V19.Modeler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.IO;

namespace StructureCreator
{
    /// <summary>
    /// Creates Spheres in lattice structure at common points of at least to beams. 
    /// </summary>
    class PostprocessingCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.Postprocessing";

        public PostprocessingCapsule()
            : base(CommandName, Resources.LoadCaseAttributesText, null, Resources.LoadCaseAttributesHint)
        {
        }

        protected override void OnInitialize(Command command)
        {
            // If your command doesn't modify the document/model, uncomment the following line
            // to avoid creating an undo step.
            //command.IsWriteBlock = false;
        }

        protected override void OnUpdate(Command command)
        {
        }


        protected override void OnExecute(Command command, ExecutionContext context, System.Drawing.Rectangle buttonRect)
        {
            Part mainPart = Window.ActiveWindow.Document.MainPart;
            Window.ActiveWindow.InteractionMode = InteractionMode.Solid;
            Window.ActiveWindow.ZoomSelection();


            try
            {
                // Get Sphere multiplier
                Settings set = Settings.Default;
                double multiplier = set.sphereMultiplier;


                // actual Sphere creator

                List<SpherePoint> points = getSpherePoints();   // Calculate sphere points

                createSpheres(points, multiplier); // Create sphere points

            }
            catch (Exception es)
            {
                MessageBox.Show(es.ToString(), "Info");
            }
        }



        public List<SpherePoint> getSpherePoints()
        {
            try
            {
                List<StabCurve> curves = new List<StabCurve>();

                int index = 0;

                foreach (Beam t in Window.ActiveWindow.Document.MainPart.Beams) // Get all Beams of active document
                {
                    // Save every beam in window in a collection

                    ITrimmedCurve c = t.Shape;

                    Point start = Point.Create(c.StartPoint.X, c.StartPoint.Y, c.StartPoint.Z); // Startpoint of beam
                    Point end = Point.Create(c.EndPoint.X, c.EndPoint.Y, c.EndPoint.Z); // Endpoint of beam 

                    Vector v = new Vector();
                    v = end - start; // Direction vector of beam

                    curves.Add(new StabCurve(index, c.StartPoint.X, c.StartPoint.Y, c.StartPoint.Z, c.EndPoint.X, c.EndPoint.Y, c.EndPoint.Z, v.X, v.Y, v.Z));

                    index++;
                }

                // Get diameter of csv file
                // Compare first 6 Elements of csv file with beam of windows to get the right diameter

                Settings set = Settings.Default;

                using (var reader = new StreamReader(set.csv3Path))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');

                        foreach (StabCurve stabCurve in curves)
                        {
                            //result += "(X1:" + stabCurve.x1 + ",Y1:" + stabCurve.y1 + ",Z1:" + stabCurve.z1 + ",X2:" + stabCurve.x2 + ",Y2:" + stabCurve.y2 + ",Z2:" + stabCurve.z2 + ");";

                            if (stabCurve.x1.Equals(Double.Parse(values[0]) / 1000) && stabCurve.y1.Equals(Double.Parse(values[1]) / 1000) && stabCurve.z1.Equals(Double.Parse(values[2]) / 1000) && stabCurve.x2.Equals(Double.Parse(values[3]) / 1000) && stabCurve.y2.Equals(Double.Parse(values[4]) / 1000) && stabCurve.z2.Equals(Double.Parse(values[5]) / 1000))
                            {
                                stabCurve.diameter = (Double.Parse(values[6]));
                                stabCurve.diameter = stabCurve.diameter / 1000;

                            }
                        }
                    }
                }


                // Calculate angles between beams and decide wether to create a sphere or not

                List<SpherePoint> spherePoints = new List<SpherePoint>();


                foreach (StabCurve stabCurve in curves)
                {
                    foreach (StabCurve stabCurve2 in curves)
                    {
                        if (stabCurve.index != stabCurve2.index) // make sure different beams are selected
                        {

                            if (stabCurve.x1 == stabCurve2.x1 && stabCurve.y1 == stabCurve2.y1 && stabCurve.z1 == stabCurve2.z1) // startpoints of beams are equal
                            {
                                Point curve1Start = Point.Create(stabCurve.x1, stabCurve.y1, stabCurve.z1);
                                Point curve2Start = Point.Create(stabCurve2.x1, stabCurve2.y1, stabCurve2.z1);
                                Point curve1End = Point.Create(stabCurve.x2, stabCurve.y2, stabCurve.z2);
                                Point curve2End = Point.Create(stabCurve2.x2, stabCurve2.y2, stabCurve2.z2);

                                Vector vec1 = curve1End - curve1Start;
                                Vector vec2 = curve2End - curve2Start;

                                double innerProduct = getInnerProduct(vec1, vec2); // Get inner product of two vectors to get the angle between selected beams
                                double angle = getAngle(innerProduct); // Get angle in degree

                                // if angle is not 180° nothing is done. Otherwise a sphere is created at the common point of beams. The radius is multiple of bigger radius of beams

                                if (angle != 180)
                                {
                                    // Check if equal point is already found
                                    bool alreadyFound = false;
                                    foreach (SpherePoint p in spherePoints)
                                    {
                                        if (p.X == stabCurve.x1 && p.Y == stabCurve.y1 && p.Z == stabCurve.z1)
                                        {
                                            alreadyFound = true;


                                            // Check, if there is a bigger radius than the saved one
                                            if (p.radius < (stabCurve.diameter / 2))
                                            {
                                                p.radius = (stabCurve.diameter / 2);
                                            }
                                            if (p.radius < (stabCurve2.diameter / 2))
                                            {
                                                p.radius = (stabCurve2.diameter / 2);
                                            }
                                        }
                                    }

                                    if (!alreadyFound)
                                    {
                                        // Get bigger radius of beams
                                        double radius = 0;
                                        if (stabCurve.diameter > stabCurve2.diameter)
                                        {
                                            radius = stabCurve.diameter / 2;
                                        }
                                        else if (stabCurve.diameter < stabCurve2.diameter)
                                        {
                                            radius = stabCurve2.diameter / 2;
                                        }
                                        else if (stabCurve.diameter == stabCurve2.diameter)
                                        {
                                            radius = stabCurve.diameter / 2;
                                        }
                                        SpherePoint tempPoint = new SpherePoint(stabCurve.x1, stabCurve.y1, stabCurve.z1, radius);
                                        spherePoints.Add(tempPoint);

                                    }
                                }

                            }
                            else if (stabCurve.x1 == stabCurve2.x2 && stabCurve.y1 == stabCurve2.y2 && stabCurve.z1 == stabCurve2.z2) // startpoint of beam1 and endpoint of beam2 are equal
                            {
                                Point curve1Start = Point.Create(stabCurve.x1, stabCurve.y1, stabCurve.z1);
                                Point curve2Start = Point.Create(stabCurve2.x1, stabCurve2.y1, stabCurve2.z1);
                                Point curve1End = Point.Create(stabCurve.x2, stabCurve.y2, stabCurve.z2);
                                Point curve2End = Point.Create(stabCurve2.x2, stabCurve2.y2, stabCurve2.z2);

                                Vector vec1 = curve1End - curve1Start;
                                Vector vec2 = curve2Start - curve2End;

                                double innerProduct = getInnerProduct(vec1, vec2); // Get inner product of two vectors to get the angle between selected beams
                                double angle = getAngle(innerProduct); // Get angle in degree

                                // if angle is not 180° nothing is done. Otherwise a sphere is created at the common point of beams. The radius is multiple of bigger radius of beams

                                if (angle != 180)
                                {
                                    // Check if equal point is already found
                                    bool alreadyFound = false;
                                    foreach (SpherePoint p in spherePoints)
                                    {
                                        if (p.X == stabCurve.x1 && p.Y == stabCurve.y1 && p.Z == stabCurve.z1)
                                        {
                                            alreadyFound = true;

                                            //result1 += "(R:" + p.radius + ",S1:" + (stabCurve.diameter / 2) + ",S2:" + (stabCurve2.diameter / 2) + ");";

                                            // Check, if there is a bigger radius than the saved one
                                            if (p.radius < (stabCurve.diameter / 2))
                                            {
                                                p.radius = (stabCurve.diameter / 2);
                                            }
                                            if (p.radius < (stabCurve2.diameter / 2))
                                            {
                                                p.radius = (stabCurve2.diameter / 2);
                                            }
                                        }
                                    }

                                    if (!alreadyFound)
                                    {
                                        // Get bigger radius of beams
                                        double radius = 0;
                                        if (stabCurve.diameter > stabCurve2.diameter)
                                        {
                                            radius = stabCurve.diameter / 2;
                                        }
                                        else if (stabCurve.diameter < stabCurve2.diameter)
                                        {
                                            radius = stabCurve2.diameter / 2;
                                        }
                                        else if (stabCurve.diameter == stabCurve2.diameter)
                                        {
                                            radius = stabCurve.diameter / 2;
                                        }
                                        SpherePoint tempPoint = new SpherePoint(stabCurve.x1, stabCurve.y1, stabCurve.z1, radius);
                                        spherePoints.Add(tempPoint);
                                    }
                                }

                            }
                            else if (stabCurve.x2 == stabCurve2.x1 && stabCurve.y2 == stabCurve2.y1 && stabCurve.z2 == stabCurve2.z1) // startpoint of beam2 and endpoint of beam1 are equal
                            {
                                Point curve1Start = Point.Create(stabCurve.x1, stabCurve.y1, stabCurve.z1);
                                Point curve2Start = Point.Create(stabCurve2.x1, stabCurve2.y1, stabCurve2.z1);
                                Point curve1End = Point.Create(stabCurve.x2, stabCurve.y2, stabCurve.z2);
                                Point curve2End = Point.Create(stabCurve2.x2, stabCurve2.y2, stabCurve2.z2);

                                Vector vec1 = curve1Start - curve1End;
                                Vector vec2 = curve2End - curve2Start;

                                double innerProduct = getInnerProduct(vec1, vec2); // Get inner product of two vectors to get the angle between selected beams
                                double angle = getAngle(innerProduct); // Get angle in degree

                                // if angle is not 180° nothing is done. Otherwise a sphere is created at the common point of beams. The radius is multiple of bigger radius of beams

                                if (angle != 180)
                                {
                                    // Check if equal point is already found
                                    bool alreadyFound = false;
                                    foreach (SpherePoint p in spherePoints)
                                    {
                                        if (p.X == stabCurve.x2 && p.Y == stabCurve.y2 && p.Z == stabCurve.z2)
                                        {
                                            alreadyFound = true;

                                            //result1 += "(R:" + p.radius + ",S1:" + (stabCurve.diameter / 2) + ",S2:" + (stabCurve2.diameter / 2) + ");";

                                            // Check, if there is a bigger radius than the saved one
                                            if (p.radius < (stabCurve.diameter / 2))
                                            {
                                                p.radius = (stabCurve.diameter / 2);
                                            }
                                            if (p.radius < (stabCurve2.diameter / 2))
                                            {
                                                p.radius = (stabCurve2.diameter / 2);
                                            }
                                        }
                                    }

                                    if (!alreadyFound)
                                    {
                                        // Get bigger radius of beams
                                        double radius = 0;
                                        if (stabCurve.diameter > stabCurve2.diameter)
                                        {
                                            radius = stabCurve.diameter / 2;
                                        }
                                        else if (stabCurve.diameter < stabCurve2.diameter)
                                        {
                                            radius = stabCurve2.diameter / 2;
                                        }
                                        else if (stabCurve.diameter == stabCurve2.diameter)
                                        {
                                            radius = stabCurve.diameter / 2;
                                        }
                                        SpherePoint tempPoint = new SpherePoint(stabCurve.x2, stabCurve.y2, stabCurve.z2, radius);
                                        spherePoints.Add(tempPoint);
                                    }
                                }

                            }
                            else if (stabCurve.x2 == stabCurve2.x2 && stabCurve.y2 == stabCurve2.y2 && stabCurve.z2 == stabCurve2.z2) // endpoints of beams are equal
                            {
                                Point curve1Start = Point.Create(stabCurve.x1, stabCurve.y1, stabCurve.z1);
                                Point curve2Start = Point.Create(stabCurve2.x1, stabCurve2.y1, stabCurve2.z1);
                                Point curve1End = Point.Create(stabCurve.x2, stabCurve.y2, stabCurve.z2);
                                Point curve2End = Point.Create(stabCurve2.x2, stabCurve2.y2, stabCurve2.z2);

                                Vector vec1 = curve1Start - curve1End;
                                Vector vec2 = curve2Start - curve2End;

                                double innerProduct = getInnerProduct(vec1, vec2); // Get inner product of two vectors to get the angle between selected beams
                                double angle = getAngle(innerProduct); // Get angle in degree

                                // if angle is not 180° nothing is done. Otherwise a sphere is created at the common point of beams. The radius is multiple of bigger radius of beams

                                if (angle != 180)
                                {
                                    // Check if equal point is already found
                                    bool alreadyFound = false;
                                    foreach (SpherePoint p in spherePoints)
                                    {
                                        if (p.X == stabCurve.x2 && p.Y == stabCurve.y2 && p.Z == stabCurve.z2)
                                        {
                                            alreadyFound = true;
                                            //result1 += "(R:" + p.radius + ",S1:" + (stabCurve.diameter / 2) + ",S2:" + (stabCurve2.diameter / 2) + ");";


                                            // Check, if there is a bigger radius than the saved one
                                            if (p.radius < (stabCurve.diameter / 2))
                                            {
                                                p.radius = (stabCurve.diameter / 2);
                                            }
                                            if (p.radius < (stabCurve2.diameter / 2))
                                            {
                                                p.radius = (stabCurve2.diameter / 2);
                                            }
                                        }
                                    }

                                    if (!alreadyFound)
                                    {
                                        // Get bigger radius of beams
                                        double radius = 0;
                                        if (stabCurve.diameter > stabCurve2.diameter)
                                        {
                                            radius = stabCurve.diameter / 2;
                                        }
                                        else if (stabCurve.diameter < stabCurve2.diameter)
                                        {
                                            radius = stabCurve2.diameter / 2;
                                        }
                                        else if (stabCurve.diameter == stabCurve2.diameter)
                                        {
                                            radius = stabCurve.diameter / 2;
                                        }
                                        SpherePoint tempPoint = new SpherePoint(stabCurve.x2, stabCurve.y2, stabCurve.z2, radius);
                                        spherePoints.Add(tempPoint);
                                    }
                                }

                            }
                        }
                    }
                }

                return spherePoints;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Choose an appropriate csv3 file!", "Info");
            }

            return null;

        }

        // Creates all Spheres on the points found in method getSpherePoints
        public void createSpheres(List<SpherePoint> points, double multiplier)
        {

            int count = 1;
            foreach (SpherePoint point in points)
            {
                Direction dir = Direction.Create(1, 1, 0);
                Frame frame = Frame.Create(Point.Create(point.X, point.Y, point.Z), dir);

                //result += point.radius + ", ";

                Sphere sphere = Sphere.Create(frame, (point.radius * multiplier));

                BoxUV box = new BoxUV();

                Body body1 = Body.CreateSurfaceBody(sphere, box);

                DesignBody designBody = DesignBody.Create(Window.ActiveWindow.Document.MainPart, "Sphere" + count, body1);

                count++;
            }

        }


        public double getAngle(double innerPro)
        {
            return Math.Round(innerPro * (180.0 / Math.PI)); // Round to nearest integer value
        }

        public double getInnerProduct(Vector v1, Vector v2)
        {
            // Calculate the inner product
            double vectorProduct = ((v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z));

            double vectorLength1 = Math.Sqrt(Math.Pow(v1.X, 2) + Math.Pow(v1.Y, 2) + Math.Pow(v1.Z, 2));
            double vectorLength2 = Math.Sqrt(Math.Pow(v2.X, 2) + Math.Pow(v2.Y, 2) + Math.Pow(v2.Z, 2));

            return Math.Acos(vectorProduct / (vectorLength1 * vectorLength2)); // Inner product formula
        }

    }

    // Represents a Beam 
    public class StabCurve
    {
        public int index { get; set; }
        public double x1 { get; set; }
        public double y1 { get; set; }
        public double z1 { get; set; }
        public double x2 { get; set; }
        public double y2 { get; set; }
        public double z2 { get; set; }
        public double directionX { get; set; }
        public double directionY { get; set; }
        public double directionZ { get; set; }
        public double diameter { get; set; }


        public StabCurve(int _index, double _x1, double _y1, double _z1, double _x2, double _y2, double _z2, double _dirX, double _dirY, double _dirZ)
        {
            index = _index;
            x1 = _x1;
            y1 = _y1;
            z1 = _z1;
            x2 = _x2;
            y2 = _y2;
            z2 = _z2;
            directionX = _dirX;
            directionX = _dirY;
            directionX = _dirZ;
            diameter = -1;
        }
    }

    // represents a sphere location with the sphere radius
    public class SpherePoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double radius { get; set; }


        public SpherePoint(double _x, double _y, double _z, double _radius)
        {
            X = _x;
            Y = _y;
            Z = _z;
            radius = _radius;
        }
    }
}
