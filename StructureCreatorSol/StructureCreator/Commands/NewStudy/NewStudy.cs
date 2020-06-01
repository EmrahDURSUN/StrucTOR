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
    /// Opend the new study form.
    /// </summary>
    class NewStudyCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.NewStudy";

        public NewStudyCapsule()
            : base(CommandName, Resources.NewStudyText, Resources.NewStudyImage, Resources.NewStudyHint)
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

        [STAThread]
        protected override void OnExecute(Command command, SpaceClaim.Api.V19.ExecutionContext context, Rectangle buttonRect)
        {
            if (!Settings.Default.NewStudyFormOpened)
            {
                Settings.Default.NewStudyFormOpened = true;
                Settings.Default.Save();

                Thread _thread = new Thread(() =>
            {
                System.Windows.Forms.Application.Run(new NewStudyForm());
            });
                _thread.SetApartmentState(ApartmentState.STA);
                _thread.Start();
            }
        }
    }
}
