﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PublicApiFacts.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Tests
{
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using PublicApiGenerator;
    using VerifyNUnit;

    [TestFixture]
    public class PublicApiFacts
    {
        [Test, MethodImpl(MethodImplOptions.NoInlining)]
        public async Task Orc_Controls_HasNoBreakingChanges_Async()
        {
            var assembly = typeof(ResizingAdorner).Assembly;

            await PublicApiApprover.ApprovePublicApiAsync(assembly);
        }

        internal static class PublicApiApprover
        {
            public static async Task ApprovePublicApiAsync(Assembly assembly)
            {
                var publicApi = ApiGenerator.GeneratePublicApi(assembly, new ApiGeneratorOptions());
                await Verifier.Verify(publicApi);
            }
        }
    }
}
