namespace Orc.Controls;

using System.Collections.Generic;
using System.Threading.Tasks;

public interface IApplicationLogFilterGroupService
{
    Task<IReadOnlyList<LogFilterGroup>> LoadAsync();

    Task SaveAsync(IReadOnlyList<LogFilterGroup> filterGroups);
}
