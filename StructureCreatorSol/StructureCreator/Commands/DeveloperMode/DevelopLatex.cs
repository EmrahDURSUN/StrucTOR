/*
 * Sample CommandCapsule for the SpaceClaim API
 */
using System.Drawing;
using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Extensibility;
using StructureCreator.Properties;
using StructureCreator.UI_extensions;

namespace StructureCreator
{
    class DevelopLatexCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.Dlatex";

        public DevelopLatexCapsule()
            : base(CommandName, Resources.DLatexText, Resources.DLatexImage, Resources.DLatexHint)
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
            DevelopLatexForm f = new DevelopLatexForm();
            f.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            f.Show();
        }
    }
}
