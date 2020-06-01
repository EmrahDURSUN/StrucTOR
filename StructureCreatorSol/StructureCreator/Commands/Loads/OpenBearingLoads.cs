/*
 * Sample CommandCapsule for the SpaceClaim API
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Extensibility;
using StructureCreator.Properties;
using StructureCreator.UI_extensions.LoadsUI;

namespace StructureCreator
{
    /// <summary>
    /// Opens the bearing load form. Ribbon button 'Create load'
    /// </summary>
    class OpenBearingLoadsCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.OpenBearingLoads";

        public OpenBearingLoadsCapsule()
            : base(CommandName, Resources.OpenBearingLoadsText, Resources.OpenBearingLoadsImage, Resources.OpenBearingLoadsHint)
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
            if (!Settings.Default.BearingFormOpened)
            {
                Settings.Default.BearingFormOpened = true;
                Settings.Default.Save();

                CreateBearingLoad form = new CreateBearingLoad();
                form.StartPosition = FormStartPosition.Manual;

                Screen screen = Screen.PrimaryScreen;
                Rectangle bounds = screen.Bounds;

                // Show form in left bottom corner 
                form.Location = (new Point(bounds.Width - 634 - 5, bounds.Height - 303 - 40 - 60)); // X=Screen Width - Form Width [604] - margin [5], Y=Screen Heigth - Form Heigth [303] - Taskbar W10 [40] - Taskbar Ansys [60]

                form.Show();
            }
        }
    }
}
