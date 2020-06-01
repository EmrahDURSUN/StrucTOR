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
    class YasolOptCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.YasolOpt";

        public YasolOptCapsule()
            : base(CommandName, Resources.YasolOptText, Resources.GLPSOLImage, null)
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
            
        }
    }
}
