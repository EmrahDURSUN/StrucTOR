using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using StructureCreator.Properties;
using SpaceClaim.Api.V19.Extensibility;
using SpaceClaim.Api.V19;
using StructureCreator.UI_extensions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace StructureCreator
{
    class DataControlCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.DataControl";

        public DataControlCapsule()
            : base(CommandName, Resources.DataControlText, Resources.TableImage, Resources.DataControlHint)
        {
        }

        protected override void OnUpdate(Command command)
        {
            command.IsEnabled = SpaceClaim.Api.V19.Window.ActiveWindow != null;
        }

        protected override void OnExecute(Command command, ExecutionContext context, Rectangle buttonRect)
        {


            // These three line are responsibly for calling Windows Form => Our form name is PointsCalForm
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.Run(new DataControlForm());
            // that line above means = Run that form wait until it finish then continue

            // TODO - take data from user after calculation of it save it to csv file
        }
    }
}
