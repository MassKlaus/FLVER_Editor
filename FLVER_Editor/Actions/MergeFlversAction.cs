using SoulsFormats;

namespace FLVER_Editor.Actions;

public class MergeFlversAction : TransformAction
{
    private readonly FLVER2 currentFlver;
    private readonly int layoutOffset;
    private readonly int materialOffset;
    private readonly int meshOffset;
    private readonly int gxOffset;
    private readonly FLVER2 newFlver;
    private readonly TPF? currentTPF;
    private readonly TPF? newFlverTpf;
    private readonly Stack<AddTextureAction> addActions = new();
    private readonly Action refresher;

    public MergeFlversAction(FLVER2 currentFlver, FLVER2 newFlver, TPF? currentTPF, TPF? newFlverTpf, Action refresher)
    {
        materialOffset = currentFlver.Materials.Count;
        meshOffset = currentFlver.Meshes.Count;
        layoutOffset = currentFlver.BufferLayouts.Count;
        gxOffset = currentFlver.GXLists.Count;
        this.currentFlver = currentFlver;
        this.newFlver = newFlver;
        this.currentTPF = currentTPF;
        this.newFlverTpf = newFlverTpf;
        this.refresher = refresher;

        // TODO: WIP (Pear)
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
            material.GXIndex += gxOffset;
        currentFlver.BufferLayouts = currentFlver.BufferLayouts.Concat(newFlver.BufferLayouts).ToList();
        currentFlver.Meshes = currentFlver.Meshes.Concat(newFlver.Meshes).ToList();
        currentFlver.Materials = currentFlver.Materials.Concat(newFlver.Materials).ToList();
        currentFlver.GXLists = currentFlver.GXLists.Concat(newFlver.GXLists).ToList();


        if (currentTPF is not null && newFlverTpf is not null)
        {
            foreach (TPF.Texture tex in newFlverTpf)
            {
                var action = new AddTextureAction(currentTPF, tex, () => { });
                action.Execute();
                addActions.Push(action);
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
        currentFlver.GXLists.RemoveRange(gxOffset, currentFlver.GXLists.Count - gxOffset);

        if (currentTPF is not null && newFlverTpf is not null)
        {
            while (addActions.Count > 0)
            {
                addActions.Pop().Undo();
            }
        }

        refresher.Invoke();
    }
}