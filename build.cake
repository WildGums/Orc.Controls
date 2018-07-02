// Define the required parameters
var DefaultSolutionName = "Orc.Controls";
var DefaultCompany = "WildGums";
var DefaultRepositoryUrl = string.Format("https://github.com/{0}/{1}", DefaultCompany, DefaultSolutionName);
var StartYear = 2014;

// Note: the rest of the variables should be coming from the build server,
// see `/deployment/cake/*-variables.cake` for customization options

//=======================================================

// Components

var ComponentsToBuild = new string[]
{
    "Orc.Controls"
};

//=======================================================

// WPF apps

var WpfAppsToBuild = new string[]
{

};

//=======================================================

// UWP apps

var UwpAppsToBuild = new string[]
{

};

//=======================================================

// Test projects

var TestProjectsToBuild = new string[]
{
    "Orc.Controls.Tests"
};

//=======================================================

// Now all variables are defined, include the tasks, that
// script will take care of the rest of the magic

#l "./deployment/cake/tasks.cake"