/*
 * Sample CommandCapsule for the SpaceClaim API
 */
using System.Drawing;
using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Extensibility;
using StructureCreator.Properties;

namespace StructureCreator
{
    // ribbon button 'solve'
    class SolveCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.Solve";

        public SolveCapsule()
            : base(CommandName,null /*Resources.SolveText*/, Resources.SolveImage, Resources.SolveHint)
        {
        }

        protected override void OnInitialize(Command command)
        {
            // If your command doesn't modify the document/model, uncomment the following line
            // to avoid creating an undo step.
            //command.IsWriteBlock = false;
            command.IsEnabled = true;
            command.DisabledHint = "Create a new study first.";

            Settings set = Settings.Default;

            command.IsEnabled = false; 

            //if (set.glpsolCommand)
            //{
            //    command.IsEnabled = true;
            //}
            //else
            //{
            //    command.IsEnabled = false;
            //}
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
        }
    }
}
