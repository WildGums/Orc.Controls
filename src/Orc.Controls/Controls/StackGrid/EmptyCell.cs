namespace Orc.Controls;

using System.Windows.Controls;

/// <summary>
/// Control to fill up a row in the <see cref="StackGrid"/> control. This control will use an entire row to fill up.
/// </summary>
/// <example>
/// <code>
/// <![CDATA[
/// <StackGrid>
///   <StackGrid.RowDefinitions>
///     <RowDefinition Height="Auto" />
///     <RowDefinition Height="*" />
///     <RowDefinition Height="Auto" />
///   </StackGrid.RowDefinitions>
/// 
///   <StackGrid.ColumnDefinitions>
///     <ColumnDefinition Width="Auto" />
///     <ColumnDefinition Width="*" />
///   </StackGrid.ColumnDefinitions>
/// 
///   <Label Content="Title" />
///   <EmptyCell />
/// 
///   <!-- Name, will be set to row 1, column 1 and 2 -->
///   <Label Content="Name" />
///   <TextBox Text="{Bindng Name}" />
///
///   <!-- Wrappanel, will span 2 columns -->
///   <WrapPanel StackGrid.ColumnSpan="2">
///     <Button Command="{Binding OK}" />
///   </WrapPanel>
/// </StackGrid>
/// ]]>
/// </code>
/// </example>
public class EmptyCell : Control
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmptyCell"/> class.
    /// </summary>
    public EmptyCell()
    {
        Focusable = false;

        Grid.SetRowSpan(this, 1);
        Grid.SetColumnSpan(this, 1);
    }
}