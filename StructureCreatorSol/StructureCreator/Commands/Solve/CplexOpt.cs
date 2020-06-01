/*
 * Sample CommandCapsule for the SpaceClaim API
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Extensibility;
using StructureCreator.Properties;
using StructureCreator.UI_extensions.SolveUI;

namespace StructureCreator
{
    /// <summary>
    /// Opens CPLEX form
    /// </summary>
    class CplexOptCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.CplexOpt";

        public CplexOptCapsule()
            : base(CommandName, Resources.CplexOptText, Resources.GLPSOLImage, Resources.CplexOptHint)
        {
        }

        protected override void OnInitialize(Command command)
        {
            // If your command doesn't modify the document/model, uncomment the following line
            // to avoid creating an undo step.
            //command.IsWriteBlock = false;
            //command.DisabledHint = "";
            Settings set = Settings.Default;

            if (set.cplexCommand)
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
            Settings set = Settings.Default;

            if (set.cplexCommand)
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
            if (!Settings.Default.CPLEXFormOpened)
            {
                try
                {
                    Settings.Default.CPLEXFormOpened = true;
                    Settings.Default.Save();

                    cplexOptForm form = new cplexOptForm();
                    form.StartPosition = FormStartPosition.Manual;

                    Screen screen = Screen.PrimaryScreen;
                    Rectangle bounds = screen.Bounds;

                    // Show form in left bottom corner 
                    form.Location = (new Point(bounds.Width - 679 - 5, bounds.Height - 380 - 40 - 60)); // X=Screen Width - Form Width [681] - margin [5], Y=Screen Heigth - Form Heigth [340] - Taskbar W10 [40] - Taskbar Ansys [60]
                    form.Show();
                }catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                
            }
        }
    }
}
