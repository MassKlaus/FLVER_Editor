using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLVER_Editor.Actions;

public class AddTextureAction : TransformAction
{
    private Action windowRefresh;
    private readonly TPF tpf;
    private TPF.Texture? newTexture;
    private TPF.Texture? replacedTexture;

    private bool Appended = false;

    public AddTextureAction(TPF tpf, TPF.Texture? newTexture, Action refresher)
    {
        this.tpf = tpf;
        this.newTexture = newTexture;
        windowRefresh = refresher;
    }
    /// <summary>
    /// We want to add a texture to the TPF, names should not conflict as such we replace any uneeded ones
    /// </summary>
    public override void Execute()
    {
        var textureIndex = tpf.Textures.FindIndex(i => i.Name == newTexture?.Name);
        Appended = textureIndex == -1;

        if (Appended)
            tpf.Textures.Add(newTexture);
        else
        {
            replacedTexture = tpf.Textures[textureIndex];
            tpf.Textures[textureIndex] = newTexture;
        }

        windowRefresh?.Invoke();
    }

    public override void Undo()
    {
        if (Appended)
        {
            tpf.Textures.Remove(newTexture);
        }
        else
        {
            var textureIndex = tpf.Textures.IndexOf(newTexture);
            tpf.Textures[textureIndex] = replacedTexture;
        }

        windowRefresh?.Invoke();
    }
}


public class UpdateTextureAction : TransformAction
{
    private readonly string oldTextureName;
    private readonly string newTextureName;
    private readonly Action<string> refresher;
    private TransformAction addTextureAction;

    public UpdateTextureAction(TPF tpf, TPF.Texture? newTexture, string oldTextureName, Action<string> refresher)
    {
        addTextureAction = new AddTextureAction(tpf, newTexture, () => {});
        this.oldTextureName = oldTextureName;
        this.newTextureName = newTexture?.Name ?? "";
        this.refresher = refresher;
    }
    /// <summary>
    /// We want to add a texture to the TPF, names should not conflict as such we replace any uneeded ones
    /// </summary>
    public override void Execute()
    {
        addTextureAction.Execute();
        refresher?.Invoke(newTextureName);
    }

    public override void Undo()
    {
        addTextureAction.Undo();
        refresher?.Invoke(oldTextureName);
    }
}
