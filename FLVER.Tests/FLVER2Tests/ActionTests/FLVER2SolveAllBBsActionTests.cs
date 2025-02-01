using System.Numerics;
using FLVER_Editor.Actions;
using SoulsFormats;
using Xunit;

namespace FLVERS.Tests.FLVER2Tests.ActionTests;

public class FLVER2SolveAllBBsActionTests : IClassFixture<DataFixture>
{
    private readonly DataFixture dataFixture;

    public FLVER2SolveAllBBsActionTests(DataFixture dataFixture)
    {
        this.dataFixture = dataFixture;
    }

    [Fact]
    public void EnsuringUndoIsCorrect()
    {
        var file = FLVER2.Read(dataFixture.Flver2_1);
        var expected = dataFixture.Flver2_1_Read;
        SolveAllBBsAction action = new(file, () => { });
        action.Execute();
        action.Undo();

        FlverTestHelper.Equal(expected, file);
    }
}
