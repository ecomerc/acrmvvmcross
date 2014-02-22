﻿using System;
using System.IO;
#if __ANDROID
using Android.Webkit;
#endif

namespace Acr.MvvmCross.Plugins.Storage.Impl {
    
    public class FileInfoImpl : IFileInfo {
        private readonly FileInfo file;
        private readonly Lazy<IDirectoryInfo> directory; 


        public FileInfoImpl(FileInfo file) {
            this.file = file;
            this.directory = new Lazy<IDirectoryInfo>(() => new DirectoryInfoImpl(file.Directory));
        }


        #region IFileInfo Members

        public string Name {
            get { return this.file.Name; }
        }


        public string Extension {
            get { return this.file.Extension; }
        }


        public string FullName {
            get { return this.file.FullName; }
        }


        private string mimeType;
        public string MimeType {

            get {
                if (this.mimeType == null) {
#if __ANDROID
                    this.mimeType = MimeTypeMap.Singleton.GetMimeTypeFromExtension(this.Extension);
#elif __IOS__
                    this.mimeType = this.Extension; // TODO: not implemented for ios - needed?
#endif
                }
                return this.mimeType;
            }

        }

        public bool Exists {
            get { return this.file.Exists; }
        }


        public long Length {
            get { return this.file.Length; }
        }


        public IDirectoryInfo Directory {
            get { return this.directory.Value; }
        }


        public DateTime LastWriteTime {
            get { return this.file.LastAccessTime; }
        }


        public DateTime LastWriteTimeUtc {
            get { return this.file.LastWriteTimeUtc; }
        }


        public DateTime LastAccessTime {
            get { return this.file.LastAccessTime; }
        }


        public DateTime LastAccessTimeUtc {
            get { return this.file.LastAccessTimeUtc; }
        }


        public void Delete() {
            this.file.Delete();
        }


        public Stream Create() {
            return this.file.Create();
        }


        public Stream OpenRead() {
            return this.file.OpenRead();
        }


        public Stream OpenWrite() {
            return this.file.OpenWrite();
        }

        #endregion
    }
}