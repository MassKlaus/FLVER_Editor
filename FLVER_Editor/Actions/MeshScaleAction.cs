using ObjLoader.Loader.Data;
using SharpDX.MediaFoundation;
using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FLVER_Editor.Actions;

public class MeshScaleAction : TransformAction
{
    private readonly Action<decimal, float, bool, IEnumerable<FLVER2.Mesh>, IEnumerable<FLVER.Dummy>> refresher;
    private readonly IEnumerable<FLVER2.Mesh> selectedMeshes;
    private readonly IEnumerable<FLVER.Dummy> selectedDummies;
    private readonly float offset;
    private readonly Vector3 totals;
    private readonly TransformAxis axis;
    private readonly float oldValue;
    private readonly float newValue;
    private readonly bool uniform;
    private readonly bool vectorMode;

    // TODO(metty): Make this less confusing
    public MeshScaleAction(IEnumerable<FLVER2.Mesh> selectedMeshes, IEnumerable<FLVER.Dummy> selectedDummies, float offset, Vector3 totals, TransformAxis axis, float oldValue, float newValue, bool uniform, bool vectorMode, Action<decimal, float, bool, IEnumerable<FLVER2.Mesh>, IEnumerable<FLVER.Dummy>> refresher)
    {
        this.selectedMeshes = selectedMeshes;
        this.selectedDummies = selectedDummies;
        this.offset = offset;
        this.totals = totals;
        this.axis = axis;
        this.oldValue = oldValue;
        this.newValue = newValue;
        this.uniform = uniform;
        this.vectorMode = vectorMode;
        this.refresher = refresher;
    }

    private void TransformThing(dynamic thing, float offset, Vector3 totals, TransformAxis axis, bool uniform, bool vectorMode)
    {
        Transform3DOperations2.ScaleThing(thing, offset, totals, axis, uniform, false, vectorMode);
    }

    public override void Execute()
    {
        foreach (var mesh in selectedMeshes)
        {
            foreach (FLVER.Vertex v in mesh.Vertices)
                TransformThing(v, offset, totals, axis, uniform, vectorMode);
        }

        foreach (var i in selectedDummies)
            TransformThing(i, offset, totals, axis, uniform, vectorMode);

        refresher?.Invoke(ExtractInputValue(newValue), newValue, uniform, selectedMeshes, selectedDummies);
    }

    public override void Undo()
    {
        foreach (var mesh in selectedMeshes)
        {
            foreach (FLVER.Vertex v in mesh.Vertices)
                TransformThing(v, -offset, totals, axis, uniform, vectorMode);
        }

        foreach (var i in selectedDummies)
            TransformThing(i, -offset, totals, axis, uniform, vectorMode);

        refresher?.Invoke(ExtractInputValue(oldValue), oldValue, uniform, selectedMeshes, selectedDummies);
    }

    public decimal ExtractInputValue(float value)
    {
        return (decimal)value * 100;
    }
}
