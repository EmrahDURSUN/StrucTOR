/*
 * Sample CommandCapsule for the SpaceClaim API
 */
using System.Drawing;
using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Extensibility;
using StructureCreator.Properties;
using System.Windows.Forms;
using StructureCreator.UI_extensions;

namespace StructureCreator
{
    class RibbonTabCapsuleCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.RibbonTab"; 

        public RibbonTabCapsuleCapsule()
            : base(CommandName, Resources.RibbonTabText, null, Resources.RibbonTabText)
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
            Settings set = Settings.Default;
            if (set.showHintOnTab)
            {
                UserGuideForm form = new UserGuideForm();
                form.Show();

                set.showHintOnTab = false;
                set.Save(); 
            }
            else
            {

            }
        }
    }
}
