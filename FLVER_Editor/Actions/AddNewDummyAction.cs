using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FLVER_Editor.Actions;

public class AddNewDummyAction : TransformAction
{
    private readonly FLVER2 flver;
    private readonly Action refresher;
    private readonly FLVER.Dummy newDummy;

    public AddNewDummyAction(FLVER2 flver, Vector3 position, Action refresher)
    {
        this.flver = flver;
        this.refresher = refresher;
        newDummy = new()
        {
            Position = position,
            ReferenceID = -1
        };
    }

    public override void Execute()
    {
        flver.Dummies.Add(newDummy);
        refresher.Invoke();
    }

    public override void Undo()
    {
        flver.Dummies.Remove(newDummy);
        refresher.Invoke();
    }
}
