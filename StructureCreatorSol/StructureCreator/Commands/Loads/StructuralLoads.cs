/*
 * Sample CommandCapsule for the SpaceClaim API
 */
using System.Drawing;
using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Extensibility;
using StructureCreator.Properties;
using System.Windows.Forms;
using StructureCreator.UI_extensions.LoadsUI;
using System;
using SpaceClaim.Api.V19.Geometry;
using SpaceClaim.Api.V19.Modeler;

namespace StructureCreator
{
    /// <summary>
    /// Creates structural loads on selected point. An arrow is created on point
    /// </summary>
    class StructuralLoadsCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.structuralLoads";

        public StructuralLoadsCapsule()
            : base(CommandName, Resources.StructuralLoadsText, Resources.StructuralLoadsImage, Resources.StructuralLoadsHint)
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
            Window window = Window.ActiveWindow;
        }

        protected override void OnExecute(Command command, ExecutionContext context, Rectangle buttonRect)
        {
            try
            {
                Settings set = Settings.Default;
                // Get Coordinates of selected point
                String coord = set.activePointCoord;
                String[] coords = coord.Split(';');

                double distance = set.distance;

                // Save force in settings
                set.forces += "(" + set.activePointName + "," + (Double.Parse(coords[0]) * 1000).ToString() + "," + (Double.Parse(coords[1]) * 1000).ToString() + "," + (Double.Parse(coords[2]) * 1000).ToString() + "," + set.pointForceX + "," + set.pointForceY + "," + set.pointForceZ + ");";
                set.forceCount = set.forceCount + 1;


                if(set.Dimension == 0) // 3D
                {
                    // Check if force is positive or negative for arrow direction
                    bool sign = true;
                    if (set.pointForceY < 0)
                    {
                        sign = false;
                    }
                    else if (set.pointForceY > 0)
                    {
                        sign = true;
                    }

                    
                    Plane pathPlane = null;
                    if (sign)
                    {
                        pathPlane = Plane.Create(Frame.Create(SpaceClaim.Api.V19.Geometry.Point.Create(Double.Parse(coords[0]), Double.Parse(coords[1]), Double.Parse(coords[2])), -Direction.DirY, Direction.DirX)); // Direction of axis need to be set to give the arrow the right direction
                    }
                    else if (!sign)
                    {
                        pathPlane = Plane.Create(Frame.Create(SpaceClaim.Api.V19.Geometry.Point.Create(Double.Parse(coords[0]), (Double.Parse(coords[1]) + ((distance / 1000) - ((distance / 1000) * 0.2))), Double.Parse(coords[2])), Direction.DirY, Direction.DirX)); // Direction of axis need to be set to give the arrow the right direction
                                                                                                                                                                                                                                                                         //MessageBox.Show("" + ( Double.Parse(coords[1]) + ((distance/1000) - ((distance/1000) * 0.2))));
                    }


                    //var profile = new ArrowProfile(pathPlane, 0.0005, 0.00025, 0.0005, 0.00008, 0.0);
                    var profile = new ArrowProfile(pathPlane, (distance / 1000) * 0.4, (distance / 1000) * 0.2, (distance / 1000) * 0.4, (distance / 1000) * 0.1, 0.0);


                    Body body = Body.ExtrudeProfile(profile, 0.0002);

                    // Create arrow
                    DesignBody.Create(Window.ActiveWindow.Document.MainPart, "ExternalForce" + set.activePointName, body);

                    // Set color of arrow
                    foreach (DesignBody b in Window.ActiveWindow.Document.MainPart.Bodies)
                    {
                        if (b.Name.Equals("ExternalForce" + set.activePointName))
                        {
                            b.SetColor(null, System.Drawing.Color.Red);
                        }
                    }
                }else if(set.Dimension == 1) // 2D
                {
                    // Check if force is positive or negative for arrow direction
                    bool sign = true;
                    if (set.pointForceZ < 0)
                    {
                        sign = false;
                    }
                    else if (set.pointForceZ > 0)
                    {
                        sign = true;
                    }


                    Plane pathPlane = null;
                    if (sign)
                    {
                        pathPlane = Plane.Create(Frame.Create(SpaceClaim.Api.V19.Geometry.Point.Create(Double.Parse(coords[0]), Double.Parse(coords[1]), Double.Parse(coords[2]) + ((distance / 1000) - ((distance / 1000) * 0.2))), Direction.DirZ, Direction.DirX)); // Direction of axis need to be set to give the arrow the right direction
                    }
                    else if (!sign)
                    {
                        pathPlane = Plane.Create(Frame.Create(SpaceClaim.Api.V19.Geometry.Point.Create(Double.Parse(coords[0]), Double.Parse(coords[1]), Double.Parse(coords[2])/* + ((distance / 1000) - ((distance / 1000) * 0.2))*/), -Direction.DirZ, Direction.DirX)); // Direction of axis need to be set to give the arrow the right direction
                                                                                                                                                                                                                                                                         //MessageBox.Show("" + ( Double.Parse(coords[1]) + ((distance/1000) - ((distance/1000) * 0.2))));
                    }


                    //var profile = new ArrowProfile(pathPlane, 0.0005, 0.00025, 0.0005, 0.00008, 0.0);
                    var profile = new ArrowProfile(pathPlane, (distance / 1000) * 0.4, (distance / 1000) * 0.2, (distance / 1000) * 0.4, (distance / 1000) * 0.1, 0.0);


                    Body body = Body.ExtrudeProfile(profile, 0.0002);


                    DesignBody.Create(Window.ActiveWindow.Document.MainPart, "ExternalForce" + set.activePointName, body);


                    foreach (DesignBody b in Window.ActiveWindow.Document.MainPart.Bodies)
                    {
                        if (b.Name.Equals("ExternalForce" + set.activePointName))
                        {
                            b.SetColor(null, System.Drawing.Color.Red);
                        }
                    }
                }
                
                
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Info");
            }
            
        }
    }
}
