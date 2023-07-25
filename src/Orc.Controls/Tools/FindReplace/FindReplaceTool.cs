namespace Orc.Controls;

using System;
using Catel.IoC;
using Catel.Logging;
using Catel.Services;
using Services;
using ViewModels;

public class FindReplaceTool<TFindReplaceService> : DialogWindowHostedToolBase<FindReplaceViewModel>
    where TFindReplaceService : IFindReplaceService
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private readonly FindReplaceSettings _findReplaceSettings;
    private readonly ITypeFactory _typeFactory;
    private readonly IServiceLocator _serviceLocator;

    private IFindReplaceService? _findReplaceService;

    public FindReplaceTool(IUIVisualizerService uiVisualizerService, ITypeFactory typeFactory, IServiceLocator serviceLocator)
        : base(uiVisualizerService)
    {
        ArgumentNullException.ThrowIfNull(typeFactory);
        ArgumentNullException.ThrowIfNull(serviceLocator);

        _typeFactory = typeFactory;
        _serviceLocator = serviceLocator;

        _findReplaceSettings = new FindReplaceSettings();
    }

    public override string Name => "FindReplaceTool";
    public override bool IsModal => false;

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

        base.Detach();
    }

    protected override void OnAccepted()
    {
        //Do nothing
    }

    protected override FindReplaceViewModel InitializeViewModel()
    {
        if (_findReplaceService is null)
        {
            throw Log.ErrorAndCreateException<Exception>("Can't open find replace tool because FindReplaceService isn't initialized yet");
        }

        return new FindReplaceViewModel(_findReplaceSettings, _findReplaceService);
    }
}
