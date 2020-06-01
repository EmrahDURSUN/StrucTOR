/*
 * Sample CommandCapsule for the SpaceClaim API
 */
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Extensibility;
using StructureCreator.Properties;
using StructureCreator.UI_extensions.SolveUI;

namespace StructureCreator
{
    class CreateQLPCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.CreateQLP";

        public CreateQLPCapsule()
            : base(CommandName, Resources.CreateQLPText, Resources.CreateLPImage, Resources.CreateQLPHint)
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
