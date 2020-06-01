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
using StructureCreator.UI_extensions;


namespace StructureCreator
{
    class CreateStrucTorCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.CreateStrucTor";

        public CreateStrucTorCapsule()
            : base(CommandName,Resources.StrucTorText,Resources.StrucTorImage,Resources.StrucTorHint)
        {

        }
        protected override void OnUpdate(Command command)
        {
            command.IsEnabled = SpaceClaim.Api.V19.Window.ActiveWindow != null;
        }
        protected override void OnExecute(Command command, ExecutionContext context, Rectangle buttonRect)
        {
            Window window = Window.ActiveWindow;
            CylinderPtPt(window.Document.MainPart);
            window.InteractionMode = InteractionMode.Solid;

        }

        public static DesignBody CylinderPtPt(Part part)
        {

            Point point1 = Point.Create(0.00, 0.01, 0.00);
            Point point2 = Point.Create(0.03, 0.05, 0.04);
            double diameter = 0.004;
            double radi = diameter / 2;

            Vector heightVector = point2 - point1;
            Frame frame = Frame.Create(point1, heightVector.Direction);
            Plane plane = Plane.Create(frame);


            Body cylinder4 = Body.ExtrudeProfile(new CircleProfile(plane, radi), heightVector.Magnitude);
            return DesignBody.Create(part, "Cylinder3", cylinder4);


        }

    }
}
