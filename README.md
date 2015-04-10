# Orc.Controls

The library of UI controls, which help you create user friendly interface.

## Content of library

* BindableRichTextBox
* DateTimePicker
* DropDownButton
* FilterBox
* LogViewer
* TimeSpan

### BindableRichTextBox

The control, used as well as regular *System.Windows.Controls.RichTextBox* but have one advantage. It could be used for binding *FlowDocument*. It is very useful when you're depeloping you application using MVVM pattern.

For binding data to the *BindableRichTextBox*, use property *BindableDocument*:

    <orc:BindableRichTextBox BindableDocument="{Binding FlowDoc}" />

after that you can manage your `FlowDoc` property of type `FlowDocument` as you want. You may assign new value to it or change the layout of FlowDocument. Everything will be displayed in your UI.

### DateTimePicker

Advanced DateTimePicker control.

![DateTimePicker 01](doc/images/DateTimePicker%2001.png) ![DateTimePicker 02](doc/images/DateTimePicker%2002.png)

![DateTimePicker 03](doc/images/DateTimePicker%2003.png) ![DateTimePicker 04](doc/images/DateTimePicker%2004.png) 

![DateTimePicker 05](doc/images/DateTimePicker%2005.png)

#### How to use it

Just put it in your .cs.xaml file in correct place. And use property *Value* for binding your *DateTime* value:

    <orc:DateTimePickerControl Value="{Binding DateTimeValue}" />

### DropDownButton

The control wich consists of two buttont. Fitsr one is works as ragular Button and the second one allows you to show customisable drop down menu under the control. 

![DropDownButton 01](doc/images/DropDownButton%2001.png)

#### How to use it

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

### FilterBox

Looks like regular TextBox. But when you're starting to type the text into it, you can see the drop down list with the possible variants of text which you've started to write. And the list is dynamically changing while you're typing your text.

#### How to use it

The control has two main bindable properties which can be used to configure the behavior:

* **FilterSource** => caption of Button
* **PropertyName** => used for configure default action
* **Text** => the ContextMenu
 

### LogViewer

The control for displaying the list of Catel.Logging.LogEntry


### TimeSpan


THe control for displaying and edinig values of type TimeSpan
