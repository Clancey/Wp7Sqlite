using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;

namespace Community.CsharpSqlite
{
    /// <summary>
    /// Wrapper for IsolatedStorageFileStream
    /// </summary>
    public class FileStream : Stream
    {
        public static Dictionary<string, IsolatedStorageFileStream> HandleTracker = new Dictionary<string, IsolatedStorageFileStream>();

        private IsolatedStorageFileStream _internal;

        public FileStream(string path, FileMode mode, FileAccess access, FileShare share, int unused)
        {
            IsolatedStorageFileStream handler = null;
            if (FileStream.HandleTracker.TryGetValue(path, out handler))
            {
                _internal = handler;
            }
            else
            {
                if (mode == FileMode.Create || mode == FileMode.CreateNew)
                {
                    _internal = IsolatedStorageIO.Default.CreateFile(path);
                }
                else
                {
                    _internal = IsolatedStorageIO.Default.OpenFile(path, FileMode.OpenOrCreate);
                }
                FileStream.HandleTracker.Add(path, _internal);
            }
        }

        public override bool CanRead
        {
            get { return _internal.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _internal.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _internal.CanWrite; }
        }

        public override void Flush()
        {
            _internal.Flush();
        }

        public override long Length
        {
            get { return _internal.Length; }
        }

        public override long Position
        {
            get
            {
                return _internal.Position;
            }
            set
            {
                _internal.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _internal.Read( buffer, offset, count );
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _internal.Seek( offset, origin);
        }

        public override void SetLength(long value)
        {
            _internal.SetLength( value );
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _internal.Write( buffer, offset, count );
        }
    }
}
