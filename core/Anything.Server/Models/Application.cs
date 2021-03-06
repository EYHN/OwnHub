﻿using System.Linq;
using System.Threading.Tasks;
using Anything.FileSystem;
using Anything.FileSystem.Exception;
using Anything.Preview;
using Anything.Search;
using Anything.Search.Properties;
using Anything.Search.Query;
using Anything.Utils;
using Microsoft.Extensions.Configuration;

namespace Anything.Server.Models
{
    public class Application
    {
        public Application(
            IConfiguration configuration,
            IFileService fileService,
            IPreviewService previewService,
            ISearchService searchService)
        {
            Configuration = configuration;
            FileService = fileService;
            PreviewService = previewService;
            SearchService = searchService;
        }

        public IConfiguration Configuration { get; }

        public IFileService FileService { get; }

        public IPreviewService PreviewService { get; }

        public ISearchService SearchService { get; }

        public async ValueTask<Directory> OpenDirectory(Url url)
        {
            var stats = await FileService.Stat(url);

            if (!stats.Type.HasFlag(FileType.Directory))
            {
                throw new FileNotADirectoryException(url);
            }

            return (CreateFile(url, stats) as Directory)!;
        }

        public async ValueTask<File> Open(Url url)
        {
            var stats = await FileService.Stat(url);

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

        public async ValueTask<File[]> Search(string q, Url? baseUrl)
        {
            var result = await SearchService.Search(new SearchOptions(new TextSearchQuery(SearchProperty.FileName, q), baseUrl));
            return await Task.WhenAll(result.Nodes.Select(async node => CreateFile(node.Url, await FileService.Stat(node.Url))));
        }
    }
}
