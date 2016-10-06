using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using PCLStorage;
using SQLite;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;

namespace TraceYourStackLibrary
{
    public class Class1
    {
        public static async void Test()
        {
            IFolder folder = await FileSystem.Current.LocalStorage.CreateFolderAsync("TraceYourStackLibrary", CreationCollisionOption.OpenIfExists);
            IFile file = await folder.CreateFileAsync("TraceYourStackLibraryQueue.db", CreationCollisionOption.OpenIfExists);

            
        }
    }
}
