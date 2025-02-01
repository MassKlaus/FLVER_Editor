using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLVER_Editor.Actions;

public class UpdateTextureAction : TransformAction
{
    private Action<string> windowRefresh;
    private readonly TPF tpf;
    private readonly IBNDWrapper? flverBnd;
    private readonly string flverFilePath;
    private readonly string textureFilePath;
    private readonly string oldfilename;
    private TPF.Texture? newTexture;
    private TPF.Texture? oldTexture;
    private TPF.Texture? replacedTexture;
    private int textureIndex;

    public UpdateTextureAction(TPF tpf, IBNDWrapper? flverBnd, string flverFilePath, string textureFilePath, string oldfilename, TPF.Texture? newTexture, Action<string> refresher)
    {
        this.tpf = tpf;
        this.flverBnd = flverBnd;
        this.flverFilePath = flverFilePath;
        this.textureFilePath = textureFilePath;
        this.oldfilename = oldfilename;
        this.newTexture = newTexture;
        windowRefresh = refresher;
    }

    public override void Execute()
    {
        oldTexture = null;
        replacedTexture = null;

        BinderFile? flverBndTpfEntry = flverBnd?.Files.FirstOrDefault(i => i.Name.EndsWith(".tpf"));

        if (textureFilePath != "")
        {
            byte[] ddsBytes = File.ReadAllBytes(textureFilePath);
            DDS dds = new(ddsBytes);
            byte formatByte = 107;
            try
            {
                formatByte = (byte)Enum.Parse(typeof(Program.TextureFormats), dds.header10.dxgiFormat.ToString());
            }
            catch { }
            newTexture = new(Path.GetFileNameWithoutExtension(textureFilePath), formatByte, 0x00, File.ReadAllBytes(textureFilePath));
        }

        textureIndex = tpf.Textures.FindIndex(i => i.Name == newTexture?.Name);

        var oldTextureIndex = tpf.Textures.FindIndex(i => i.Name == oldfilename);

        if (oldTextureIndex != -1)
        {
            oldTexture = tpf.Textures[oldTextureIndex];
        }

        if (textureIndex != -1)
        {
            replacedTexture = tpf.Textures[textureIndex];

            tpf.Textures.RemoveAt(textureIndex);
            tpf.Textures.Insert(textureIndex, newTexture);
        }
        else tpf.Textures.Add(newTexture);

        if (flverBndTpfEntry is not null)
        {
            flverBnd!.Files[flverBnd.Files.IndexOf(flverBndTpfEntry)].Bytes = tpf.Write();
        }
        else
        {
            SaveTPF();
        }

        windowRefresh?.Invoke(textureFilePath);
    }

    private void SaveTPF()
    {
        if (flverFilePath.Contains(".flver")) tpf.Write(Program.RemoveIndexSuffix(flverFilePath).Replace(".flver", ".tpf"));
        else if (flverFilePath.Contains(".flv")) tpf.Write(Program.RemoveIndexSuffix(flverFilePath).Replace(".flv", ".tpf"));
    }

    public override void Undo()
    {
        BinderFile? flverBndTpfEntry = flverBnd?.Files.FirstOrDefault(i => i.Name.EndsWith(".tpf"));

        if (oldTexture is null)
        {
            tpf.Textures.Remove(newTexture);
        }
        else
        {
            tpf.Textures.RemoveAt(textureIndex);
            tpf.Textures.Insert(textureIndex, replacedTexture);
        }

        if (flverBndTpfEntry is not null)
        {
            flverBnd!.Files[flverBnd.Files.IndexOf(flverBndTpfEntry)].Bytes = tpf.Write();
        }
        else
        {
            SaveTPF();
        }

        windowRefresh?.Invoke(oldTexture?.Name ?? oldfilename ?? "");
    }
}

