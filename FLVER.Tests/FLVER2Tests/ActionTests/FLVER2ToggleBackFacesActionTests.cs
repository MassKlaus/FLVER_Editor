using System.Numerics;
using FLVER_Editor.Actions;
using SoulsFormats;
using Xunit;

namespace FLVERS.Tests.FLVER2Tests.ActionTests;

public class FLVER2ToggleBackFacesActionTests : IClassFixture<DataFixture>
{
    private readonly DataFixture dataFixture;

    public FLVER2ToggleBackFacesActionTests(DataFixture dataFixture)
    {
        this.dataFixture = dataFixture;
    }

    [Fact]
    public void EnsuringUndoIsCorrect()
    {
        var file = FLVER2.Read(dataFixture.Flver2_1);
        var expected = dataFixture.Flver2_1_Read;
        ToggleBackFacesAction action = new(file.Meshes[0].FaceSets, () => {});
        action.Execute();
        action.Undo();

        FlverTestHelper.Equal(expected, file);
    }
}
