﻿using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLVER_Editor.Actions;

public class DeleteSelectedMeshAction : TransformAction
{
    private readonly FLVER2 flver;
    private readonly DataGridView meshTable;
    private readonly DataGridView dummiesTable;
    private readonly bool deleteFacesets;
    private readonly Action? refresher;
    private Dictionary<FLVER2.FaceSet, List<int>> facesetOldIndices = new();
    private Dictionary<int, FLVER2.Mesh> deletedMeshes = new();
    private Dictionary<int, FLVER.Dummy> deletedDummies = new();

    public DeleteSelectedMeshAction(FLVER2 flver, DataGridView meshTable, DataGridView dummiesTable, bool deleteFacesets, Action? refresher)
    {
        this.flver = flver;
        this.meshTable = meshTable;
        this.dummiesTable = dummiesTable;
        this.deleteFacesets = deleteFacesets;
        this.refresher = refresher;
    }

    public override void Execute()
    {
        this.facesetOldIndices.Clear();
        this.deletedMeshes.Clear();
        this.deletedDummies.Clear();

        for (int i = flver.Meshes.Count - 1; i >= 0; --i)
        {
            if (!(bool)meshTable.Rows[i].Cells[3].Value) continue;
            if (deleteFacesets)
            {
                foreach (FLVER2.FaceSet fs in flver.Meshes[i].FaceSets)
                {
                    var facesetIndices = new List<int>();
                    
                    for (int j = 0; j < fs.Indices.Count; ++j)
                    {
                        facesetIndices.Add(fs.Indices[j]);
                        fs.Indices[j] = 1;
                    }

                    facesetOldIndices.Add(fs, facesetIndices);
                }
            }
            else
            {
                deletedMeshes.Add(i, flver.Meshes[i]);
                flver.Meshes.RemoveAt(i);
            }

            refresher?.Invoke();
        }

        for (int i = flver.Dummies.Count - 1; i >= 0; --i)
        {
            if (!(bool)dummiesTable.Rows[i].Cells[4].Value) continue;

            deletedDummies.Add(i, flver.Dummies[i]);
            flver.Dummies.RemoveAt(i);
        }
    }

    public override void Undo()
    {
        if (deleteFacesets)
        {
            foreach (var fs in facesetOldIndices)
            {
                var facesetIndices = fs.Value;
                var faceset = fs.Key;

                for (int j = 0; j < faceset.Indices.Count; ++j)
                {
                    faceset.Indices[j] = facesetIndices[j];
                }
            }
        }
        else
        {
            foreach (var fs in deletedMeshes)
            {
                flver.Meshes.Insert(fs.Key, fs.Value);
            }
        }

        foreach (var fs in deletedDummies)
        {
            flver.Dummies.Insert(fs.Key, fs.Value);
        }

        refresher?.Invoke();
    }
}