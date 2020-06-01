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
    class ImportDataCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.ImportData";


        public ImportDataCapsule()
            : base(CommandName, Resources.ImportDataText, Resources.ImportDataImage, Resources.ImportDataHint)
        {
        }

        protected override void OnUpdate(Command command)
        {
            command.IsEnabled = SpaceClaim.Api.V19.Window.ActiveWindow != null;
        }

        protected override void OnExecute(Command command, ExecutionContext context, Rectangle buttonRect)
        {

           
            // These three line are responsibly for calling Windows Form => Our form name is SolFileForm
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.Run(new SolFileForm());

            /* Ignore the rest they are my tests

            // ImportingFun.LoadSampleData();

            // OpenFileDialog ofd = new OpenFileDialog();

            //var ofd = new OpenFileDialog
            //{
            //    Title = @"Data Selection",
            //    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            //    RestoreDirectory = true,
            //    Filter = @"Sol File|*.sol|Text File|*.txt|All files|*.*",
            //    CheckFileExists = true,
            //    Multiselect = false
            //};

            //if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{

            //    using (var dialog = new SolFileForm())
            //    {
            //        if (dialog.ShowDialog() != DialogResult.OK)
            //        {

            //            StreamReader sr = new StreamReader(File.OpenRead(ofd.FileName));
            //            //  string templatePath = ofd.FileName;
            //            //  Document doc = Document.Load(templatePath);


            //            //SolFileForm textBox1 = new SolFileForm();



            //            var asd = new System.Windows.Forms.TextBox();
            //            asd.textBox1 = sr.ReadToEnd();
            //            sr.Dispose();
            //        }


            //    }


            //}
            */
        }

    }
}