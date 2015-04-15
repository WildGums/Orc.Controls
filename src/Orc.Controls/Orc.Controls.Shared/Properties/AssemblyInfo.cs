// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyInfo.cs" company="Simulation Modelling Services">
//   Copyright (c) 2008 - 2014 Simulation Modelling Services. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System.Reflection;
using System.Windows;
using System.Windows.Markup;

[assembly: AssemblyTitle("Orc.Controls")]
[assembly: AssemblyProduct("Orc.Controls")]
[assembly: AssemblyDescription("Orc.Controls")]

[assembly: XmlnsPrefix("http://www.wildgums.net.au/orc", "orc")]
//[assembly: XmlnsDefinition("http://www.wildgums.net.au/orc", "Orc.Behaviors")]
//[assembly: XmlnsDefinition("http://www.wildgums.net.au/orc", "Orc.Converters")]
[assembly: XmlnsDefinition("http://www.wildgums.net.au/orc", "Orc.Controls")]
//[assembly: XmlnsDefinition("http://www.wildgums.net.au/orc", "Orc.Fonts")]
//[assembly: XmlnsDefinition("http://www.wildgums.net.au/orc", "Orc.Markup")]
//[assembly: XmlnsDefinition("http://www.wildgums.net.au/orc", "Orc.Views")]
//[assembly: XmlnsDefinition("http://www.wildgums.net.au/orc", "Orc.Windows")]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
    //(used if a resource is not found in the page, 
    // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
    //(used if a resource is not found in the page, 
    // app, or any theme specific resource dictionaries)
    )]