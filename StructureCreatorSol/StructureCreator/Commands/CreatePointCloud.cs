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
    class CreatePointCloudCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.CreatePointCloud";

        public CreatePointCloudCapsule()
            : base(CommandName, Resources.PointCloudText, Resources.PointCloudImage, Resources.PointCloudHint)
        {
        }

        protected override void OnUpdate(Command command)
        {
            command.IsEnabled = SpaceClaim.Api.V19.Window.ActiveWindow != null;
        }


        protected override void OnExecute(Command command, ExecutionContext context, Rectangle buttonRect)
        {


            // These three line are responsibly for calling Windows Form => Our form name is PointsCalForm
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.Run(new PointsCalForm());
            // that line above means = Run that form wait until it finish then continue

            // TODO - take data from user after calculation of it save it to csv file
        }

        static void CreatePoints(Part part) {
            Window window = Window.ActiveWindow;

            List<PointTarget> points = ImportingFun.LoadSampleData();

            foreach (var PointTarget in points)
            {
                //MessageBox.Show($"Line between {points[0]} and will be created");
                Point.Create(PointTarget.xPoint, PointTarget.yPoint, PointTarget.zPoint);

            }
        }
    }
}