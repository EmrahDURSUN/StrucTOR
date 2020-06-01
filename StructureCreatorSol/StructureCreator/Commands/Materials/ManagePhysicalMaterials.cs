/*
 * Sample CommandCapsule for the SpaceClaim API
 */
using System.Drawing;
using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Extensibility;
using StructureCreator.Properties;
using System.Windows.Forms; 

namespace StructureCreator
{
    class ManagePhysicalMaterialsCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.ManageMaterial";

        public ManagePhysicalMaterialsCapsule()
            : base(CommandName, Resources.ManagePhysicalMaterialsText, Resources.ManageMaterialImage, Resources.ManagePhysicalMaterialsHint)
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
            //MessageBox.Show("Not yet implemented");
        }
    }
}
