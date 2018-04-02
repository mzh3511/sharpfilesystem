using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using SevenZip;
using SharpFileSystem.IO;

namespace SharpFileSystem.SevenZip
{
    public class SevenZipFileSystem : IFileSystem
    {
        private readonly ICollection<FileSystemPath> _entities = new List<FileSystemPath>();
        private readonly SevenZipExtractor _extractor;

        private SevenZipFileSystem(SevenZipExtractor extractor)
        {
            _extractor = extractor;
            foreach (var file in _extractor.ArchiveFileData)
                AddEntity(GetVirtualFilePath(file));
        }

        public SevenZipFileSystem(Stream stream)
            : this(new SevenZipExtractor(stream))
        {
        }

        public SevenZipFileSystem(string physicalPath)
            : this(new SevenZipExtractor(physicalPath))
        {
        }

        public ICollection<FileSystemPath> GetEntities(FileSystemPath path)
        {
            if (!path.IsDirectory)
                throw new ArgumentException("The specified path is not a directory.", nameof(path));
            return _entities.Where(p => !p.IsRoot && p.ParentPath.Equals(path)).ToArray();
        }

        public bool Exists(FileSystemPath path)
        {
            return _entities.Contains(path);
        }

        public Stream CreateFile(FileSystemPath path)
        {
            throw new NotSupportedException();
        }

        public Stream OpenFile(FileSystemPath path, FileAccess access)
        {
            if (access == FileAccess.Write)
                throw new NotSupportedException();

            Stream s = new ProducerConsumerStream();
            ThreadPool.QueueUserWorkItem(delegate
            {
                _extractor.ExtractFile(GetSevenZipPath(path), s);
                s.Close();
            });
            return s;
        }

        public void CreateDirectory(FileSystemPath path)
        {
            throw new NotSupportedException();
        }

        public void Delete(FileSystemPath path)
        {
            throw new NotSupportedException();
        }

        public void Dispose()
        {
            _extractor.Dispose();
        }

        public void AddEntity(FileSystemPath path)
        {
            if (!_entities.Contains(path))
                _entities.Add(path);
            if (!path.IsRoot)
                AddEntity(path.ParentPath);
        }

        public string GetSevenZipPath(FileSystemPath path)
        {
            return path.ToString().Remove(0, 1);
        }

        public FileSystemPath GetVirtualFilePath(ArchiveFileInfo archiveFile)
        {
            var path = FileSystemPath.DirectorySeparator +
                       archiveFile.FileName.Replace(Path.DirectorySeparatorChar, FileSystemPath.DirectorySeparator);
            if (archiveFile.IsDirectory && path[path.Length - 1] != FileSystemPath.DirectorySeparator)
                path += FileSystemPath.DirectorySeparator;
            return FileSystemPath.Parse(path);
        }
    }
}
