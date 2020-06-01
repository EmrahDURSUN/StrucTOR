/*
 * Sample CommandCapsule for the SpaceClaim API
 */
using System.Drawing;
using System.Windows.Forms;
using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Extensibility;
using StructureCreator.Properties;
using StructureCreator.UI_extensions.LoadsUI;

namespace StructureCreator
{
    /// <summary>
    /// Opens form for force creator
    /// </summary>
    class OpenStructuralLoadsCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.OpenStructuralLoads";

        public OpenStructuralLoadsCapsule()
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
        }

        protected override void OnExecute(Command command, ExecutionContext context, Rectangle buttonRect)
        {
            // Open form if none is opened yet
            if (!Settings.Default.ForceFormOpened)
            {
                Settings.Default.ForceFormOpened = true;
                Settings.Default.Save();

                CreateForceForm form = new CreateForceForm();
                form.StartPosition = FormStartPosition.Manual;

                Screen screen = Screen.PrimaryScreen;
                Rectangle bounds = screen.Bounds;

                // Show form in left bottom corner 
                form.Location = (new Point(bounds.Width - 634 - 5, bounds.Height - 303 - 40 - 60 )); // X=Screen Width - Form Width [604] - margin [5], Y=Screen Heigth - Form Heigth [303] - Taskbar W10 [40] - Taskbar Ansys [60]

                form.Show();
            }
        }
    }
}
