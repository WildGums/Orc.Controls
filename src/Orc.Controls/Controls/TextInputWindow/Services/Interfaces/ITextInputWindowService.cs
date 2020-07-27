// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITextInputWindowService.cs" company="WildGums">
//   Copyright (c) 2008 - 2020 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Services
{
    using System.Threading.Tasks;

    public interface ITextInputWindowService
    {
        Task<TextInputDialogResult> ShowDialogAsync(string title, string initialText);
    }
}
