﻿using System.Threading.Tasks;
using NUnit.Framework;
using StagingBox.FileSystem;
using StagingBox.Preview.Thumbnails;
using StagingBox.Preview.Thumbnails.Renderers;
using StagingBox.Utils;

namespace StagingBox.Tests.Preview.Thumbnails.Renderers
{
    public class BaseThumbnailsRendererTests
    {
        private class TestRenderer : BaseThumbnailsRenderer
        {
            protected override string[] SupportMimeTypes { get; } = { "text/plain", "image/png" };

            public override Task<bool> Render(ThumbnailsRenderContext ctx, ThumbnailsRenderOption option)
            {
                Assert.AreEqual("image/png", option.MimeType);
                return Task.FromResult(true);
            }
        }

        [Test]
        public async Task ImplementationTest()
        {
            IThumbnailsRenderer testRenderer = new TestRenderer();
            Assert.IsTrue(
                testRenderer.IsSupported(
                    new ThumbnailsRenderOption(Url.Parse("file://test/a.txt")) { MimeType = "text/plain", FileType = FileType.File }));
            Assert.IsTrue(
                testRenderer.IsSupported(
                    new ThumbnailsRenderOption(Url.Parse("file://test/a.png")) { MimeType = "image/png", FileType = FileType.File }));
            Assert.IsFalse(
                testRenderer.IsSupported(
                    new ThumbnailsRenderOption(Url.Parse("file://test/a.jpg")) { MimeType = "image/jpeg", FileType = FileType.File }));

            var testRenderContext = new ThumbnailsRenderContext();

            Assert.IsTrue(
                await testRenderer.Render(
                    testRenderContext,
                    new ThumbnailsRenderOption(Url.Parse("file://test/a.png")) { MimeType = "image/png", FileType = FileType.File }));
            Assert.IsFalse(
                await testRenderer.Render(
                    testRenderContext,
                    new ThumbnailsRenderOption(Url.Parse("file://test/a.jpg")) { MimeType = "image/jpeg", FileType = FileType.File }));
        }
    }
}