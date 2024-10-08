﻿using SharpDX.MediaFoundation;
using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FLVER_Editor.Actions;

public class MeshTansformAction : TransformAction
{
    private readonly Action<decimal, float, bool, IEnumerable<int>, IEnumerable<int>> refresher;
    private readonly FLVER2 flver;
    private readonly IEnumerable<int> selectedMeshes;
    private readonly IEnumerable<int> selectedDummies;
    private readonly float offset;
    private readonly IReadOnlyList<float> totals;
    private readonly int nbi;
    private readonly float oldValue;
    private readonly float newValue;
    private readonly bool uniform;
    private readonly bool vectorMode;

    public MeshTansformAction(FLVER2 flver, List<int> selectedMeshes, List<int> selectedDummies, float offset, IReadOnlyList<float> totals, int nbi, float oldValue, float newValue, bool uniform, bool vectorMode, Action<decimal, float, bool, IEnumerable<int>, IEnumerable<int>> refresher)
    {
        this.flver = flver;
        this.selectedMeshes = selectedMeshes;
        this.selectedDummies = selectedDummies;
        this.offset = offset;
        this.totals = totals;
        this.nbi = nbi;
        this.oldValue = oldValue;
        this.newValue = newValue;
        this.uniform = uniform;
        this.vectorMode = vectorMode;
        this.refresher = refresher;
    }

    private void TransformThing(dynamic thing, float offset, IReadOnlyList<float> totals, int nbi, bool uniform, bool vectorMode)
    {
        switch (nbi)
        {
            case 0:
            case 1:
            case 2:
                Transform3DOperations.TranslateThing(thing, offset / 55, nbi);
                break;
            case 3:
            case 4:
            case 5:
                Transform3DOperations.ScaleThing(thing, offset, totals, nbi, uniform, false, vectorMode);
                break;
            case 6:
            case 7:
            case 8:
                Transform3DOperations.RotateThing(thing, offset, totals, nbi, vectorMode);
                break;
        }
    }

    public override void Execute()
    {
        foreach (var meshIndex in selectedMeshes)
        {
            var mesh = flver.Meshes[meshIndex];

            foreach (FLVER.Vertex v in mesh.Vertices)
                TransformThing(v, offset, totals, nbi, uniform, vectorMode);
        }

        foreach (int i in selectedDummies)
            TransformThing(flver.Dummies[i], offset, totals, nbi, uniform, vectorMode);

        refresher?.Invoke(ExtractInputValue(newValue), newValue, uniform, selectedMeshes, selectedDummies);
    }

    public override void Undo()
    {
        foreach (var meshIndex in selectedMeshes)
        {
            var mesh = flver.Meshes[meshIndex];

            foreach (FLVER.Vertex v in mesh.Vertices)
                TransformThing(v, -offset, totals, nbi, uniform, vectorMode);
        }

        foreach (int i in selectedDummies)
            TransformThing(flver.Dummies[i], -offset, totals, nbi, uniform, vectorMode);

        refresher?.Invoke(ExtractInputValue(oldValue), oldValue, uniform, selectedMeshes, selectedDummies);
    }

    private static decimal ToRadians(decimal degrees) { return degrees * (decimal)(Math.PI / 180); }
    private static decimal ToDeg(decimal radians) { return radians * (decimal)(180 / Math.PI); }


    public decimal ExtractInputValue(float value)
    {
        return nbi switch
        {
            0 or 1 or 2 => (decimal)value,
            3 or 4 or 5 => (decimal)value * 100,
            6 or 8 => ToDeg((decimal)value),
            7 => selectedMeshes.Any() ? -ToDeg((decimal)value) : ToDeg((decimal)value),
            _ => (decimal)value
        };
    }
}
