/*
 * Sample CommandCapsule for the SpaceClaim API
 */
using System.Drawing;
using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Extensibility;
using StructureCreator.Properties;

namespace StructureCreator
{
    // ribbon button 'developer mode'
    class DevelopCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.Develop";

        public DevelopCapsule()
            : base(CommandName, null /*Resources.DevelopText*/, Resources.DevelopImage, Resources.DevelopHint)
        {
        }

        protected override void OnInitialize(Command command)
        {
            // If your command doesn't modify the document/model, uncomment the following line
            // to avoid creating an undo step.
            //command.IsWriteBlock = false;

            Settings set = Settings.Default;
            if (set.DevelopMode)
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
            // Make sure only one window is opened
            Settings set = Settings.Default;
            if (set.DevelopMode)
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
