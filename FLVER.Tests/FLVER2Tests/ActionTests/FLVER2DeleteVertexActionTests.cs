using System.Numerics;
using FLVER_Editor.Actions;
using SoulsFormats;
using Xunit;

namespace FLVERS.Tests.FLVER2Tests.ActionTests;

public class FLVER2DeleteVertexActionTests : IClassFixture<DataFixture>
{
    private readonly DataFixture dataFixture;

    public FLVER2DeleteVertexActionTests(DataFixture dataFixture)
    {
        this.dataFixture = dataFixture;
    }

    [Fact]
    public void DeleteOnlyTargetVertexThenUndo()
    {
        FLVER2 file = FLVER2.Read(dataFixture.Flver2_1);
        FLVER2 expected = dataFixture.Flver2_1_Read;

        FLVER2.Mesh expectedMesh = expected.Meshes[0];
        FLVER2.Mesh mesh = file.Meshes[0];

        DeleteVertexAction action = new(mesh, 0, () => { });
        action.Execute();

        Assert.NotEqual(expectedMesh.Vertices.Count - 1, mesh.Vertices.Count);

        action.Undo();
        Assert.Equal(expectedMesh.Vertices.Count, mesh.Vertices.Count);

        FlverTestHelper.Equal(expected, file);
    }
}
