/*
 * Sample CommandCapsule for the SpaceClaim API
 */
using System.Drawing;
using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Extensibility;
using StructureCreator.Properties;

namespace StructureCreator
{
    /// <summary>
    /// Creates a new scdoc file with selected folder as name
    /// </summary>
    class CreateSCDOCCapsule : CommandCapsule
    {
        public const string CommandName = "StructureCreator.CreateSCDOC";

        public CreateSCDOCCapsule()
            : base(CommandName, Resources.CreateSCDOCText, null, Resources.CreateSCDOCHint)
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
            // Create new document and save in project directory
            Document document = Document.Create();
            document.CoreProperties.Title = Settings.Default.ProjectName;
            document.CoreProperties.Identifier = System.DateTime.Now.Date.ToString();
            document.CoreProperties.Subject = Settings.Default.ProjectName;

            //document.CoreProperties.Identifier = date1.ToString(new CultureInfo("de-DE"));
            document.SaveAs(Settings.Default.ProjectPath + "\\CAD\\" + Settings.Default.ProjectName + ".scdoc");
        }
    }
}
