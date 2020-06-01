using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using StructureCreator.Properties;
using SpaceClaim.Api.V19.Extensibility;
using SpaceClaim.Api.V19.Geometry;
using SpaceClaim.Api.V19.Modeler;
using Point = SpaceClaim.Api.V19.Geometry.Point;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using SpaceClaim.Api.V19;

namespace StructureCreator
{
    class CylinderPPCapsule : CommandCapsule
    {

        public const string CommandName = "ConstructorAddIn.C#.V19.CylinderPP";

        public CylinderPPCapsule()
            : base(CommandName, Resources.CylinderPPText, Resources.CreatePPCylinder, Resources.CylinderPPHint)
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
            CylinderPtPt(window.Document.MainPart);
            window.InteractionMode = InteractionMode.Solid;
        }

        /// <summary>
        /// Purpose = Creating plane using point and line
        /// </summary>
        /// <param name="part2"></param>
        /// <returns></returns>
        public static DesignBody CylinderPtPt(Part part2)
        {
            
            Point point1 = Point.Create(0.00, 0.00, 0.00);
            Point point2 = Point.Create(0.02, 0.03, 0.03);
            double diameter = 0.004;
            double radi = diameter / 2;

            Vector heightVector = point2 - point1;
            Frame frame = Frame.Create(point1, heightVector.Direction);
            Plane plane = Plane.Create(frame);


            Body cylinder4 = Body.ExtrudeProfile(new CircleProfile(plane, radi), heightVector.Magnitude);
            return DesignBody.Create(part2, "Cylinder3", cylinder4);
        }
    }
}