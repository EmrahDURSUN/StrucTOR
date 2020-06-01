/*
 * Sample CommandCapsule for the SpaceClaim API
 */
using System.Drawing;
using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Extensibility;
using StructureCreator.Properties;
using System.Windows.Forms;
using System;

namespace StructureCreator
{
    /// <summary>
    /// Saves the selectes point in Settings 
    /// </summary>
    class RemoveArrowLoadsCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.RemoveArrowLoadsCapsule";

        public RemoveArrowLoadsCapsule()
            : base(CommandName, null, null, null)
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
            try
            {
                String[] arrows = Settings.Default.deleteArrows.Split(';');

                foreach (DesignBody b in Window.ActiveWindow.Document.MainPart.Bodies)
                {
                    foreach (String s in arrows)
                    {
                        if (!s.Equals(""))
                        {
                            if (b.Name.Equals("BearingReactionForce" + s))
                            {
                                b.Delete();
                            }
                        }
                    }
                }

                Settings.Default.deleteArrows = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}
