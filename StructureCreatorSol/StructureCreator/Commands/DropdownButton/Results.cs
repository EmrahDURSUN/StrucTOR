/*
 * Sample CommandCapsule for the SpaceClaim API
 */
using System.Drawing;
using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Extensibility;
using StructureCreator.Properties;

namespace StructureCreator
{
    // ribbon button 'results'
    class ResultsCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.Results";

        public ResultsCapsule()
            : base(CommandName, null /*Resources.ResultsText*/, Resources.ResultsImage, Resources.ResultsHint)
        {
        }

        protected override void OnInitialize(Command command)
        {
            // If your command doesn't modify the document/model, uncomment the following line
            // to avoid creating an undo step.
            //command.IsWriteBlock = false;
            //command.IsEnabled = false;
            command.DisabledHint = "Do a csv import first.";
        }

        protected override void OnUpdate(Command command)
        {
        }

        protected override void OnExecute(Command command, ExecutionContext context, Rectangle buttonRect)
        {
        }
    }
}
