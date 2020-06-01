/*
 * Sample CommandCapsule for the SpaceClaim API
 */
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Extensibility;
using StructureCreator.Properties;
using StructureCreator.UI_extensions.SolveUI;

namespace StructureCreator
{
    /// <summary>
    /// Opens lp creator form 
    /// </summary>
    class CreateLPCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.CreateLP";

        public CreateLPCapsule()
            : base(CommandName, Resources.CreateLPText, Resources.CreateLPImage, Resources.CreateLPHint)
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
            if (!Settings.Default.CreateLPFormOpened)
            {
                Settings.Default.CreateLPFormOpened = true;
                Settings.Default.Save();

                CreateLPForm form = new CreateLPForm();
                form.StartPosition = FormStartPosition.Manual;

                Screen screen = Screen.PrimaryScreen;
                Rectangle bounds = screen.Bounds;

                // Show form in left bottom corner 
                form.Location = (new Point(bounds.Width - 600 - 5, bounds.Height - 600 - 40 - 60)); // X=Screen Width - Form Width [614] - margin [5], Y=Screen Heigth - Form Heigth [498] - Taskbar W10 [40] - Taskbar Ansys [60]
                form.Show();
            }
        }
    }
}
