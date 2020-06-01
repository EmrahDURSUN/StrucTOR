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
    /// Transform a .sol file into csv files with points or coordinates
    /// </summary>
    class DevelopSolImport : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.DsolFile";

        public DevelopSolImport()
            : base(CommandName, Resources.DSolFileText, Resources.SolFileImage, Resources.DSolFileHint)
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
            command.IsEnabled = Window.ActiveWindow != null;
        }

        [STAThread]
        protected override void OnExecute(Command command, SpaceClaim.Api.V19.ExecutionContext context, Rectangle buttonRect)
        {
            //SolImportForm i = new SolImportForm();
            //i.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen; 

            //// Show() opens the Windows.Form
            //i.Show();

            Thread _thread = new Thread(() =>
            {
                System.Windows.Forms.Application.Run(new DevelopSolImportForm());
            });
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.Start();
        }
    }
}
