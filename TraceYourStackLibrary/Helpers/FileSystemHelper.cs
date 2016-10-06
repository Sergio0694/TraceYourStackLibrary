using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using PCLStorage;

namespace TraceYourStackLibrary.Helpers
{
    internal static class FileSystemHelper
    {
        public static async Task<IFile> TryGetFileAsync([NotNull] String filename, [NotNull] String folderName)
        {
            IFolder folder = await FileSystem.Current.LocalStorage.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
            return await folder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
        }
    }
}
