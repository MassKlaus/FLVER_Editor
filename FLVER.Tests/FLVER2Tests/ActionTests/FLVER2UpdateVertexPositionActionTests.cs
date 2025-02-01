using System.Numerics;
using FLVER_Editor.Actions;
using SoulsFormats;
using Xunit;

namespace FLVERS.Tests.FLVER2Tests.ActionTests;

public class FLVER2UpdateVertexPositionActionTests : IClassFixture<DataFixture>
{
    private readonly DataFixture dataFixture;

    public FLVER2UpdateVertexPositionActionTests(DataFixture dataFixture)
    {
        this.dataFixture = dataFixture;
    }

    [Fact]
    public void UnWrittenTest()
    {
        var file = FLVER2.Read(dataFixture.Flver2_1);
        var expected = dataFixture.Flver2_1_Read;

        var expectedVertex = expected.Meshes[0].Vertices[0];
        var vertex = file.Meshes[0].Vertices[0];

        var newPosition = new Vector3(5555, 5555, 5555);
        UpdateVertexPositionAction action = new(newPosition, vertex, () => { });
        action.Execute();

        FlverTestHelper.Equal(newPosition, vertex.Position);
        action.Undo();

        FlverTestHelper.Equal(expectedVertex.Position, vertex.Position);
    }
}
