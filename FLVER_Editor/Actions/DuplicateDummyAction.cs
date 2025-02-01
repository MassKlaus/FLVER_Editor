using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLVER_Editor.Actions;

public class DuplicateDummyAction : TransformAction
{
    private readonly FLVER2 flver;
    private readonly int insertIndex;
    private readonly FLVER.Dummy dummy;
    private Action? windowRefresh;

    public DuplicateDummyAction(FLVER2 flver, FLVER.Dummy dummy, int insertIndex, Action refresh)
    {
        this.flver = flver;
        this.insertIndex = insertIndex;
        this.dummy = dummy.Copy();
        this.windowRefresh = refresh;
    }

    public override void Execute()
    {
        flver.Dummies.Insert(insertIndex, dummy);
        windowRefresh?.Invoke();
    }

    public override void Undo()
    {
        flver.Dummies.Remove(dummy);
        windowRefresh?.Invoke();
    }
}
