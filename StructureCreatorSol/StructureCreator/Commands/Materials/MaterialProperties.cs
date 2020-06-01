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
    class MaterialPropertiesCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.MaterialProperties";

        public MaterialPropertiesCapsule()
            : base(CommandName, Resources.MaterialPropertiesText, Resources.MaterialPropImage, Resources.MaterialPropertiesHint)
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
