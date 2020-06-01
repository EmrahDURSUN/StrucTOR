/*
 * Purpose = Draw a bar between two point
 * challenge = creating plane
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using StructureCreator.Properties;
using SpaceClaim.Api.V19.Extensibility;
using SpaceClaim.Api.V19.Geometry;
using SpaceClaim.Api.V19.Modeler;
using SpaceClaim.Api.V19;
using Point = SpaceClaim.Api.V19.Geometry.Point;
using System.Windows.Forms;
using System.IO;
using System.Linq;

namespace StructureCreator
{
    class CreatePPCylinderCapsule : CommandCapsule
    {

        public const string CommandName = "ConstructorAddIn.C#.V19.CreatePPBar";

        public CreatePPCylinderCapsule()
            : base(CommandName, Resources.CreatePPBarText, Resources.CreatePPBarImage, Resources.CreatePPBarHint)
        {
        }

        protected override void OnUpdate(Command command)
        {
            // When a command is disabled, all UI components associated with the command are also disabled.
            command.IsEnabled = Window.ActiveWindow != null;
        }

        protected override void OnExecute(Command command, ExecutionContext context, Rectangle buttonRect)
        {
            Window window = Window.ActiveWindow; 
            //CreatePPBar(window.Document.MainPart);
           // CreateLine(window.Document.MainPart);
            CreateLines(window.Document.MainPart);
            window.InteractionMode = InteractionMode.Solid; 

        }


        //static Plane CreatePlane(Point origin, Direction normal)
        //{
        //    return Plane.Create(Frame.Create(origin, normal));
        //}

        static DesignBody CreatePPBar(Part part)  {

            Window window = Window.ActiveWindow;
            Debug.Assert(part != null, "part 1= null");

            double barRadius = 0.002;
            double barLenght = 0.020;

            Point pointStart = Point.Create(0, 0.03, 0.07);
            Point pointEnd = Point.Create(0.02, 0.04, 0.09);

            DesignCurve.Create(part, CurveSegment.Create(pointStart, pointEnd));

            //Plane plane = CreatePlane(pointStart, normal); 
            //DesignCurve line = DesignCurve.Create();
            //Direction direction = Direction.Equals(CurveSegment);
            //Frame frame = Frame.Create(pointStart);
            //Plane plane = Plane.Create(frame);


            Body bar = Body.ExtrudeProfile(new CircleProfile(Plane.PlaneYZ, barRadius), barLenght);
            return DesignBody.Create(part, "Cylinder", bar);
        }

        static void CreateLine(Part line)
        {
            Window window = Window.ActiveWindow;
            Point pointStart = Point.Create(0.10, 0.04, 0.07);
            Point pointEnd = Point.Create(0.02, 0.11, 0.09);
            DesignCurve.Create(line, CurveSegment.Create(pointStart, pointEnd));
            MessageBox.Show($"Line between {pointStart} and {pointEnd} will be created", "Info");
        }

        static void CreateLines(Part multiLine)
        {
            
            Window window = Window.ActiveWindow;

            List<PointTarget> points = ImportingFun.LoadSampleData();

            foreach (var PointTarget in points)
            {
                //MessageBox.Show($"Line between {points[0]} and will be created");
                DesignCurve.Create(multiLine, CurveSegment.Create(Point.Create(PointTarget.xPoint, PointTarget.yPoint, PointTarget.zPoint), Point.Create(PointTarget.x2Point, PointTarget.y2Point, PointTarget.z2Point)));

            }

        }
    }
}
