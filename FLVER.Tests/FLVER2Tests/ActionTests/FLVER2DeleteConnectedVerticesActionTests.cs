using System.Numerics;
using FLVER_Editor.Actions;
using SoulsFormats;
using Xunit;

namespace FLVERS.Tests.FLVER2Tests.ActionTests;

public class FLVER2DeleteConnectedVerticesActionTests : IClassFixture<DataFixture>
{
    private readonly DataFixture dataFixture;

    public FLVER2DeleteConnectedVerticesActionTests(DataFixture dataFixture)
    {
        this.dataFixture = dataFixture;
    }

    [Fact]
    public void DeletedVetexAndThoseConnectedToItThenRestore()
    {
        FLVER2 file = FLVER2.Read(dataFixture.Flver2_1);
        FLVER2 expected = dataFixture.Flver2_1_Read;

        FLVER2.Mesh expectedMesh = expected.Meshes[0];
        FLVER2.Mesh mesh = file.Meshes[0];

        DeleteConnectedVerticesAction action = new(file.Meshes[0], 2, () => { });
        action.Execute();

        action.Undo();
        Assert.Equal(expectedMesh.Vertices.Count, mesh.Vertices.Count);

        FlverTestHelper.Equal(expected, file);
    }
}
