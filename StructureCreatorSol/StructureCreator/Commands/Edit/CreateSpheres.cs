/*
 * Sample CommandCapsule for the SpaceClaim API
 */
using System.Drawing;
using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Extensibility;
using StructureCreator.Properties;
using System.Windows.Forms;
using StructureCreator.UI_extensions.EditUI;

namespace StructureCreator
{
    // Create Spheres button in ribbon
    class CreateSpheresCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.CreateSpheres";

        public CreateSpheresCapsule()
            : base(CommandName, Resources.CreateSpheresText, Resources.CreateSpheresImage, Resources.CreateSpheresHint)
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
            // Open form
            if (!Settings.Default.EditFormOpened)
            {
                Settings.Default.EditFormOpened = true;
                Settings.Default.Save();


                CreatespheresForm form = new CreatespheresForm();
                form.StartPosition = FormStartPosition.Manual;

                Screen screen = Screen.PrimaryScreen;
                Rectangle bounds = screen.Bounds;

                // Show form in left bottom corner 
                form.Location = (new Point(bounds.Width - 500 - 5, bounds.Height - 300 - 40 - 60)); // X=Screen Width - Form Width [395] - margin [5], Y=Screen Heigth - Form Heigth [210] - Taskbar W10 [40] - Taskbar Ansys [60]
                form.Show();
            }
        }
    }
}