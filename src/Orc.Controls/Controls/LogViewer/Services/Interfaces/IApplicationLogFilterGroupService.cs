namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IApplicationLogFilterGroupService
    {
        #region Methods
        Task<IEnumerable<LogFilterGroup>> LoadAsync();

        Task SaveAsync(IEnumerable<LogFilterGroup> filterGroups);
        #endregion
    }
}
