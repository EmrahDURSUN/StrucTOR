/*
 * Sample CommandCapsule for the SpaceClaim API
 */
using System.Drawing;
using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Extensibility;
using StructureCreator.Properties;

namespace StructureCreator
{
    // ribbon button 'new study'
    class NewStudyGroupCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.NewStudyGroup";

        public NewStudyGroupCapsule()
            : base(CommandName,null /*Resources.NewStudyText*/, Resources.NewStudyImage, null /*Resources.NewStudyHint*/)
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
        }
    }
}
