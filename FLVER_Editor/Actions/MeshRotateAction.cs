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

public class MeshRotateAction : TransformAction
{
    private readonly Action<decimal, float, IEnumerable<FLVER2.Mesh>, IEnumerable<FLVER.Dummy>> refresher;
    private readonly IEnumerable<FLVER2.Mesh> selectedMeshes;
    private readonly IEnumerable<FLVER.Dummy> selectedDummies;
    private readonly float offset;
    private readonly  Vector3 totals;
    private readonly TransformAxis axis;
    private readonly float oldValue;
    private readonly float newValue;
    private readonly bool vectorMode;

    // TODO(metty): Make this less confusing
    public MeshRotateAction(IEnumerable<FLVER2.Mesh> selectedMeshes, IEnumerable<FLVER.Dummy> selectedDummies, float offset,  Vector3 totals, TransformAxis axis, float oldValue, float newValue, bool vectorMode, Action<decimal, float, IEnumerable<FLVER2.Mesh>, IEnumerable<FLVER.Dummy>> refresher)
    {
        this.selectedMeshes = selectedMeshes;
        this.selectedDummies = selectedDummies;
        this.offset = offset;
        this.totals = totals;
        this.axis = axis;
        this.oldValue = oldValue;
        this.newValue = newValue;
        this.vectorMode = vectorMode;
        this.refresher = refresher;
    }

    private void TransformThing(dynamic thing, float offset, Vector3 totals, TransformAxis axis, bool vectorMode)
    {
        Transform3DOperations2.RotateThing(thing, offset, totals, axis, vectorMode);
    }

    public override void Execute()
    {
        foreach (var mesh in selectedMeshes)
        {
            foreach (FLVER.Vertex v in mesh.Vertices)
                TransformThing(v, offset, totals, axis, vectorMode);
        }

        foreach (var i in selectedDummies)
            TransformThing(i, offset, totals, axis, vectorMode);

        refresher?.Invoke(ExtractInputValue(newValue), newValue, selectedMeshes, selectedDummies);
    }

    public override void Undo()
    {
        foreach (var mesh in selectedMeshes)
        {
            foreach (FLVER.Vertex v in mesh.Vertices)
                TransformThing(v, -offset, totals, axis, vectorMode);
        }

        foreach (var i in selectedDummies)
            TransformThing(i, -offset, totals, axis, vectorMode);

        refresher?.Invoke(ExtractInputValue(oldValue), oldValue, selectedMeshes, selectedDummies);
    }

    private static decimal ToRadians(decimal degrees) { return degrees * (decimal)(Math.PI / 180); }
    private static decimal ToDeg(decimal radians) { return radians * (decimal)(180 / Math.PI); }


    public decimal ExtractInputValue(float value)
    {
        return axis switch
        {
            TransformAxis.X or TransformAxis.Z => ToDeg((decimal)value),
            TransformAxis.Y => selectedMeshes.Any() ? -ToDeg((decimal)value) : ToDeg((decimal)value),
            _ => (decimal)value
        };
    }
}
