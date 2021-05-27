﻿using System.Threading.Tasks;
using Anything.FileSystem;
using Anything.FileSystem.Exception;
using Anything.Preview;
using Anything.Utils;

namespace Anything.Server.Models
{
    public class Application
    {
        public Application(IFileSystemService fileSystemService, IPreviewService previewService)
        {
            FileSystemService = fileSystemService;
            PreviewService = previewService;
        }

        public IFileSystemService FileSystemService { get; }

        public IPreviewService PreviewService { get; }

        public async ValueTask<Directory> OpenDirectory(Url url)
        {
            var stats = await FileSystemService.Stat(url);

            if (!stats.Type.HasFlag(FileType.Directory))
            {
                throw new FileNotADirectoryException(url);
            }

            return (CreateFile(url, stats) as Directory)!;
        }

        public async ValueTask<File> Open(Url url)
        {
            var stats = await FileSystemService.Stat(url);

            return CreateFile(url, stats);
        }

        public File CreateFile(Url url, FileStats stats)
        {
            if (stats.Type.HasFlag(FileType.File))
            {
                return new RegularFile(this, url, stats);
            }

            if (stats.Type.HasFlag(FileType.Directory))
            {
                return new Directory(this, url, stats);
            }

            return new UnknownFile(this, url, stats);
        }
    }
}