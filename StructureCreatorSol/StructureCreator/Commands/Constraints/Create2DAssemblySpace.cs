/*
 * Sample CommandCapsule for the SpaceClaim API
 */
using System.Drawing;
using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Extensibility;
using StructureCreator.Properties;
using System.Windows.Forms;
using System.Collections.Generic;
using SpaceClaim.Api.V19.Geometry;
using System.Diagnostics;
using SpaceClaim.Api.V19.Modeler;
using StructureCreator.UI_extensions.ConstraintUI;
using System;

namespace StructureCreator
{
    // Creates an assembly space for 2D Face
    class AssemblySpace2DCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.AssemblySpace2D";

        public AssemblySpace2DCapsule()
            : base(CommandName, Resources.StructuralConstraints2DText, Resources.StructuralConstraintsImage, Resources.StructuralConstraints2DHint)
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

        protected override void OnExecute(Command command, ExecutionContext context, Rectangle buttonRect)
        {
            Settings set = Settings.Default; 

            if (Window.ActiveWindow.ActiveContext.SingleSelection is DesignFace)
            {
                // Reset all necessary parameteres
                Settings.Default.Dimension = 1;  // Save that 2D Dimension is chosen 
                Settings.Default.forceCount = 0; 
                Settings.Default.forces = "";
                Settings.Default.bearingLoadCount = 0;
                Settings.Default.bearingLoads = "";
                Settings.Default.xLength = 0;
                Settings.Default.yLength = 0;
                Settings.Default.zLength = 0;
                Settings.Default.Save();

                Form form = new CreateCrossSectionForm();

                form.StartPosition = FormStartPosition.Manual;

                Screen screen = Screen.PrimaryScreen;
                Rectangle bounds = screen.Bounds;

                // Show form in left bottom corner
                form.Location = (new System.Drawing.Point(bounds.X + (int)(bounds.Width * 0.05), bounds.Height - 258 - (int)(bounds.Height * 0.1))); // 523 = form Width, 258 Form Height


                DialogResult dr = form.ShowDialog();
                // Show testDialog as a modal dialog and determine if DialogResult = OK.

                if (dr == DialogResult.OK)
                {
                    try
                    {
                        int count = 1;  // node number
                        double distance = set.distance; // point distance
                        bool teilbar = true;   

                        // Values to find origin of face
                        double Xursprung = double.MaxValue; 
                        double Zursprung = double.MaxValue;

                        double Xmax = double.MinValue;
                        double Zmax = double.MinValue;


                        double length = 0;
                        double width = 0;

                        IDocObject face = Window.ActiveWindow.ActiveContext.SingleSelection;

                        IDesignFace designFace = (IDesignFace)face;


                        foreach (IDesignEdge e in designFace.Edges)
                        {
                            SpaceClaim.Api.V19.Geometry.Point p1 = e.Shape.StartPoint;
                            SpaceClaim.Api.V19.Geometry.Point p2 = e.Shape.EndPoint;

                            // Locate minimum point of plane 
                            if (p1.X < Xursprung)
                            {
                                Xursprung = p1.X;
                            }
                            if (p2.X < Xursprung)
                            {
                                Xursprung = p2.X;
                            }

                            if (p1.Z < Zursprung)
                            {
                                Zursprung = p1.Z;
                            }
                            if (p2.Z < Zursprung)
                            {
                                Zursprung = p2.Z;
                            }

                            // Locate max value of x and z
                            if (p1.X > Xmax)
                            {
                                Xmax = p1.X;
                            }
                            if (p2.X > Xmax)
                            {
                                Xmax = p2.X;
                            }


                            if (p1.Z > Zmax)
                            {
                                Zmax = p1.Z;
                            }
                            if (p2.Z > Zmax)
                            {
                                Zmax = p2.Z;
                            }



                            // Check if distance is a factor of length, height or width 
                            if ((e.Shape.Length * 1000) % distance > -0.001 && (e.Shape.Length * 1000) % distance < 0.001)
                            {

                            }
                            else
                            {
                                teilbar = false;
                            }

                        }


                        if (teilbar)
                        {
                            // Create all points in block

                            for (double z = Zursprung; z <= Zmax + 0.001; z += distance / 1000)
                            {
                                for (double x = Xursprung; x <= Xmax + 0.001; x += distance / 1000)
                                {
                                    SpaceClaim.Api.V19.Geometry.Point point = SpaceClaim.Api.V19.Geometry.Point.Create(x, 0, z);
                                    DatumPoint P1 = DatumPoint.Create(Window.ActiveWindow.Document.MainPart, "P" + count, point);

                                    count++;
                                }

                            }

                            // save length
                            set.xLength = (Xmax - Xursprung) * 1000;
                            set.zLength = (Zmax - Zursprung) * 1000;

                            set.Dimension = 1;

                            set.Save();

                            createCSV2D(decimal.Parse("" + set.xLength), decimal.Parse("" + set.zLength), decimal.Parse("" + distance), set.csv2Path);

                            // Set camera position
                            Window.ActiveWindow.SetProjection(Frame.Create(SpaceClaim.Api.V19.Geometry.Point.Origin, -Direction.DirY), 0.1);
                        }
                        else
                        {
                            MessageBox.Show("Distance is not a factor of length, height or width of face!", "Info");

                            Settings.Default.CrossSecFormOpened = false;
                            Settings.Default.Save();
                        }

                    }
                    catch (Exception es)
                    {
                        MessageBox.Show("");
                    }
                }
                else if (dr == DialogResult.Cancel)
                {
                    //MessageBox.Show("Canceled!");
                }

            }
            else
            {
                MessageBox.Show("Select a face in document first!", "Info");
            }
        }

        // Create csv file with all points
        static void createCSV2D(decimal width, decimal depth, decimal distance, String path)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(path))
            {
                int point = 1;

                for (decimal z = 0; z <= depth; z += distance) //int z = 0; z < depth; z+=distance
                {
                    for (decimal x = 0; x <= width; x += distance)
                    {
                        file.WriteLine("P" + point + ";" + x + ";" + 0 + ";" + z);
                        point++;
                    }
                }
                
            }
        }
    }
}
