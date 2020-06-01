/*
 * Sample CommandCapsule for the SpaceClaim API
 */
using System.Drawing;
using System.Windows.Forms;
using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Extensibility;
using StructureCreator.Properties;
using StructureCreator.UI_extensions.SolveUI; 

namespace StructureCreator
{
    /// <summary>
    /// Opens CPLEX Solutions form
    /// </summary>
    class SolImportCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.SolImport";

        public SolImportCapsule()
            : base(CommandName, Resources.SolImportText, Resources.SolFileImage, Resources.SolImportHint)
        {
        }

        protected override void OnInitialize(Command command)
        {
            // If your command doesn't modify the document/model, uncomment the following line
            // to avoid creating an undo step.
            //command.IsWriteBlock = false;
            command.DisabledHint = "Do cplex Optimization first.";
            Settings set = Settings.Default;

            if (set.solImportCommand)
            {
                command.IsEnabled = true;
            }
            else
            {
                command.IsEnabled = false;
            }
        }

        protected override void OnUpdate(Command command)
        {
            // Check if button should be activated
            Settings set = Settings.Default;

            if (set.solImportCommand)
            {
                command.IsEnabled = true;
            }
            else
            {
                command.IsEnabled = false;
            }
        }

        protected override void OnExecute(Command command, ExecutionContext context, Rectangle buttonRect)
        {
            // Open form if none is opened yet
            if (!Settings.Default.CPLEXSolutionFormOpened)
            {
                Settings.Default.CPLEXSolutionFormOpened = true;
                Settings.Default.Save();

                solImportForm form = new solImportForm();
                form.StartPosition = FormStartPosition.Manual;

                Screen screen = Screen.PrimaryScreen;
                Rectangle bounds = screen.Bounds;

                // Show form in left bottom corner 
                form.Location = (new Point(bounds.Width - 681 - 5, bounds.Height - 340 - 40 - 60)); // X=Screen Width - Form Width [681] - margin [5], Y=Screen Heigth - Form Heigth [340] - Taskbar W10 [40] - Taskbar Ansys [60]
                form.Show();
            }
        }
    }
}
