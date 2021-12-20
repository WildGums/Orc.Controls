namespace Orc.Automation.Tests
{
    using System.Collections;

    public abstract class EnumerableTestCaseBase : IEnumerable
    {
        #region IEnumerable Members
        public IEnumerator GetEnumerator()
        {
            return GetTestCases().GetEnumerator();
        }
        #endregion

        //Took border values
        protected abstract IEnumerable GetTestCases();
    }
}
