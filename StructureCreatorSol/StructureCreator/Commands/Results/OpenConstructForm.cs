/*
 * Sample CommandCapsule for the SpaceClaim API
 */
using System.Drawing;
using System.Windows.Forms;
using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Extensibility;
using StructureCreator.Properties;
using StructureCreator.UI_extensions.ResultsUI;

namespace StructureCreator
{
    /// <summary>
    /// Opens construct form.
    /// </summary>
    class OpenConstructFormCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.OpenConstructForm";

        public OpenConstructFormCapsule()
            : base(CommandName, Resources.OpenConstructFormText,Resources.icon_solution_results_lattice_density_beta_32x32, Resources.OpenConstructFormHint)
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
            if (!Settings.Default.ConstructFormOpened)
            {
                Settings.Default.ConstructFormOpened = true;
                Settings.Default.Save();

                ConstructForm form = new ConstructForm();
                form.StartPosition = FormStartPosition.Manual;

                Screen screen = Screen.PrimaryScreen;
                Rectangle bounds = screen.Bounds;

                // Show form in left bottom corner 
                form.Location = (new Point(bounds.Width - 395 - 5, bounds.Height - 210 - 40 - 60)); // X=Screen Width - Form Width [395] - margin [5], Y=Screen Heigth - Form Heigth [210] - Taskbar W10 [40] - Taskbar Ansys [60]
                form.Show();
            }
        }
    }
}
