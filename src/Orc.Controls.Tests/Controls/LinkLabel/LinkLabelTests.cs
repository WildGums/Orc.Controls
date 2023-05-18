namespace Orc.Controls.Tests;

using System;
using System.Threading;
using System.Windows;
using Catel.IoC;
using Catel.Services;
using Moq;
using NUnit.Framework;

[TestFixture]
[Apartment(ApartmentState.STA)]
public class LinkLabelTests
{
    [TestCase("catelproject.com", UriKind.Relative, "https://catelproject.com/")]
    [TestCase("myapp://myaction", UriKind.Absolute, "myapp://myaction")]
    [TestCase("С:\\my\\long\\path\\to\\exe\\file\\exe.exe", UriKind.Absolute, "С:\\my\\long\\path\\to\\exe\\file\\exe.exe")]
    public void Open_Correct_Uri(string uriPath, UriKind kind, string expectedUri)
    {
        //Prepare
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(x => x.StartProcess(It.IsAny<ProcessContext>(), It.IsAny<ProcessCompletedDelegate>()))
            .Callback<ProcessContext, ProcessCompletedDelegate>((c, d) =>
            {
                Assert.That(c.FileName, Is.EqualTo(expectedUri));
            });

        DependencyResolverManager.Default = new LinkLabelDependencyResolverManager
        {
            DependencyResolver = new LinkLabelDependencyResolver
            {
                ProcessService = processServiceMock.Object
            }
        };

        var linkLabel = new LinkLabel();
        linkLabel.Url = new Uri(uriPath, kind);
        linkLabel.ClickBehavior = LinkLabelClickBehavior.OpenUrlInBrowser;
        linkLabel.RaiseEvent(new RoutedEventArgs(LinkLabel.ClickEvent));
    }

    private class LinkLabelDependencyResolver : IDependencyResolver
    {
        public IProcessService ProcessService { get; set; }

        public object Resolve(Type type, object tag = null)
        {
            if (type == typeof(IProcessService))
            {
                return ProcessService;
            }

            return null;
        }

        public object[] ResolveMultiple(Type[] types, object tag = null) => null;
        public bool CanResolve(Type type, object tag = null) => true;
        public bool CanResolveMultiple(Type[] types) => false;
    }

    private class LinkLabelDependencyResolverManager : IDependencyResolverManager
    {
        public LinkLabelDependencyResolver DependencyResolver { get; set; }

        public void RegisterDependencyResolverForInstance(object instance, IDependencyResolver dependencyResolver)
        {
        }

        public IDependencyResolver GetDependencyResolverForInstance(object instance) => DependencyResolver;

        public void RegisterDependencyResolverForType(Type type, IDependencyResolver dependencyResolver)
        {
        }

        public IDependencyResolver GetDependencyResolverForType(Type type) => DependencyResolver;

        public IDependencyResolver DefaultDependencyResolver { get; set; }
    }
}
