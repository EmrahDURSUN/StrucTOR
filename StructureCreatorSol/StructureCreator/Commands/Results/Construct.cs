/*
 * Sample CommandCapsule for the SpaceClaim API
 */
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
using StructureCreator.UI_extensions.ResultsUI;
using System.Globalization;

namespace StructureCreator
{
    /// <summary>
    /// Construct beams of csv3 file.
    /// </summary>
    class ConstructCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.ConstructCapsule";

        public ConstructCapsule()
            : base(CommandName, null, null, null)
        {

        }

        protected override void OnUpdate(Command command)
        {
            // base.OnUpdate(command);
            command.IsEnabled = Window.ActiveWindow != null;
        }

        protected override void OnExecute(Command command, ExecutionContext context, Rectangle buttonRect)
        {
            // base.OnExecute(command, context, buttonRect);
            Window window = Window.ActiveWindow;
            MultiBeamCreator(window.Document.MainPart);
            //window.InteractionMode = InteractionMode.Solid;
            //window.ZoomSelection();
        }

        
        static void MultiBeamCreator(Part part)
        {
            try
            {
                //Document doc = Document.GetDocument(Settings.Default.ProjectPath + "/" + Settings.Default.ProjectName + ".scdoc");
                Document doc = Window.ActiveWindow.Document;
                Part p = doc.MainPart;

                int id = 0;
                List<PointLocation> pointLocations = CsvDataRead.ReadData();  // Get all beams out of file

                foreach (var location in pointLocations)
                {
                    Point startPoint = Point.Create(Double.Parse( "" + (location.xPoint / 1000), new CultureInfo("de-DE")), Double.Parse("" + (location.yPoint / 1000), new CultureInfo("de-DE")), Double.Parse("" + (location.zPoint / 1000), new CultureInfo("de-DE")));
                    Point endPoint = Point.Create(Double.Parse("" + (location.x2Point / 1000), new CultureInfo("de-DE")), Double.Parse("" + (location.y2Point / 1000), new CultureInfo("de-DE")), Double.Parse("" + (location.z2Point / 1000), new CultureInfo("de-DE")));
                    if (location.diameter != 0)
                    {
                        try
                        {
                            double diameter = location.diameter;
                            double radi = Double.Parse("" + (location.diameter / 2000), new CultureInfo("de-DE"));

                            var lineSegment = CurveSegment.Create(startPoint, endPoint);
                            var designLine = DesignCurve.Create(p, lineSegment);

                            Vector heightVector = endPoint - startPoint;
                            Frame frame = Frame.Create(startPoint, heightVector.Direction);
                            Plane plane = Plane.Create(frame);
                            var profi = new CircleProfile(plane, radi);

                            //var doc = DocumentHelper.GetActiveDocument();
                            //Document doc = Document.GetDocument(Path.GetDirectoryName("Document1"));
                            //Document doc = window.ActiveContext.Context.Document;


                            Part beamProfile = Part.CreateBeamProfile(doc, "Beam" + id, profi);

                            Beam.Create(beamProfile, designLine);
                            id += 1;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), "Info");
                        }

                    }
                }
                
            }
            catch (Exception exe)
            {
                MessageBox.Show(exe.ToString(), "Info");
            }

        }

    }
}
