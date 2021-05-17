﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using OwnHub.FileSystem.Exception;
using OwnHub.Utils;
using FileNotFoundException = OwnHub.FileSystem.Exception.FileNotFoundException;

namespace OwnHub.FileSystem.Provider
{
    /// <summary>
    /// File system provider, providing files from an <see cref="EmbeddedFileProvider"/>.
    /// </summary>
    public class EmbeddedFileSystemProvider : IFileSystemProviderSupportStream
    {
        private readonly EmbeddedFileProvider _embeddedFileProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedFileSystemProvider"/> class.
        /// </summary>
        /// <param name="embeddedFileProvider">The embedded file provider associated with this.</param>
        public EmbeddedFileSystemProvider(EmbeddedFileProvider embeddedFileProvider)
        {
            _embeddedFileProvider = embeddedFileProvider;
        }

        /// <inheritdoc/>
        public ValueTask CreateDirectory(Url url)
        {
            throw new NoPermissionsException(url);
        }

        /// <inheritdoc/>
        public ValueTask Delete(Url url, bool recursive)
        {
            throw new NoPermissionsException(url);
        }

        /// <inheritdoc/>
        public ValueTask<IEnumerable<(string Name, FileStats Stats)>> ReadDirectory(Url url)
        {
            var contents = _embeddedFileProvider.GetDirectoryContents(url.Path);
            if (!contents.Exists)
            {
                throw new FileNotFoundException(url);
            }

            return ValueTask.FromResult(
                contents.Select(
                    content =>
                    {
                        var fileType = content.IsDirectory ? FileType.Directory : FileType.File;
                        var fileSize = content.IsDirectory ? 0 : content.Length;
                        return (content.Name, new FileStats(content.LastModified, content.LastModified, fileSize, fileType));
                    }));
        }

        /// <inheritdoc/>
        public async ValueTask<byte[]> ReadFile(Url url)
        {
            var fileInfo = _embeddedFileProvider.GetFileInfo(url.Path);
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException(url);
            }

            if (fileInfo.IsDirectory)
            {
                throw new FileIsADirectoryException(url);
            }

            await using var memoryStream = new MemoryStream();
            await fileInfo.CreateReadStream().CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        /// <inheritdoc/>
        public ValueTask Rename(Url oldUrl, Url newUrl, bool overwrite)
        {
            throw new NoPermissionsException(oldUrl);
        }

        /// <inheritdoc/>
        public ValueTask<FileStats> Stat(Url url)
        {
            var fileInfo = _embeddedFileProvider.GetFileInfo(url.Path);
            var fileType = fileInfo.IsDirectory ? FileType.Directory : FileType.File;
            var fileSize = fileInfo.IsDirectory ? 0 : fileInfo.Length;
            return ValueTask.FromResult(new FileStats(fileInfo.LastModified, fileInfo.LastModified, fileSize, fileType));
        }

        /// <inheritdoc/>
        public ValueTask WriteFile(Url url, byte[] content, bool create = true, bool overwrite = true)
        {
            throw new NoPermissionsException(url);
        }

        public ValueTask<Stream> OpenReadFileStream(Url url)
        {
            var fileInfo = _embeddedFileProvider.GetFileInfo(url.Path);
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException(url);
            }

            if (fileInfo.IsDirectory)
            {
                throw new FileIsADirectoryException(url);
            }

            return ValueTask.FromResult(fileInfo.CreateReadStream());
        }
    }
}