﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Anything.FileSystem.Exception;
using Anything.Utils;

namespace Anything.FileSystem.Provider
{
    public interface IFileSystemProvider
    {
        /// <summary>
        ///     Create a new directory.
        /// </summary>
        /// <param name="url">The uri of the new directory.</param>
        /// <exception cref="FileNotFoundException">parent of <paramref name="url" /> doesn't exist.</exception>
        /// <exception cref="FileExistsException">when <paramref name="url" /> already exists.</exception>
        /// <exception cref="NoPermissionsException">permissions aren't sufficient.</exception>
        public ValueTask CreateDirectory(Url url);

        /// <summary>
        ///     Removes a file or directory.
        /// </summary>
        /// <param name="url">The uri of the file or directory that is to be deleted.</param>
        /// <param name="recursive">Remove directories and their contents recursively.</param>
        /// <exception cref="FileNotFoundException"><paramref name="url" /> doesn't exist.</exception>
        /// <exception cref="FileIsADirectoryException"><paramref name="url" /> is a directory and <paramref name="recursive" /> is false.</exception>
        /// <exception cref="NoPermissionsException">permissions aren't sufficient.</exception>
        public ValueTask Delete(Url url, bool recursive);

        /// <summary>
        ///     Retrieve all entries of a directory.
        /// </summary>
        /// <param name="url">The uri of the directory.</param>
        /// <exception cref="FileNotFoundException"><paramref name="url" /> doesn't exist.</exception>
        /// <exception cref="FileNotADirectoryException"><paramref name="url" /> is not a directory.</exception>
        /// <exception cref="NoPermissionsException">permissions aren't sufficient.</exception>
        /// <returns>A task that resolves a collection of name/stats pair.</returns>
        public ValueTask<IEnumerable<(string Name, FileStats Stats)>> ReadDirectory(Url url);

        /// <summary>
        ///     Read the entire contents of a file.
        /// </summary>
        /// <param name="url">The uri of the file.</param>
        /// <exception cref="FileNotFoundException"><paramref name="url" /> doesn't exist.</exception>
        /// <exception cref="FileIsADirectoryException"><paramref name="url" /> is a directory.</exception>
        /// <exception cref="NoPermissionsException">permissions aren't sufficient.</exception>
        /// <returns>A task that resolves an array of bytes.</returns>
        public ValueTask<byte[]> ReadFile(Url url);

        /// <summary>
        ///     Copy a file or directory. The directory can have contents.
        ///     Note that the rename operation may modify the modification and creation times, timestamp behavior depends on the implementation.
        /// </summary>
        /// <param name="oldUrl">The existing file.</param>
        /// <param name="newUrl">The new location.</param>
        /// <param name="overwrite">Overwrite existing files.</param>
        /// <exception cref="FileNotFoundException"><paramref name="oldUrl" /> or parent of <paramref name="newUrl" /> doesn't exist.</exception>
        /// <exception cref="FileExistsException"><paramref name="newUrl" /> exists and <paramref name="overwrite" /> is false.</exception>
        /// <exception cref="NoPermissionsException">permissions aren't sufficient.</exception>
        public ValueTask Rename(Url oldUrl, Url newUrl, bool overwrite);

        /// <summary>
        ///     Retrieve stats about a file or directory.
        ///     Note that the stats for symbolic links should be the stats of the file they refer to.
        /// </summary>
        /// <param name="url">The uri of the file or directory to retrieve metadata about.</param>
        /// <exception cref="FileNotFoundException"><paramref name="url" /> doesn't exist.</exception>
        /// <exception cref="NoPermissionsException">permissions aren't sufficient.</exception>
        public ValueTask<FileStats> Stat(Url url);

        /// <summary>
        ///     Write data to a file, replacing its entire contents.
        /// </summary>
        /// <param name="url">The uri of the file.</param>
        /// <param name="content">The new content of the file.</param>
        /// <param name="create">Create when the file does not exist.</param>
        /// <param name="overwrite">When the file exists, overwrite its entire contents.</param>
        /// <exception cref="FileNotFoundException"><paramref name="url" /> doesn't exist and <paramref name="create" /> is false.</exception>
        /// <exception cref="FileExistsException"><paramref name="url" /> exists and <paramref name="overwrite" /> is false.</exception>
        /// <exception cref="FileIsADirectoryException"><paramref name="url" /> is a directory.</exception>
        /// <exception cref="NoPermissionsException">permissions aren't sufficient.</exception>
        public ValueTask WriteFile(Url url, byte[] content, bool create = true, bool overwrite = true);
    }
}
