using StructureCreator.Properties;
using SpaceClaim.Api.V19.Extensibility;
using SpaceClaim.Api.V19;
using System.Windows.Forms;
using StructureCreator.UI_extensions;
using System.Globalization;

namespace StructureCreator
{
    /// <summary>
    /// That's our main class, we named it ConstructorAddIn8, that it is same name as addressed at manifest.xml typename section
    /// </summary>
    public class ConstructorAddIn8 : AddIn, IExtensibility, ICommandExtensibility, IRibbonExtensibility    {

        /// <summary>
        /// These are actually our ribbon and buttons. The place where Button occupy called as Capsule. Each Capsule will later initialize at ICommandExtensibility section 
        /// </summary>
        readonly CommandCapsule[] capsules = new[] {
            new RibbonTabCapsuleCapsule(),
            //new CommandCapsule("ConstructorAddIn.C#.V19.RibbonTab", Resources.RibbonTabText),
            new CommandCapsule("ConstructorAddIn.C#.V19.FirstGroup", Resources.FirstGroupText),
            new CommandCapsule("ConstructorAddIn.C#.V19.SecondGroup", Resources.SecondGroupText),
            new CommandCapsule("ConstructorAddIn.C#.V19.ThirdGroup", Resources.ThirdGroupText),
            new CommandCapsule("ConstructorAddIn.C#.V19.ForthdGroup", Resources.ForthGroupText),
            new CommandCapsule("ConstructorAddIn.C#.V19.ExtraGroup", Resources.ExtraGroupText),
            new OpenConstructFormCapsule(),
            new CreatePointCloudCapsule(),
            new DataControlCapsule(),
            new CreateStrucTorCapsule(),
            new ExportStlCapsule(),
            new ExportStepCapsule(),
            new GroundStructureCapsule(),
            new BccCapsule(),
            new FccCapsule(),
            new CompleteSelectionCapsule(),
            new MergeFacesCapsule(),
            new CreateBarCapsule(),
            new CreatePPCylinderCapsule(),
            new ImportDataCapsule(),
            new CylinderPPCapsule(),
            new DevelopCplexCapsule(), 
            new DevelopSolImport(), 
            new DevelopGlpsolCapsule(), 
            new DevelopEditStabsCapsule(), 
            //new DevelopCreateStructureCapsule(), 
            new NewStudyCapsule(), 
            new NewStudyGroupCapsule(), 
            new DevelopCapsule(), 
            new DevelopLatexCapsule(), 
            new SimplifyCapsule(), 
            new Dummy8Capsule(),
            new Dummy9Capsule(),
            new Dummy10Capsule(), 
            new MaterialsCapsule(), 
            new StudyMaterialsCapsule(), 
            new MaterialPropertiesCapsule(), 
            new ManagePhysicalMaterialsCapsule(), 
            new ConstraintsCapsule(), 
            new AssemblySpaceCapsule(), 
            new LoadsCapsule(), 
            new AngularGlobalLoadCapsule(), 
            new EditGravityCapsule(), 
            new LinearGlobalLoadCapsule(), 
            new PointMassAutoCapsule(), 
            new PointMassManualCapsule(), 
            new StructuralLoadsCapsule(), 
            new ToggleGravityOnCapsule(), 
            new SolveCapsule(), 
            new ManageCapsule(), 
            new PostprocessingCapsule(), 
            new ResultFilesCapsule(), 
            new ResultsCapsule(),
            new ConstructCapsule(), 
            new GlpsolOptCapsule(), 
            new CplexOptCapsule(), 
            new SolImportCapsule(), 
            new LatexCapsule(), 
            new ReadDataCapsule(), 
            new CreateLPCapsule(), 
            new CreateSpheresCapsule(), 
            new OpenStructuralLoadsCapsule(), 
            new ReadForcePointCapsule(), 
            new OpenBearingLoadsCapsule(), 
            new ReadBearingLoadCapsule(), 
            new BearingLoadsCapsuleCapsule(), 
            new CreateSCDOCCapsule(), 
            new YasolOptCapsule(), 
            new AssemblySpace2DCapsule(), 
            new CreateQLPCapsule(), 
            new YasolSolutionCapsule(), 
            new RemoveArrowCapsule(), 
            new RemoveArrowLoadsCapsule()
        };

