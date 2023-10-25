using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Markup;

// All other assembly info is defined in SolutionAssemblyInfo.cs

[assembly: AssemblyTitle("Orc.Controls")]
[assembly: AssemblyProduct("Orc.Controls")]
[assembly: AssemblyDescription("Orc.Controls library")]
[assembly: NeutralResourcesLanguage("en-US")]

[assembly: XmlnsPrefix("http://schemas.wildgums.com/orc/controls", "orccontrols")]
[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/controls", "Orc.Controls")]
[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/controls", "Orc.Controls.Converters")]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page, 
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page, 
                                              // app, or any theme specific resource dictionaries)
    )]

[assembly: InternalsVisibleTo("Orc.Controls.Tests")]
