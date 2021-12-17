namespace Orc.Automation
{
    using System;
    using Catel;

    public static class IDisposableExtensions
    {
        public static TResult Execute<TResult>(this IDisposable disposable, Func<TResult> func)
        {
            Argument.IsNotNull(() => func);

#pragma warning disable IDISP007 // Don't dispose injected.
            using (disposable)
#pragma warning restore IDISP007 // Don't dispose injected.
            {
                return func.Invoke();
            }
        }

        public static void Execute(this IDisposable disposable, Action action)
        {
            Argument.IsNotNull(() => action);

            disposable.Execute(action.MakeDefault<bool>());
        }
    }
}
