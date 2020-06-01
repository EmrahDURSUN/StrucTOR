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
    /// Opens GLPK form
    /// </summary>
    class GlpsolOptCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.GlpsolOpt";

        public GlpsolOptCapsule()
            : base(CommandName, Resources.GlpsolOptText, Resources.GLPSOLImage, Resources.GlpsolOptHint)
        {
        }

        protected override void OnInitialize(Command command)
        {
            // If your command doesn't modify the document/model, uncomment the following line
            // to avoid creating an undo step.
            //command.IsWriteBlock = false;
            Settings set = Settings.Default;

            if (set.glpsolCommand)
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
            // check if button should be activated
            Settings set = Settings.Default;

            if (set.glpsolCommand)
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
            if (!Settings.Default.GLPKFormOpened)
            {
                Settings.Default.GLPKFormOpened = true;
                Settings.Default.Save();

                glpsolOptForm form = new glpsolOptForm();
                form.StartPosition = FormStartPosition.Manual;

                Screen screen = Screen.PrimaryScreen;
                Rectangle bounds = screen.Bounds;

                // Show form in left bottom corner 
                form.Location = (new Point(bounds.Width - 714 - 5, bounds.Height - 351 - 40 - 60)); // X=Screen Width - Form Width [681] - margin [5], Y=Screen Heigth - Form Heigth [340] - Taskbar W10 [40] - Taskbar Ansys [60]
                form.Show();
            }
        }
    }
}
