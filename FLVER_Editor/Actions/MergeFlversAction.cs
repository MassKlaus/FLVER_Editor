using SoulsFormats;

namespace FLVER_Editor.Actions;

public class MergeFlversAction : TransformAction
{
    private readonly string _flverFilePath;
    private readonly string _newFlverFilePath;
    private readonly IBNDWrapper bnd;
    private readonly FLVER2 currentFlver;
    private readonly int layoutOffset;
    private readonly int materialOffset;
    private readonly int meshOffset;
    private readonly FLVER2 newFlver;
    private readonly Action refresher;

    public MergeFlversAction(IBNDWrapper bnd, FLVER2 currentFlver, FLVER2 newFlver, string flverFilePath, string newFlverFilePath, Action refresher)
    {
        materialOffset = currentFlver.Materials.Count;
        meshOffset = currentFlver.Meshes.Count;
        layoutOffset = currentFlver.BufferLayouts.Count;
        this.bnd = bnd;
        this.currentFlver = currentFlver;
        _flverFilePath = flverFilePath;
        _newFlverFilePath = newFlverFilePath;
        this.newFlver = newFlver;
        this.refresher = refresher;
    }

    public override void Execute()
    {
        Dictionary<int, int> newFlverToCurrentFlver = new();
        for (int i = 0; i < newFlver.Nodes.Count; ++i)
        {
            FLVER.Node attachBone = newFlver.Nodes[i];
            for (int j = 0; j < currentFlver.Nodes.Count; ++j)
            {
                if (attachBone.Name != currentFlver.Nodes[j].Name) continue;
                newFlverToCurrentFlver.Add(i, j);
                break;
            }
        }
        foreach (FLVER2.Mesh m in newFlver.Meshes)
        {
            m.MaterialIndex += materialOffset;
            foreach (FLVER2.VertexBuffer vb in m.VertexBuffers)
                vb.LayoutIndex += layoutOffset;
            foreach (FLVER.Vertex v in m.Vertices.Where(v => Util3D.BoneIndicesToIntArray(v.BoneIndices) != null))
            {
                for (int i = 0; i < v.BoneIndices.Length; ++i)
                {
                    if (newFlverToCurrentFlver.ContainsKey(v.BoneIndices[i])) v.BoneIndices[i] = newFlverToCurrentFlver[v.BoneIndices[i]];
                }
            }
        }
        foreach (FLVER2.Material material in newFlver.Materials)
            material.GXIndex += currentFlver.GXLists.Count;
        currentFlver.BufferLayouts = currentFlver.BufferLayouts.Concat(newFlver.BufferLayouts).ToList();
        currentFlver.Meshes = currentFlver.Meshes.Concat(newFlver.Meshes).ToList();
        currentFlver.Materials = currentFlver.Materials.Concat(newFlver.Materials).ToList();
        currentFlver.GXLists = currentFlver.GXLists.Concat(newFlver.GXLists).ToList();

        // TODO: WIP (Pear)
        TPF newFlverTpf = new();
        if (_newFlverFilePath.EndsWith(".dcx"))
        {
            BND4? newFlverBnd = BND4.Read(_newFlverFilePath);
            BinderFile? newFlverTpfEntry = newFlverBnd.Files.Find(i => i.Name.EndsWith(".tpf"));
            if (newFlverTpfEntry != null) newFlverTpf = TPF.Read(newFlverTpfEntry.Bytes);
        }
        else if (_newFlverFilePath.EndsWith(".flver"))
        {
            newFlverTpf = TPF.Read(_newFlverFilePath.Replace(".flver", ".tpf"));
        }
        Program.Tpf ??= TPF.Read(_flverFilePath.Replace(".flver", ".tpf"));
        foreach (TPF.Texture tex in newFlverTpf)
        {
            if (Program.Tpf.Textures.All(i => i.Name != tex.Name))
            {
                UpdateTextureAction action = new(Program.Tpf, bnd, _flverFilePath, "", tex.Name, tex, _ => { });
                ActionManager.Apply(action);
            }
        }
        refresher.Invoke();
    }

    // TODO: WIP (Pear)

    public override void Undo()
    {
        foreach (FLVER2.Mesh m in newFlver.Meshes)
        {
            m.MaterialIndex -= materialOffset;
            foreach (FLVER2.VertexBuffer vb in m.VertexBuffers)
                vb.LayoutIndex -= layoutOffset;
        }
        currentFlver.BufferLayouts.RemoveRange(layoutOffset, currentFlver.BufferLayouts.Count - layoutOffset);
        currentFlver.Meshes.RemoveRange(meshOffset, currentFlver.Meshes.Count - meshOffset);
        currentFlver.Materials.RemoveRange(materialOffset, currentFlver.Materials.Count - materialOffset);
        refresher.Invoke();
    }
}