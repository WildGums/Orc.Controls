namespace Orc.Automation
{
    using System.Threading;

    public static class Wait
    {
        /// <summary>
        /// Wait control to be responsive after manipulations
        /// </summary>
        public static void UntilResponsive()
        {
            //TODO:Vladimir: Look at Wait_ref.cs and implement it more accurate
            Thread.Sleep(100);
        }
    }
}
