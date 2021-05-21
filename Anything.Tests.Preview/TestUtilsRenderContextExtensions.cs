﻿using System.Threading.Tasks;
using Anything.Preview;
using NUnit.Framework;

namespace Anything.Tests.Preview
{
    internal static class TestUtilsRenderContextExtensions
    {
        public static async Task SaveTestResult(this RenderContext renderContext, string name)
        {
            var resultName = name + ".png";

            await using var pngStream = renderContext.SnapshotPng().AsStream();

            await TestUtils.SaveResult(
                resultName,
                pngStream);
        }
    }
}