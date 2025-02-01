using System.Numerics;
using FLVER_Editor;
using FLVER_Editor.Actions;
using SoulsFormats;
using Xunit;

namespace FLVERS.Tests.FLVER2Tests.ActionTests;

public class FLVER2MeshTansformActionTests : IClassFixture<DataFixture>
{
    private readonly DataFixture dataFixture;

    public FLVER2MeshTansformActionTests(DataFixture dataFixture)
    {
        this.dataFixture = dataFixture;
    }

    [Theory]
    [InlineData(10, TransformAxis.X)]
    [InlineData(10, TransformAxis.Y)]
    [InlineData(10, TransformAxis.Z)]
    public void TranslateActionThenUndo(float offset, TransformAxis axis)
    {
        FLVER2 file = FLVER2.Read(dataFixture.Flver2_1);
        var expected = dataFixture.Flver2_1_Read;

        MeshTansformAction action = new(file.Meshes, file.Dummies, offset, axis, 0, 0, (_, _, _, _) => { });
        action.Execute();

        var gap = Math.Round(offset / 55, 2);
        for (int j = 0; j < file.Meshes.Count; j++)
        {
            var mesh = file.Meshes[j];
            var expectedMesh = expected.Meshes[j];

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                var expectedVertice = expectedMesh.Vertices[i];
                var vertice = mesh.Vertices[i];
                Assert.NotEqual(expectedVertice.Position.Get(axis), vertice.Position.Get(axis), 0.001);
            }
        }

        for (int i = 0; i < file.Dummies.Count; i++)
        {
            var expectedVertice = expected.Dummies[i];
            var vertice = file.Dummies[i];
            Assert.NotEqual(expectedVertice.Position.Get(axis), vertice.Position.Get(axis), 0.001);
        }

        action.Undo();

        for (int j = 0; j < file.Meshes.Count; j++)
        {
            var mesh = file.Meshes[j];
            var expectedMesh = expected.Meshes[j];
            FlverTestHelper.Equal(expectedMesh, mesh);
        }

        for (int j = 0; j < file.Dummies.Count; j++)
        {
            var dummy = file.Dummies[j];
            var expectedDummy = expected.Dummies[j];
            FlverTestHelper.Equal(expectedDummy, dummy);
        }

        FlverTestHelper.Equal(expected, file);
    }
}
