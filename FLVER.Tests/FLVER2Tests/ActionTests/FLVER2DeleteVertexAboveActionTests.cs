using System.Numerics;
using FLVER_Editor.Actions;
using SoulsFormats;
using Xunit;

namespace FLVERS.Tests.FLVER2Tests.ActionTests;

public class FLVER2DeleteVertexAboveActionTests : IClassFixture<DataFixture>
{
    private readonly DataFixture dataFixture;

    public FLVER2DeleteVertexAboveActionTests(DataFixture dataFixture)
    {
        this.dataFixture = dataFixture;
    }

    [Fact]
    public void DeleteAllVertexHigherThanTargetThenUndo()
    {
        FLVER2 file = FLVER2.Read(dataFixture.Flver2_1);
        FLVER2 expected = dataFixture.Flver2_1_Read;

        FLVER2.Mesh expectedMesh = expected.Meshes[0];
        FLVER2.Mesh mesh = file.Meshes[0];

        var height = mesh.Vertices[0].Position.Y;

        var deleteCount = mesh.Vertices.Count(x => x.Position.Y > height);

        DeleteVertexAboveAction action = new(mesh, height, () => { });
        action.Execute();

        Assert.NotEqual(deleteCount, expectedMesh.Vertices.Count -  mesh.Vertices.Count);

        action.Undo();
        Assert.Equal(expectedMesh.Vertices.Count, mesh.Vertices.Count);

        FlverTestHelper.Equal(expected, file);
    }
}
