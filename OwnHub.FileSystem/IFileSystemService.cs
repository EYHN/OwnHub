﻿using System;
using System.Threading.Tasks;
using OwnHub.FileSystem.Exception;
using OwnHub.FileSystem.Provider;
using OwnHub.Utils;

namespace OwnHub.FileSystem
{
    public interface IFileSystemService : IFileSystemProvider
    {
        /// <summary>
        /// Copy a file or directory.
        /// Note that the copy operation may modify the modification and creation times, timestamp behavior depends on the implementation.
        /// </summary>
        /// <param name="source">The existing file location.</param>
        /// <param name="destination">The destination location.</param>
        /// <param name="overwrite">Overwrite existing files.</param>
        /// <exception cref="FileNotFoundException"><paramref name="source"/> or parent of <paramref name="destination"/> doesn't exist.</exception>
        /// <exception cref="FileExistsException">files exists and <paramref name="overwrite"/> is false.</exception>
        /// <exception cref="NoPermissionsException">permissions aren't sufficient.</exception>
        public ValueTask Copy(Url source, Url destination, bool overwrite);

        public ValueTask AttachMetadata(Url url, FileMetadata metadata);

        public ValueTask<FileMetadata[]> GetMetadata(Url url);

        public event Action<FileChangeEvent[]> OnFileChange;
    }
}
