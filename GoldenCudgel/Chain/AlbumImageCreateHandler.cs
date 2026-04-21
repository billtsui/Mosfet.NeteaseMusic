// --------------------------------------------------------------------------
// Copyright (c) 2026 Bill Tsui. All rights reserved.
// Licensed under the GPLv3 License.
// 
// File: AlbumImageCreateHandler.cs
// Author: Bill Tsui
// Date: 04 21, 2026 18:04
// Description:
// --------------------------------------------------------------------------

using GoldenCudgel.Entities;

namespace GoldenCudgel.Chain;

public class AlbumImageCreateHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, byte[] shareArray, NcmObject ncmObject)
    {
        //flac
        var fileIdentifier = "66-4C-61-43";

        var fileType = BitConverter.ToString(shareArray[ncmObject.AlbumImageLength..4]);
        if (fileIdentifier.Equals(fileType))
        {
            
        }

        base.Handle(file, fs, shareArray, ncmObject);
    }
}