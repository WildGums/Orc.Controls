// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyInfo.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Markup;

// All other assembly info is defined in SolutionAssemblyInfo.cs

[assembly: AssemblyTitle("Orc.Automation")]
[assembly: AssemblyProduct("Orc.Automation")]
[assembly: AssemblyDescription("Orc.Automation library")]
[assembly: NeutralResourcesLanguage("en-US")]

[assembly: XmlnsPrefix("http://schemas.wildgums.com/orc/automation", "orcautomation")]
[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/automation", "Orc.Automation")]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page, 
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page, 
                                              // app, or any theme specific resource dictionaries)
    )]
