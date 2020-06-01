/*
 * Sample CommandCapsule for the SpaceClaim API
 */
using System.Drawing;
using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Extensibility;
using StructureCreator.Properties;
using System.Windows.Forms;
using StructureCreator.UI_extensions.ResultsUI;

namespace StructureCreator
{
    /// <summary>
    /// Opens result files form
    /// </summary>
    class ResultFilesCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.ResultFiles";

        public ResultFilesCapsule()
            : base(CommandName, Resources.ResultFilesText, Resources.ResultFilesImage, Resources.ResultFilesHint)
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
            if (!Settings.Default.ResultsFormOpened)
            {
                Settings.Default.ResultsFormOpened = true;
                Settings.Default.Save();

                ResultsFormNew form = new ResultsFormNew();
                form.StartPosition = FormStartPosition.Manual;

                Screen screen = Screen.PrimaryScreen;
                Rectangle bounds = screen.Bounds;

                // Show form in left bottom corner 
                form.Location = (new Point(bounds.Width - 837 - 5, bounds.Height - 452 - 40 - 60)); // X=Screen Width - Form Width [837] - margin [5], Y=Screen Heigth - Form Heigth [452] - Taskbar W10 [40] - Taskbar Ansys [60]
                form.Show();
            }
        }
    }
}
