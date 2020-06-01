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
    class GroundStructureCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.GroundStructure";

        public GroundStructureCapsule()
            : base(CommandName, Resources.GroundStructureText, Resources.GroundImage, Resources.GroundStructureHint)
        {
        }

        protected override void OnUpdate(Command command)
        {
            command.IsEnabled = SpaceClaim.Api.V19.Window.ActiveWindow != null;
        }


        protected override void OnExecute(Command command, ExecutionContext context, Rectangle buttonRect)
        {
            //MessageBox.Show($"Not yet");
        }
    }
}