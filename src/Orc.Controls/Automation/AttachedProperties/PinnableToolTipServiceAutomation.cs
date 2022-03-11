namespace Orc.Controls.Automation
{
    using System.Windows.Controls.Primitives;
    using Orc.Automation;

    [AutomationAccessType(DefaultOwnerType = typeof(PinnableToolTipService))]
    public class PinnableToolTipServiceModel : AutomationControlModel
    {
        public PinnableToolTipServiceModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        public int InitialShowDelay { get; set; }
        public int ShowDuration { get; set; }
        public bool IsToolTipOwner { get; set; }
        public PlacementMode Placement { get; set; }
        //TODO:
        //public object PlacementTarget { get; set; }
    }
}
