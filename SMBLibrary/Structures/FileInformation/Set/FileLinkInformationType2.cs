/* Copyright (C) 2014-2017 Tal Aloni <tal.aloni.il@gmail.com>. All rights reserved.
 * 
 * You can redistribute this program and/or modify it under the terms of
 * the GNU Lesser Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 */
using System;
using System.Collections.Generic;
using System.Text;
using Utilities;

namespace SMBLibrary
{
    /// <summary>
    /// [MS-FSCC] 2.4.21.2 - FileLinkInformation Type 2
    /// </summary>
    public class FileLinkInformationType2 : FileInformation
    {
        public const int FixedLength = 20;

        public bool ReplaceIfExists;
        // 7 reserved bytes
        public ulong RootDirectory;
        private uint FileNameLength;
        public string FileName = String.Empty;

        public FileLinkInformationType2()
        {
        }

        public FileLinkInformationType2(byte[] buffer, int offset)
        {
            ReplaceIfExists = Conversion.ToBoolean(ByteReader.ReadByte(buffer, offset + 0));
            RootDirectory = LittleEndianConverter.ToUInt64(buffer, offset + 8);
            FileNameLength = LittleEndianConverter.ToUInt32(buffer, offset + 16);
            FileName = ByteReader.ReadUTF16String(buffer, offset + 20, (int)FileNameLength / 2);
        }

        public override void WriteBytes(byte[] buffer, int offset)
        {
            FileNameLength = (uint)(FileName.Length * 2);
            ByteWriter.WriteByte(buffer, offset + 0, Convert.ToByte(ReplaceIfExists));
            LittleEndianWriter.WriteUInt64(buffer, offset + 8, RootDirectory);
            LittleEndianWriter.WriteUInt32(buffer, offset + 16, FileNameLength);
            ByteWriter.WriteUTF16String(buffer, offset + 20, FileName);
        }

        public override FileInformationClass FileInformationClass
        {
            get
            {
                return FileInformationClass.FileLinkInformation;
            }
        }

        public override int Length
        {
            get
            {
                return FixedLength + FileName.Length * 2;
            }
        }
    }
}
