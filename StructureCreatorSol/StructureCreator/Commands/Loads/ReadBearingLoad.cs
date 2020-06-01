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
    /// Saves the selectes point in Settings 
    /// </summary>
    class ReadBearingLoadCapsule : CommandCapsule
    {
        public const string CommandName = "StructureCreator.ReadBearingLoad";

        public ReadBearingLoadCapsule()
            : base(CommandName, Resources.ReadBearingLoadText, null, Resources.ReadBearingLoadHint)
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
            Window window = Window.ActiveWindow;
            command.IsEnabled = window != null && window.ActiveContext.SingleSelection is DatumPoint;
        }

        protected override void OnExecute(Command command, ExecutionContext context, Rectangle buttonRect)
        {
            Part mainPart = Window.ActiveWindow.Document.MainPart;
            Window.ActiveWindow.InteractionMode = InteractionMode.Solid;
            Window.ActiveWindow.ZoomSelection();

            Settings set = Settings.Default;
            DatumPoint p = (DatumPoint)Window.ActiveWindow.ActiveContext.SingleSelection.Master;
            set.activePoint = "Point : " + p.Name + " (x:" + p.Position.X * 1000 + ", y:" + p.Position.Y * 1000 + ", z:" + p.Position.Z * 1000 + ")";
            set.activePointCoord = "" + p.Position.X + ";" + p.Position.Y + ";" + p.Position.Z;
            set.activePointName = p.Name;
            set.Save();
        }
    }
}
