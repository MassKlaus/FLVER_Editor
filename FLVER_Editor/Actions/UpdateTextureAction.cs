﻿using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FLVER_Editor.Program;

namespace FLVER_Editor.Actions;

public class UpdateTextureAction : TransformAction
{
    private Action<string> windowRefresh;
    private readonly BND4 flverBnd;
    private readonly string textureFilePath;
    private readonly string oldfilename;
    private TPF.Texture? newTexture;
    private TPF.Texture? oldTexture;
    private TPF.Texture? replacedTexture;
    private int textureIndex;

    public UpdateTextureAction(BND4 flverBnd, string textureFilePath, string oldfilename, Action<string> refresher)
    {
        this.flverBnd = flverBnd;
        this.textureFilePath = textureFilePath;
        this.oldfilename = oldfilename;
        windowRefresh = refresher;
    }

    public override void Execute()
    {
        oldTexture = null;
        replacedTexture = null;

        if (Tpf == null) Tpf = new TPF();
        BinderFile? flverBndTpfEntry = flverBnd.Files.FirstOrDefault(i => i.Name.EndsWith(".tpf"));

        if (flverBndTpfEntry is not null)
        {
            byte[] ddsBytes = File.ReadAllBytes(textureFilePath);
            DDS dds = new(ddsBytes);
            byte formatByte = 107;
            try
            {
                formatByte = (byte)Enum.Parse(typeof(TextureFormats), dds.header10.dxgiFormat.ToString());
            }
            catch { }

            newTexture = new(Path.GetFileNameWithoutExtension(textureFilePath), formatByte, 0x00, File.ReadAllBytes(textureFilePath));
            textureIndex = Tpf.Textures.FindIndex(i => i.Name == newTexture.Name);

            var oldTextureIndex = Tpf.Textures.FindIndex(i => i.Name == oldfilename);

            if (oldTextureIndex != -1)
            {
                oldTexture = Tpf.Textures[oldTextureIndex];
            }

            if (textureIndex != -1)
            {
                replacedTexture = Tpf.Textures[textureIndex];

                Tpf.Textures.RemoveAt(textureIndex);
                Tpf.Textures.Insert(textureIndex, newTexture);
            }
            else Tpf.Textures.Add(newTexture);

            flverBnd.Files[flverBnd.Files.IndexOf(flverBndTpfEntry)].Bytes = Tpf.Write();
        }

        windowRefresh?.Invoke(textureFilePath);
    }

    public override void Undo()
    {
        if (Tpf == null) Tpf = new TPF();
        BinderFile? flverBndTpfEntry = flverBnd.Files.FirstOrDefault(i => i.Name.EndsWith(".tpf"));

        if (flverBndTpfEntry is null) return;


        if (oldTexture is null)
        {
            Tpf.Textures.Remove(newTexture);
        }
        else
        {
            Tpf.Textures.RemoveAt(textureIndex);
            Tpf.Textures.Insert(textureIndex, replacedTexture);
        }

        flverBnd.Files[flverBnd.Files.IndexOf(flverBndTpfEntry)].Bytes = Tpf.Write();

        windowRefresh?.Invoke(oldTexture?.Name ?? "");
    }
}
