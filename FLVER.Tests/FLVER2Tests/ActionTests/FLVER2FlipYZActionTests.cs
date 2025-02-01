using System.Numerics;
using FLVER_Editor.Actions;
using SoulsFormats;
using Xunit;

namespace FLVERS.Tests.FLVER2Tests.ActionTests;

public class FLVER2FlipYZActionTests : IClassFixture<DataFixture>
{
    private readonly DataFixture dataFixture;

    public FLVER2FlipYZActionTests(DataFixture dataFixture)
    {
        this.dataFixture = dataFixture;
    }

    [Fact]
    public void FlipYZThenUndo()
    {
        FLVER2 file = FLVER2.Read(dataFixture.Flver2_1);
        FLVER2 expected = dataFixture.Flver2_1_Read;

        var vertices = file.Meshes[0].Vertices;
        var expectedVertices = expected.Meshes[0].Vertices;

        FlipYZAction action = new(vertices, file.Dummies, () => { });
        action.Execute();

        for (int i = 0; i < vertices.Count; i++)
        {
            Assert.Equal(expectedVertices[i].Position.Z, vertices[i].Position.Y);
            Assert.Equal(expectedVertices[i].Position.Y, vertices[i].Position.Z);
        }

        action.Undo();

        for (int i = 0; i < vertices.Count; i++)
        {
            Assert.Equal(expectedVertices[i].Position.Z, vertices[i].Position.Z);
            Assert.Equal(expectedVertices[i].Position.Y, vertices[i].Position.Y);
        }

        FlverTestHelper.Equal(expected, file);
    }
}
