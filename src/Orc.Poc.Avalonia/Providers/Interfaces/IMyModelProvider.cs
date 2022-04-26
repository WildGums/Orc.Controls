namespace Orc.Poc.Avalonia;

using System.Collections.Generic;

public interface IMyModelProvider
{
    IEnumerable<MyModel> GetMyModels();
}
