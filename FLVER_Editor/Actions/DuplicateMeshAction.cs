using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLVER_Editor.Actions;

public class DuplicateMeshAction : TransformAction
{
    private readonly FLVER2 flver;
    private readonly FLVER2.Mesh mesh;
    private readonly int insertIndex;
    private readonly Action refresher;

    private FLVER2.Mesh? duplicateMesh;
    private FLVER2.Material? duplicateMaterial;

    public DuplicateMeshAction(FLVER2 flver, FLVER2.Mesh mesh, int insertIndex, Action refresher)
    {
        this.flver = flver;
        this.mesh = mesh;
        this.insertIndex = insertIndex;
        this.refresher = refresher;
    }

    public override void Execute()
    {
        duplicateMesh = mesh.Copy();
        duplicateMaterial = flver.Materials[mesh.MaterialIndex].Copy();
        flver.Meshes.Insert(insertIndex, duplicateMesh);
        flver.Materials.Insert(mesh.MaterialIndex + 1, duplicateMaterial);

        duplicateMesh.MaterialIndex = mesh.MaterialIndex + 1;
        refresher.Invoke();
    }

    public override void Undo()
    {
        flver.Meshes.Remove(duplicateMesh);
        flver.Materials.Remove(duplicateMaterial);
        refresher.Invoke();
    }
}
