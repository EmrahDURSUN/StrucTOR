﻿/*
 * Sample CommandCapsule for the SpaceClaim API
 */
using System.Drawing;
using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Extensibility;
using StructureCreator.Properties;
using System.Windows.Forms; 

namespace StructureCreator
{
    // ribbon button 'loads'
    class LoadsCapsule : CommandCapsule
    {
        public const string CommandName = "ConstructorAddIn.C#.V19.Loads";

        public LoadsCapsule()
            : base(CommandName, null /*Resources.LoadsText*/, Resources.LoadsImage, null /*Resources.LoadsHint*/)
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
        }
    }
}
