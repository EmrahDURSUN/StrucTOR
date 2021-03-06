﻿using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Extensibility;

namespace StructureCreator
{

    /// <summary>
    /// This base class provides on-the-fly language switching for add-in commands.
    /// </summary>
    abstract class BaseCommandCapsule : CommandCapsule
    {
        public delegate string TextDelegate();
        readonly TextDelegate textDelegate;
        readonly TextDelegate hintDelegate;

        protected BaseCommandCapsule(string commandName, TextDelegate textDelegate, System.Drawing.Image image, TextDelegate hintDelegate = null)
            : base(commandName, textDelegate != null ? textDelegate() : null, image, hintDelegate != null ? hintDelegate() : null)
        {

            this.textDelegate = textDelegate;
            this.hintDelegate = hintDelegate;
        }

        protected override void OnInitialize(Command command)
        {
            SpaceClaim.Api.V19.Options.CultureChanged += Options_CultureChanged;
        }

        void Options_CultureChanged(object sender, System.EventArgs e)
        {
            Command.Text = textDelegate();
            Command.Hint = hintDelegate();
        }
    }
}