using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;

namespace Community.CsharpSqlite
{
    public static class File
    {
        public static FileStream Create(string name)
        {
            return new FileStream(name, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 0);
        }

        public static FileStream Open(string name, FileMode mode, FileAccess access, FileShare share )
        {
            return new FileStream(name, mode, access, share, 0);
        }

        public static void Delete(string name)
        {
            // When Csharp-sqlite attempt to delete the file - it does not close the file stream...
            IsolatedStorageFileStream handler = null;
            if (FileStream.HandleTracker.TryGetValue(name, out handler))
            {
                handler.Close();
                handler.Dispose();

                FileStream.HandleTracker.Remove(name);
            }

            int retry = 0;
            while (retry < 10)
            {
                try
                {
                    IsolatedStorageIO.Default.DeleteFile(name);
                    return;
                }
                catch (Exception)
                {
                    retry++;
                    Thread.Sleep(100);
                    continue;
                }
            
            }

            throw new InvalidOperationException("Cannot delete file");
        }

        

        public static bool Exists(string name)
        {
            return IsolatedStorageIO.Default.FileExists(name);
        }
    }
}
