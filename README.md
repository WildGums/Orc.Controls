# Orc.Controls

The library of UI controls, which help you create user friendly interface.

## Content of library

* **BindableRichTextBox**
* **DateTimePickerControl**
* **DropDownButton**
* **FilterBoxControl**
* **LogViewer**
* **TimeSpanControl**

### BindableRichTextBox

The control, used as well as regular [RichTextBox](https://msdn.microsoft.com/en-us/library/system.windows.controls.richtextbox(v=vs.110).aspx) but have one advantage. It could be used for binding [FlowDocument](https://msdn.microsoft.com/en-us/library/system.windows.documents.flowdocument(v=vs.110).aspx). It is very useful when you're depeloping you application using MVVM pattern.

##### How to use BindableRichTextBox

For binding data to the *BindableRichTextBox*, use property **BindableDocument** (the type is [FlowDocument](https://msdn.microsoft.com/en-us/library/system.windows.documents.flowdocument(v=vs.110).aspx)):

    <orc:BindableRichTextBox BindableDocument="{Binding FlowDoc}" />

after that you can manage binded property of type as you want. You may assign new value to it or change the layout of [FlowDocument](https://msdn.microsoft.com/en-us/library/system.windows.documents.flowdocument(v=vs.110).aspx). Everything will be displayed in your UI.

### DateTimePickerControl

Advanced DateTimePicker control.

![DateTimePicker 01](doc/images/DateTimePicker_01.png) ![DateTimePicker 02](doc/images/DateTimePicker_02.png)

![DateTimePicker 03](doc/images/DateTimePicker_03.png) ![DateTimePicker 04](doc/images/DateTimePicker_04.png) 

![DateTimePicker 05](doc/images/DateTimePicker_05.png)

##### How to use DateTimePickerControl

Just put it in your .cs.xaml file in correct place. And use property **Value** for binding your *DateTime* value:

    <orc:DateTimePickerControl Value="{Binding DateTimeValue}" />

### DropDownButton

The control wich consists of two buttont. Fitsr one is works as ragular Button and the second one allows you to show customisable drop down menu under the control. 

![DropDownButton 01](doc/images/DropDownButton_01.png)

##### How to use DropDownButton

The control has three main bindable properties which can be used to configure the DropDownButton behavior:

* **Header** => caption of Button
* **Command** => used for configure default action
* **DropDown** => the ContextMenu

Example:
    
    <orc:DropDownButton Header="Action" Command="{Binding DefaultAction}">
		<orc:DropDownButton.DropDown>
			<ContextMenu>
				<MenuItem Header="Item 1"/>
					<MenuItem Header="Item 2"/>
					<Separator/>
					<MenuItem Header="Item 2"/>
			</ContextMenu>
		</orc:DropDownButton.DropDown>
	</orc:DropDownButton>

### FilterBoxControl

Looks like regular TextBox. 

![FilterBox 01](doc/images/FilterBox_01.png)

But when you're starting to type the text into it, you can see the drop down list with the possible variants of text which you've started to write. And the list is dynamically changing while you're typing your text.


![FilterBox 02](doc/images/FilterBox_02.png)

If you'll click the 'x' button, the text will be removed

![FilterBox 03](doc/images/FilterBox_03.png)

##### How to use FilterBoxControl

The main properties to configure the behavior of FilterBox:

* **FilterSource** => The collection of items, which used to display string in drop down list
* **PropertyName** => The name of property of item from **FilterSource**, which is used for filtering
* **Text** => The text entered by user, which is also used as filter for getting values from **FilterSource**

Example:

    <orc:FilterBoxControl PropertyName="Value" 
						FilterSource="{Binding FilterSource}" 
						Text="{Binding FilterText}"/>
 

### LogViewer

The control for broadcsting log records of current application in real time. Control uses it's own LogListener, which is derived from [Catel.Logging.LogListenerBase](http://www.nudoq.org/#!/Packages/Catel.Core/Catel.Core/LogListenerBase). For displaying logrecods used [RichTextBox](https://msdn.microsoft.com/en-us/library/system.windows.controls.richtextbox(v=vs.110).aspx).

![LogViewer 03](doc/images/LogViewer_01.png)

##### How to use LogViewer

Here are the main properties, which is used to configure LogViewer:

Filtering: 

* **LogFilter** => string, which is used for filtering displayed log records 
* **ShowDebug** => boolean value, indicates of show/hide Debug level log records
* **ShowInfo** => boolean value, indicates of show/hide Info level log records
* **ShowWarning** => boolean value, indicates of show/hide Warning level log records
* **ShowError** => boolean value, indicates of show/hide Error level log records
 
Visualisation:

* **EnableTimestamp** => Enabling/disabling of showing timestamp for log records
* **EnableTextColoring** => Enabling/disabling of highlighting for log records depends of their level
* **EnableIcons** => Enabling/disabling of showing icons for log records depends of their level

Events:

* **LogEntryDoubleClick** => helps to handle double clicking on row of LogViewer's text


Example:
	<orc:LogViewerControl LogEntryDoubleClick="LogViewerControlOnLogRecordDoubleClick
				LogFilter="{Binding Text, ElementName=FilterTextBox}"
				ShowDebug="{Binding IsChecked, ElementName=ShowDebugToggleButton}"
				ShowInfo="{Binding IsChecked, ElementName=ShowInfoToggleButton}"
				ShowWarning="{Binding IsChecked, ElementName=ShowWarningToggleButton}"
				ShowError="{Binding IsChecked, ElementName=ShowErrorToggleButton}"
				EnableTimestamp="{Binding IsChecked, ElementName=EnableTimestampCheckBox}"
				EnableTextColoring="True" 
				EnableIcons="True"/>

### TimeSpanControl

THe control for displaying and editing values of type TimeSpan. In inactive mode it shows TimeSpan value in format *dd.hh:mm:ss*

![TimeSpan 01](doc/images/TimeSpan_01.png) ![TimeSpan 02](doc/images/TimeSpan_02.png)

on double click on any of symbol *d*, *h*, *m* or *s*, you can edit value in different measure units (days, hours, minutes, seconds)

![TimeSpan 03](doc/images/TimeSpan_03.png) ![TimeSpan 04](doc/images/TimeSpan_04.png) ![TimeSpan 05](doc/images/TimeSpan_05.png)


##### How to use TimeSpanControl

the usage of TimeSpanControl the same as usage of DateTimePickerControl, except one difference. the Value property is TimeSpan but not DateTime

Example:

	<orc:TimeSpanControl Value="{Binding TimeSpanValue}"/>


