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

public class MeshTansformAction : TransformAction
{
    private readonly Action<decimal, float, IEnumerable<FLVER2.Mesh>, IEnumerable<FLVER.Dummy>> refresher;
    private readonly IEnumerable<FLVER2.Mesh> selectedMeshes;
    private readonly IEnumerable<FLVER.Dummy> selectedDummies;
    private readonly float offset;
    private readonly TransformAxis axis;
    private readonly float oldValue;
    private readonly float newValue;

    // TODO(metty): Make this less confusing
    public MeshTansformAction(IEnumerable<FLVER2.Mesh> selectedMeshes, IEnumerable<FLVER.Dummy> selectedDummies, float offset, TransformAxis axis, float oldValue, float newValue, Action<decimal, float, IEnumerable<FLVER2.Mesh>, IEnumerable<FLVER.Dummy>> refresher)
    {
        this.selectedMeshes = selectedMeshes;
        this.selectedDummies = selectedDummies;
        this.offset = offset;
        this.axis = axis;
        this.oldValue = oldValue;
        this.newValue = newValue;
        this.refresher = refresher;
    }

    public override void Execute()
    {
        foreach (var mesh in selectedMeshes)
        {
            foreach (FLVER.Vertex v in mesh.Vertices)
                Transform3DOperations2.TranslateThing(v, offset / 55, axis);
        }

        foreach (var dummy in selectedDummies)
            Transform3DOperations2.TranslateThing(dummy, offset / 55, axis);

        refresher?.Invoke(ExtractInputValue(newValue), newValue, selectedMeshes, selectedDummies);
    }

    public override void Undo()
    {
        foreach (var mesh in selectedMeshes)
        {
            foreach (FLVER.Vertex v in mesh.Vertices)
                Transform3DOperations2.TranslateThing(v, -offset / 55, axis);
        }

        foreach (var dummy in selectedDummies)
            Transform3DOperations2.TranslateThing(dummy, -offset / 55, axis);

        refresher?.Invoke(ExtractInputValue(oldValue), oldValue, selectedMeshes, selectedDummies);
    }

    public decimal ExtractInputValue(float value)
    {
        return (decimal)value;
    }
}
