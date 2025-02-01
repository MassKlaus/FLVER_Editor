using System.Numerics;
using FLVER_Editor.Actions;
using SoulsFormats;
using Xunit;

namespace FLVERS.Tests.FLVER2Tests.ActionTests;

public class FLVER2ReverseNormalsActionTests : IClassFixture<DataFixture>
{
    private readonly DataFixture dataFixture;

    public FLVER2ReverseNormalsActionTests(DataFixture dataFixture)
    {
        this.dataFixture = dataFixture;
    }

    [Fact]
    public void UnWrittenTest()
    {
        var file = FLVER2.Read(dataFixture.Flver2_1);
        var expected = dataFixture.Flver2_1_Read;

        ReverseNormalsAction action = new(file.Meshes[0].Vertices, () => { });
        action.Execute();
        action.Undo();
        FlverTestHelper.Equal(expected.Meshes[0], file.Meshes[0]);

    }
}
