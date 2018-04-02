//  ****************************************************************************
//  Ranplan Wireless Network Design Ltd.
//  __________________
//   All Rights Reserved. [2018]
//  
//  NOTICE:
//  All information contained herein is, and remains the property of
//  Ranplan Wireless Network Design Ltd. and its suppliers, if any.
//  The intellectual and technical concepts contained herein are proprietary
//  to Ranplan Wireless Network Design Ltd. and its suppliers and may be
//  covered by U.S. and Foreign Patents, patents in process, and are protected
//  by trade secret or copyright law.
//  Dissemination of this information or reproduction of this material
//  is strictly forbidden unless prior written permission is obtained
//  from Ranplan Wireless Network Design Ltd.
// ****************************************************************************

using System.Collections.Generic;
using NUnit.Framework;
using SharpFileSystem.FileSystems;
using SharpFileSystem.IO;
using SharpFileSystem.SharpZipLib;

namespace SharpFileSystem.Tests
{
    [TestFixture]
    public class GetStart
    {
        private static void WriteContents(IFileSystem fileSystem)
        {
            var rootPath = FileSystemPath.Parse("/_doc.kml");
            var buildingB1Path = FileSystemPath.Parse("/files/B1_Building.kml");
            var buildingB1F1Path = FileSystemPath.Parse("/_BuildingFiles/B1_F1.kml");
            var buildingB1F2Path = FileSystemPath.Parse("/_BuildingFiles/B1_F2.kml");
            var buildingOutdoorPath = FileSystemPath.Parse("/files/OutdoorLayout_Building.kml");

            var b1 = rootPath.ParentPath.AppendPath(buildingB1Path);
            var f1 = b1.ParentPath.AppendPath(buildingB1F1Path);
            var f2 = b1.ParentPath.AppendPath(buildingB1F2Path);
            var b2 = rootPath.ParentPath.AppendPath(buildingOutdoorPath);

            var list = new List<FileSystemPath> {rootPath, b1, f1, f2, b2};
            foreach (var path in list)
                using (var stream = fileSystem.CreateFile(path))
                {
                    stream.Write(new byte[] {1, 2, 3, 4});
                }
        }

        [Test]
        public void MyTest_PhysicalFileSystem()
        {
            using (var fileSystem = new PhysicalFileSystem(@"D:\workPrj\aaa"))
            {
                WriteContents(fileSystem);
            }
        }

        [Test]
        public void MyTest_SharpZipLibFileSystem()
        {
            using (var fileStream = System.IO.File.Create(@"D:\workPrj\aa.zip"))
            using (var fileSystem = SharpZipLibFileSystem.Create(fileStream))
            {
                WriteContents(fileSystem);
            }
        }
    }
}
