namespace Orc.Poc.Avalonia;

using System;
using System.Collections.Generic;

public class MyModelProvider : IMyModelProvider
{
    public IEnumerable<MyModel> GetMyModels()
    {
        yield return new MyModel { Name = "Name", SecondName = "Second name", TestDate = new DateTime(1, 1, 1)};
        yield return new MyModel { Name = "Name 1", SecondName = "Second name 1", TestDate = new DateTime(2, 2, 2)};
        yield return new MyModel { Name = "Name 2", SecondName = "Second name 2", TestDate = new DateTime(3, 3, 3)};
    }
}
