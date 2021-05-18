﻿using System.Threading.Tasks;
using StagingBox.File;

namespace StagingBox.Preview.Icons
{
    public interface IDynamicIconsRenderer
    {
        public Task<bool> Render(IconsRenderContext ctx, DynamicIconsRenderInfo info);
        public bool IsSupported(IFile file);
    }

    public class DynamicIconsRenderInfo
    {
        public DynamicIconsRenderInfo(IFile file)
        {
            File = file;
        }

        public IFile File { get; }
    }
}