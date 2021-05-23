﻿using System.Linq;
using System.Threading.Tasks;
using Anything.FileSystem;

namespace Anything.Preview.Thumbnails.Renderers
{
    public abstract class BaseThumbnailsRenderer : IThumbnailsRenderer
    {
        /// <summary>
        ///     Gets the mimetype supported by the renderer.
        /// </summary>
        protected abstract string[] SupportMimeTypes { get; }

        async Task<bool> IThumbnailsRenderer.Render(ThumbnailsRenderContext ctx, ThumbnailsRenderOption option)
        {
            if (!IsSupported(option))
            {
                return false;
            }

            return await Render(ctx, option);
        }

        public virtual bool IsSupported(ThumbnailsRenderOption option)
        {
            if (option.FileType.HasFlag(FileType.File) && SupportMimeTypes.Contains(option.MimeType))
            {
                return true;
            }

            return false;
        }

        public abstract Task<bool> Render(ThumbnailsRenderContext ctx, ThumbnailsRenderOption option);
    }
}
