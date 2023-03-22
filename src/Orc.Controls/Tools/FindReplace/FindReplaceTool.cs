namespace Orc.Controls;

using System;
using System.Threading.Tasks;
using Catel.IoC;
using Catel.Logging;
using Catel.MVVM;
using Catel.Services;
using Services;
using ViewModels;

public class FindReplaceTool<TFindReplaceService> : ControlToolBase
    where TFindReplaceService : IFindReplaceService
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private readonly FindReplaceSettings _findReplaceSettings;
    private readonly IUIVisualizerService _uiVisualizerService;
    private readonly ITypeFactory _typeFactory;
    private readonly IServiceLocator _serviceLocator;

    private IFindReplaceService? _findReplaceService;
    private FindReplaceViewModel? _findReplaceViewModel;

    public FindReplaceTool(IUIVisualizerService uiVisualizerService, ITypeFactory typeFactory, IServiceLocator serviceLocator)
    {
        ArgumentNullException.ThrowIfNull(uiVisualizerService);
        ArgumentNullException.ThrowIfNull(typeFactory);
        ArgumentNullException.ThrowIfNull(serviceLocator);

        _uiVisualizerService = uiVisualizerService;
        _typeFactory = typeFactory;
        _serviceLocator = serviceLocator;

        _findReplaceSettings = new FindReplaceSettings();
    }

    public override string Name => "FindReplaceTool";

    public override void Attach(object target)
    {
        ArgumentNullException.ThrowIfNull(target);

        base.Attach(target);

        _findReplaceService = _serviceLocator.ResolveType<IFindReplaceService>(target);
        if (_findReplaceService is not null)
        {
            return;
        }

        _findReplaceService = CreateFindReplaceService(target);
        if (_findReplaceService is null)
        {
            return;
        }

        _serviceLocator.RegisterInstance(_findReplaceService, target);
    }

    protected virtual TFindReplaceService? CreateFindReplaceService(object target)
    {
        return _typeFactory.CreateInstanceWithParametersAndAutoCompletion<TFindReplaceService>(target);
    }

    public override void Detach()
    {
        var target = Target;

        if (_serviceLocator.IsTypeRegistered<IFindReplaceService>(target))
        {
            _serviceLocator.RemoveType<IFindReplaceService>(target);
        }
    }

    protected override async void OnOpen(object? parameter = null)
    {
        if (_findReplaceService is null)
        {
            Log.Warning("Can't open find replace tool because FindReplaceService isn't initialized yet");
            return;
        }

        _findReplaceViewModel = new FindReplaceViewModel(_findReplaceSettings, _findReplaceService);

        await _uiVisualizerService.ShowAsync(_findReplaceViewModel);

        _findReplaceViewModel.ClosedAsync += OnClosedAsync;
    }

    public override void Close()
    {
        base.Close();

        if (_findReplaceViewModel is null)
        {
            return;
        }

        _findReplaceViewModel.ClosedAsync -= OnClosedAsync;

#pragma warning disable 4014
        _findReplaceViewModel.CloseViewModelAsync(null);
#pragma warning restore 4014
    }

    private Task OnClosedAsync(object? sender, ViewModelClosedEventArgs args)
    {
        Close();

        return Task.CompletedTask;
    }
}
