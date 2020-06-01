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
    /// Open pgf/tikz form
    /// </summary>
    class LatexCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.Latex";

        public LatexCapsule()
            : base(CommandName, Resources.LatexText, Resources.DLatexImage, Resources.LatexHint)
        {
        }

        protected override void OnInitialize(Command command)
        {
            // If your command doesn't modify the document/model, uncomment the following line
            // to avoid creating an undo step.
            //command.IsWriteBlock = false;
            command.DisabledHint = "Do csv command first.";
            Settings set = Settings.Default;

            if (set.latexCommand)
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

            if (set.latexCommand)
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
            if (!Settings.Default.CreatePDFFormOpened)
            {
                Settings.Default.CreatePDFFormOpened = true;
                Settings.Default.Save();

                latexform form = new latexform();
                form.StartPosition = FormStartPosition.Manual;

                Screen screen = Screen.PrimaryScreen;
                Rectangle bounds = screen.Bounds;

                // Show form in left bottom corner 
                form.Location = (new Point(bounds.Width - 700 - 5, bounds.Height - 650 - 40 - 60)); // X=Screen Width - Form Width [681] - margin [5], Y=Screen Heigth - Form Heigth [619] - Taskbar W10 [40] - Taskbar Ansys [60]
                form.Show();
            }
        }
    }
}
