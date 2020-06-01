/*
 * Sample CommandCapsule for the SpaceClaim API
 */
using System.Drawing;
using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Extensibility;
using StructureCreator.Properties;
using StructureCreator.UI_extensions;

namespace StructureCreator
{
    /// <summary>
    /// Open the stabs of a csv file and edit the diameter of the stabs
    /// </summary>
    class DevelopEditStabsCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.Dtable";

        public DevelopEditStabsCapsule()
            : base(CommandName, Resources.DTableText, Resources.TableImage, Resources.DTableHint)
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
            command.IsEnabled = SpaceClaim.Api.V19.Window.ActiveWindow != null;
        }

        protected override void OnExecute(Command command, ExecutionContext context, Rectangle buttonRect)
        {
            DevelopEditStabsForm f = new DevelopEditStabsForm();
            f.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen; 
            f.Show();
        }
    }
}