        #region IExtensibility Members

        public bool Connect()
        {
            // That's connecting our tool to SpaceClaim, name must macth whas is written at Ribbon XML file under resources folder

            SpaceClaim.Api.V19.Unsupported.JournalMethods.RecordAutoLoadAddIn("ConstructorAddIn.C#.V19.RibbonTab", Resources.AddInManifestInfo);

            // Activate Tutorial ast Start
            // if (Settings.Default.showHintOnTab)
            // {
            //    UserGuideForm guide = new UserGuideForm();
            //    guide.Show();
            // }

            // Set culture Info
            CultureInfo.CurrentCulture = new CultureInfo("de-DE", false);

            // Reset all settings that are used in one study
            Settings set = Settings.Default;

            set.csv1Path = "";
            set.csv2Path = "";
            set.csv3Path = "";

            set.Active = true;
            set.DevelopMode = true;
            set.cmdActive = false;
            set.ProjectPath = "";
            set.ProjectName = "";

            set.glpsolCommand = true;
            set.cplexCommand = true;
            set.solImportCommand = true;
            set.latexCommand = true;
            set.lplogPath = "";
            set.cplexlogPath = "";

            set.showHintOnTab = true;

            set.forces = "";
            set.forceCount = 0;

            set.bearingLoadCount = 0;
            set.bearingLoads = "";

            set.deleteArrows = ""; 

            // Only allow one opened form each
            set.CrossSecFormOpened = false; 
            set.ForceFormOpened = false; 
            set.BearingFormOpened = false; 
            set.CreateLPFormOpened = false; 
            set.GLPKFormOpened = false; 
            set.CPLEXFormOpened = false; 
            set.CreatePDFFormOpened = false; 
            set.CPLEXSolutionFormOpened = false; 
            set.ConstructFormOpened = false; 
            set.ResultsFormOpened = false; 
            set.EditFormOpened = false; 
            set.NewStudyFormOpened = false; 

            set.Save();


            return true;
        }

        public void Disconnect()
        {
            // Perform any cleanup for your add-in here.
        }

        #endregion

        #region ICommandExtensibility Members

        public void Initialize()
        {
            foreach (CommandCapsule capsule in capsules)
                capsule.Initialize();

            SpaceClaim.Api.V19.Application.AddOptionsPage(new Options());

            //Settings set = Settings.Default;
            //set.Active = true;
            //set.DevelopMode = false;
            //set.ProjectPath = "";
            //set.ProjectName = "";

            //set.glpsolCommand = false; 
            //set.cplexCommand = false; 
            //set.solImportCommand = false; 
            //set.latexCommand = true;

            //set.showHintOnTab = false; 

            //set.forces = "";
            //set.forceCount = 0;
            //set.Save();
        }

        #endregion

        #region IRibbonExtensibility Members

        public string GetCustomUI()
        {
            /*
			 * This method is called during startup.  The 'command' attributes in the XML refer
			 * to the names of Command objects created during the Initialize method, which will
			 * have already been called at this point.
			 */
            return Resources.Ribbon;
        }

        #endregion
    }

    class Options : OptionsPage
    {
        Command command;

        public override Command Command
        {
            get
            {
                if (command == null)
                {
                    command = Command.Create("SampleAddIn.C#.V19.Options");
                    command.Text = "construcTOR";
                    command.Hint = "Set the path variables";
                    //command.HintImage = Resources.CPLEXImage;

                    command.KeepAlive(true);
                }
                return command;
            }
        }

        public override Control CreateControl()
        {
            return new OptionsControl();
        }

        public override void OnLoad(Control pageControl)
        {
            var control = (OptionsControl)pageControl;
            control.LoadOptions();
        }

        public override void OnSave(Control pageControl)
        {
            var control = (OptionsControl)pageControl;
            control.SaveOptions();
        }
    }


}