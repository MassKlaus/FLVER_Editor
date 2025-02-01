using System.Numerics;
using FLVER_Editor.Actions;
using SoulsFormats;
using Xunit;

namespace FLVERS.Tests.FLVER2Tests.ActionTests;

public class FLVER2AddNewDummyActionTests : IClassFixture<DataFixture>
{
    private readonly DataFixture dataFixture;

    public FLVER2AddNewDummyActionTests(DataFixture dataFixture)
    {
        this.dataFixture = dataFixture;
    }

    [Fact]
    public void AddDummyThenRemove()
    {
        FLVER2 file = FLVER2.Read(dataFixture.Flver2_1);
        var original = dataFixture.Flver2_1_Read;

        AddNewDummyAction action = new(file, new Vector3(), () => { });
        action.Execute();

        Assert.Equal(original.Dummies.Count + 1, file.Dummies.Count);

        action.Undo();
        Assert.Equal(original.Dummies.Count, file.Dummies.Count);
    }

}
