﻿using System.Threading.Tasks;
using NUnit.Framework;
using StagingBox.Preview.Icons;
using StagingBox.Preview.Icons.Renderers;

namespace StagingBox.Tests.Preview.Icons.Renderers
{
    [TestFixture]
    public class TextFileRendererTests
    {
        private static async Task RenderTestTextResourceIcon(
            string resourceName,
            TextFileRenderer renderer,
            IconsRenderContext renderContext)
        {
            var file = TestUtils.OpenResourceRegularFile(resourceName);
            await renderer.Render(
                renderContext,
                new DynamicIconsRenderInfo(file));
        }

        [Test]
        public async Task TestRenderTextIcon()
        {
            var renderContext = new IconsRenderContext();
            var renderer = new TextFileRenderer();

            renderContext.Resize(1024, 1024, false);
            await RenderTestTextResourceIcon("Test Text.txt", renderer, renderContext);
            await renderContext.SaveTestResult("1024w");

            renderContext.Resize(512, 512, false);
            await RenderTestTextResourceIcon("Test Text.txt", renderer, renderContext);
            await renderContext.SaveTestResult("512w");

            renderContext.Resize(256, 256, false);
            await RenderTestTextResourceIcon("Test Text.txt", renderer, renderContext);
            await renderContext.SaveTestResult("256w");

            renderContext.Resize(128, 128, false);
            await RenderTestTextResourceIcon("Test Text.txt", renderer, renderContext);
            await renderContext.SaveTestResult("128w");

            renderContext.Resize(64, 64, false);
            await RenderTestTextResourceIcon("Test Text.txt", renderer, renderContext);
            await renderContext.SaveTestResult("64w");
        }
    }
}