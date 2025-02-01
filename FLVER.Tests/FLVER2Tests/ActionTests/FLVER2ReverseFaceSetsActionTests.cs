using System.Numerics;
using FLVER_Editor.Actions;
using SoulsFormats;
using Xunit;

namespace FLVERS.Tests.FLVER2Tests.ActionTests;

public class FLVER2ReverseFaceSetsActionTests : IClassFixture<DataFixture>
{
    private readonly DataFixture dataFixture;

    public FLVER2ReverseFaceSetsActionTests(DataFixture dataFixture)
    {
        this.dataFixture = dataFixture;
    }

    [Fact]
    public void FlipFaceSetsAndUndo()
    {
        var file = FLVER2.Read(dataFixture.Flver2_1);
        var expected = dataFixture.Flver2_1_Read;

        ReverseFaceSetsAction action = new(file.Meshes[0].FaceSets, () => { });
        action.Execute();
        action.Undo();
        for (int i = 0; i < expected.Meshes[0].FaceSets.Count; i++)
        {
            FlverTestHelper.Equal(expected.Meshes[0].FaceSets[i], file.Meshes[0].FaceSets[i]);
        }
    }
}
