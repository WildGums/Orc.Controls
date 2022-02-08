namespace Orc.Controls.Automation
{
    using Orc.Automation;

    public class ColorPickerAutomationPeer : ControlRunMethodAutomationPeerBase<Controls.ColorPicker>
    {
        public ColorPickerAutomationPeer(Controls.ColorPicker owner)
            : base(owner)
        {
            owner.ColorChanged += OnColorChanged;
        }

        private void OnColorChanged(object sender, ColorChangedEventArgs e)
        {
            RaiseEvent(nameof(Controls.ColorPicker.ColorChanged), null);
        }
    }
}
