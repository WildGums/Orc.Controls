namespace Orc.Controls
{
    using Catel.MVVM;

    public interface ICalloutManager
    {
        void Register(IViewModel callout);
        void Unregister(IViewModel callout);
    }
}
