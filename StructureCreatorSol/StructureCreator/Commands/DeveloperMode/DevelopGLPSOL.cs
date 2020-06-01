/*
 * Sample CommandCapsule for the SpaceClaim API
 */
using System;
using System.Drawing;
using System.Threading;
using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Extensibility;
using StructureCreator.Properties;
using StructureCreator.UI_extensions; 

namespace StructureCreator
{
    /// <summary>
    /// Use the glpsolver to create .lp file of .mod file
    /// </summary>
    class DevelopGlpsolCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.Dglpsol";

        public DevelopGlpsolCapsule()
            : base(CommandName, Resources.DGLPSOLText, Resources.GLPSOLImage, Resources.DGLPSOLHint)
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
            command.IsEnabled = SpaceClaim.Api.V19.Window.ActiveWindow != null;
        }

        [STAThread]
        protected override void OnExecute(Command command, SpaceClaim.Api.V19.ExecutionContext context, Rectangle buttonRect)
        {
            Thread _thread = new Thread(() =>
            {
                System.Windows.Forms.Application.Run(new DevelopGLPSOLForm());
            });
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.Start();
        }
    }
}
