/*
 * Old example, vreates 3 bars and one line. But all uses plane xyz, next we should create our plane
 */

using System.Diagnostics;
using System.Drawing;
using StructureCreator.Properties;
using SpaceClaim.Api.V19.Extensibility;
using SpaceClaim.Api.V19.Geometry;
using SpaceClaim.Api.V19.Modeler;
using Point = SpaceClaim.Api.V19.Geometry.Point;
using SpaceClaim.Api.V19;

namespace StructureCreator
{
    class CreateBarCapsule : CommandCapsule {

        /// <summary>
        /// That name must be equal to Ribbon XML file
        /// </summary>
        public const string CommandName = "ConstructorAddIn.C#.V19.CreateBar";

        public CreateBarCapsule()
            : base(CommandName, Resources.CreateBarText, Resources.CreateBarImage, Resources.CreateBarHint)
        {
        }

        protected override void OnUpdate(Command command)
        {
            command.IsEnabled = Window.ActiveWindow != null;
        }

        protected override void OnExecute(Command command, ExecutionContext context, Rectangle buttonRect)
        {
            Window window = Window.ActiveWindow;
            CreateBar(window.Document.MainPart);
            window.InteractionMode = InteractionMode.Solid;
        }

        public static DesignBody CreateBar(Part part)
        {
            Debug.Assert(part != null, "part != null");

            double barRadius = 0.002;
            double barLenght = 0.020;

            Point pointStart = Point.Create(0, 0, 0);
            Point pointEnd = Point.Create(0, 0.04, 0.05);
            DesignCurve.Create(part, CurveSegment.Create(pointStart, pointEnd));

            Body cylinder1 = Body.ExtrudeProfile(new CircleProfile(Plane.PlaneZX, barRadius), barLenght);
            DesignBody.Create(part, "Cylinder1", cylinder1);


            Body cylinder2 = Body.ExtrudeProfile(new CircleProfile(Plane.PlaneXY, barRadius), barLenght);
            DesignBody.Create(part, "Cylinder2", cylinder2);


            Body cylinder3 = Body.ExtrudeProfile(new CircleProfile(Plane.PlaneYZ, barRadius), barLenght);
            return DesignBody.Create(part, "Cylinder3", cylinder3);

        }

    }
}