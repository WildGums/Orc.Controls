namespace Orc.Controls.Automation
{
    using Orc.Automation;

    public class ColorPickerAutomationPeer : ControlRunMethodAutomationPeerBase<ColorPicker>
    {
        public ColorPickerAutomationPeer(ColorPicker owner)
            : base(owner)
        {
            owner.ColorChanged += OnColorChanged;
        }

        private void OnColorChanged(object sender, ColorChangedEventArgs e)
        {
            RaiseEvent(nameof(ColorPicker.ColorChanged), null);
        }
    }
}
