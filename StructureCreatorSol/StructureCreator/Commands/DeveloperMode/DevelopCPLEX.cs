/*
 * Sample CommandCapsule for the SpaceClaim API
 */
using System;
using System.Drawing;
using System.Threading;
using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Extensibility;
using StructureCreator.Properties;
using System.Windows.Forms;

using StructureCreator.UI_extensions; 

namespace StructureCreator
{
    /// Use the cplex optimization to create a .sol file of a .lp file ///

    class DevelopCplexCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.Dcplex";

        public DevelopCplexCapsule()
            : base(CommandName, Resources.DCPLEXText, Resources.CPLEXImage, Resources.DCPLEXHint)
        {
        }

        protected override void OnInitialize(Command command)
        {
            // If your command doesn't modify the document/model, uncomment the following line
            // to avoid creating an undo step.
            //command.IsWriteBlock = false;
            command.DisabledHint = "jisoi";
            //command.Hint = "This is hint";
            //command.KeyTip = "tessssst";
            //command.HelpId = "";
            //command.IsEnabled = tr;
            //command.Image = Resources.CPLEXImage;


            //Settings set = Settings.Default;
            //if (set.Active)
            //{
            //    command.IsEnabled = true;
            //    //MessageBox.Show("Now enabled");
            //}
            //else
            //{
            //    command.IsEnabled = false;
            //    //MessageBox.Show("Now unabled");
            //}
        }

        protected override void OnUpdate(Command command)
        {
            command.IsEnabled = SpaceClaim.Api.V19.Window.ActiveWindow != null;

            //Settings set = Settings.Default;
            //if (set.Active)
            //{
            //    command.IsEnabled = true;
            //    //MessageBox.Show("Now enabled");
            //    //Window.ActiveWindow.Command.UpdateFrequency = UpdateFrequency.Always;
            //    Window.ActiveWindow.Command.Execute();
            //}
            //else
            //{
            //    command.IsEnabled = false;
            //    //MessageBox.Show("Now unabled");
            //    //Window.ActiveWindow.Command.UpdateFrequency = UpdateFrequency.Always;
            //    Window.ActiveWindow.Command.Execute();
            //}
        }


        [STAThread]
        protected override void OnExecute(Command command, SpaceClaim.Api.V19.ExecutionContext context, Rectangle buttonRect)
        {
            //CPLEXForm f = new CPLEXForm();
            //f.Show();

                                 
            Thread _thread = new Thread(() =>
            {
                System.Windows.Forms.Application.Run(new DevelopCPLEXForm()); 

            });
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.Start();
        }
    }
}
