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
    class AssemblySpaceCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.AssemblySpace";

        public AssemblySpaceCapsule()
            : base(CommandName, Resources.StructuralConstraintsText, Resources.StructuralConstraintsImage, Resources.StructuralConstraintsHint)
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
            if(Window.ActiveWindow.ActiveContext.SingleSelection is IDesignBody)
            {
                // reset all necessary parameters
                Settings.Default.Dimension = 0;  // Save that 3D Dimension is chosen 
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
                    form.Location = (new System.Drawing.Point(bounds.X + (int)(bounds.Width*0.05), bounds.Height - 258 - (int)(bounds.Height*0.1))); // 523 = form Width, 258 Form Height


                    DialogResult dr = form.ShowDialog();
                    // Show testDialog as a modal dialog and determine if DialogResult = OK.

                    Settings set = Settings.Default;
                    double distance = set.distance;

                    if (dr == DialogResult.OK)
                    {
                        //MessageBox.Show("OK, distance : "+ distance);



                        // Create points with selected distance
                        bool teilbar = true;
                        int count = 1;  // point number
                        
                        // parameters to find origin of body
                        double Xursprung = double.MaxValue;
                        double Yursprung = double.MaxValue;
                        double Zursprung = double.MaxValue;

                        double Xmax = double.MinValue;
                        double Ymax = double.MinValue;
                        double Zmax = double.MinValue;


                        Part mainPart = Window.ActiveWindow.Document.MainPart;
                        Window.ActiveWindow.InteractionMode = InteractionMode.Solid;
                        Window.ActiveWindow.ZoomSelection();

                    //var allBodies = new List<IDesignBody>();
                    //GatherBodies(mainPart, allBodies); // gets all bodies in window

                            IDocObject Ibody = Window.ActiveWindow.ActiveContext.SingleSelection;

                            IDesignBody designBody = (IDesignBody)Ibody;


                            DesignBody body = designBody.Master;
                            body.Name = "DesignSpace";
                            body.Style = BodyStyle.Transparent;  // Make Block transparent
                            body.SetVisibility(null, false);


                            foreach (IDesignFace b in designBody.Faces)
                            {

                                foreach (IDesignEdge e in b.Edges)
                                {

                                    SpaceClaim.Api.V19.Geometry.Point p1 = e.Shape.StartPoint;
                                    SpaceClaim.Api.V19.Geometry.Point p2 = e.Shape.EndPoint;

                                    // Locate minimum point of block 
                                    if (p1.X < Xursprung)
                                    {
                                        Xursprung = p1.X;
                                    }
                                    if (p2.X < Xursprung)
                                    {
                                        Xursprung = p2.X;
                                    }

                                    if (p1.Y < Yursprung)
                                    {
                                        Yursprung = p1.Y;
                                    }
                                    if (p2.Y < Yursprung)
                                    {
                                        Yursprung = p2.Y;
                                    }

                                    if (p1.Z < Zursprung)
                                    {
                                        Zursprung = p1.Z;
                                    }
                                    if (p2.Z < Zursprung)
                                    {
                                        Zursprung = p2.Z;
                                    }

                                    // Locate max value of x, y and z
                                    if (p1.X > Xmax)
                                    {
                                        Xmax = p1.X;
                                    }
                                    if (p2.X > Xmax)
                                    {
                                        Xmax = p2.X;
                                    }

                                    if (p1.Y > Ymax)
                                    {
                                        Ymax = p1.Y;
                                    }
                                    if (p2.Y > Ymax)
                                    {
                                        Ymax = p2.Y;
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
                            
                        }

                        if (teilbar)
                        {
                            // Create all points in block

                            for (double z = Zursprung; z <= Zmax + 0.001; z += distance / 1000)
                            {
                                for (double y = Yursprung; y <= Ymax + 0.001; y += distance / 1000)
                                {
                                    for (double x = Xursprung; x <= Xmax + 0.001; x += distance / 1000)
                                    {
                                        SpaceClaim.Api.V19.Geometry.Point point = SpaceClaim.Api.V19.Geometry.Point.Create(x, y, z);
                                        DatumPoint P1 = DatumPoint.Create(Window.ActiveWindow.Document.MainPart, "P" + count, point);

                                        count++;
                                    }
                                }
                            }

                            // save length
                            set.xLength = (Xmax - Xursprung) * 1000;
                            set.yLength = (Ymax - Yursprung) * 1000;
                            set.zLength = (Zmax - Zursprung) * 1000;

                            

                            set.Save();


                            createCSV2(decimal.Parse("" + set.xLength), decimal.Parse("" + set.yLength), decimal.Parse("" + set.zLength), decimal.Parse("" + set.distance), set.csv2Path);

                            // Set camera position
                            Window.ActiveWindow.SetProjection(Frame.Create(SpaceClaim.Api.V19.Geometry.Point.Origin, -Direction.DirZ), 0.1);
                        }
                        else
                        {
                            MessageBox.Show("Distance is not a factor of length, height or width of block!", "Info");

                            body.Name = "DesignSpace";
                            body.SetVisibility(null, true);

                            Settings.Default.CrossSecFormOpened = false;
                            Settings.Default.Save();
                        }
                    }
                    else if (dr == DialogResult.Cancel)
                    {
                        //MessageBox.Show("Canceled!");
                    }

                    Settings.Default.CrossSecFormOpened = true;
                    Settings.Default.Save();
                }
            else
            {
                MessageBox.Show("Select a body in document first!", "Info");
            }
        }
        


        static void GatherBodies(IPart part, List<IDesignBody> allBodies)
        {
            allBodies.AddRange(part.Bodies);

            foreach (IComponent comp in part.Components)
                GatherBodies(comp.Content, allBodies);
        }

        // Create csv file with all points
        static void createCSV2(decimal width, decimal heigth, decimal depth, decimal distance, String path)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(path))
            {
                int point = 1;

                for (decimal z = 0; z <= depth; z += distance) //int z = 0; z < depth; z+=distance
                {
                    for (decimal y = 0; y <= heigth; y += distance)
                    {
                        for (decimal x = 0; x <= width; x += distance)
                        {
                            file.WriteLine("P" + point + ";" + x + ";" + y + ";" + z);
                            point++;
                        }
                    }
                }
            }
        }
    }
}
