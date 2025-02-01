using System.Numerics;
using FLVER_Editor;
using FLVER_Editor.Actions;
using SoulsFormats;
using Xunit;

namespace FLVERS.Tests.FLVER2Tests.ActionTests;

public class FLVER2MirrorMeshActionTests : IClassFixture<DataFixture>
{
    private readonly DataFixture dataFixture;

    public FLVER2MirrorMeshActionTests(DataFixture dataFixture)
    {
        this.dataFixture = dataFixture;
    }

    [Theory]
    [InlineData(TransformAxis.X)]
    [InlineData(TransformAxis.Y)]
    [InlineData(TransformAxis.Z)]
    public void MirrorMeshThroughWorldOriginWithoutVector(TransformAxis axis)
    {
        var file = FLVER2.Read(dataFixture.Flver2_1);
        var expected = dataFixture.Flver2_1_Read;

        MirrorMeshAction action = new(file.Meshes, file.Dummies, axis, true, false, () => { });
        action.Execute();

        for (int i = 0; i < expected.Meshes.Count; i++)
        {
            for (int j = 0; j < expected.Meshes[i].Vertices.Count; j++)
            {
                var verticePosition = file.Meshes[i].Vertices[j].Position;
                verticePosition[(int)axis] *= -1;

                FlverTestHelper.Equal(expected.Meshes[i].Vertices[j].Position, verticePosition);
            }
        }

        for (int i = 0; i < expected.Dummies.Count; i++)
        {
            var dummyPosition = file.Dummies[i].Position;
            dummyPosition[(int)axis] *= -1;

            FlverTestHelper.Equal(expected.Dummies[i].Position, dummyPosition);
        }

        action.Undo();

        for (int i = 0; i < expected.Meshes.Count; i++)
        {
            FlverTestHelper.Equal(expected.Meshes[i], file.Meshes[i]);
        }

        for (int i = 0; i < expected.Dummies.Count; i++)
        {
            FlverTestHelper.Equal(expected.Dummies[i], file.Dummies[i]);
        }
    }

    [Theory]
    [InlineData(TransformAxis.X)]
    [InlineData(TransformAxis.Y)]
    [InlineData(TransformAxis.Z)]
    public void MirrorMeshThroughWorldOriginWithVector(TransformAxis axis)
    {
        var file = FLVER2.Read(dataFixture.Flver2_1);
        var expected = dataFixture.Flver2_1_Read;

        MirrorMeshAction action = new(file.Meshes, file.Dummies, axis, true, true, () => { });
        action.Execute();

        for (int i = 0; i < expected.Meshes.Count; i++)
        {
            for (int j = 0; j < expected.Meshes[i].Vertices.Count; j++)
            {
                var verticePosition = file.Meshes[i].Vertices[j].Position;
                verticePosition[(int)axis] *= -1; 

                FlverTestHelper.Equal(expected.Meshes[i].Vertices[j].Position, verticePosition);
            }
        }

        for (int i = 0; i < expected.Dummies.Count; i++)
        {
            var dummyPosition = file.Dummies[i].Forward;
            dummyPosition[(int)axis] *= -1;

            FlverTestHelper.Equal(expected.Dummies[i].Forward, dummyPosition);
        }

        action.Undo();

        for (int i = 0; i < expected.Meshes.Count; i++)
        {
            FlverTestHelper.Equal(expected.Meshes[i], file.Meshes[i]);
        }

        for (int i = 0; i < expected.Dummies.Count; i++)
        {
            FlverTestHelper.Equal(expected.Dummies[i], file.Dummies[i]);
        }
    }

        [Theory]
    [InlineData(TransformAxis.X)]
    [InlineData(TransformAxis.Y)]
    [InlineData(TransformAxis.Z)]
    public void MirrorMeshWithoutWorldOriginWithVector(TransformAxis axis)
    {
        var file = FLVER2.Read(dataFixture.Flver2_1);
        var expected = dataFixture.Flver2_1_Read;

        var floats = MeshHelpers.CalculateMeshCenter(file.Meshes, false);
        MirrorMeshAction action = new(file.Meshes, file.Dummies, axis, false, true, () => { });
        action.Execute();

        for (int i = 0; i < expected.Meshes.Count; i++)
        {
            for (int j = 0; j < expected.Meshes[i].Vertices.Count; j++)
            {
                var verticePosition = MirrorThing(file.Meshes[i].Vertices[j].Position, axis, floats, false);
                FlverTestHelper.Equal(expected.Meshes[i].Vertices[j].Position, verticePosition);
            }
        }

        for (int i = 0; i < expected.Dummies.Count; i++)
        {
            var dummyPosition = MirrorThing(file.Dummies[i].Forward, axis, floats, false);

            FlverTestHelper.Equal(expected.Dummies[i].Forward, dummyPosition);
        }

        action.Undo();

        for (int i = 0; i < expected.Meshes.Count; i++)
        {
            FlverTestHelper.Equal(expected.Meshes[i], file.Meshes[i]);
        }

        for (int i = 0; i < expected.Dummies.Count; i++)
        {
            FlverTestHelper.Equal(expected.Dummies[i], file.Dummies[i]);
        }
    }

    [Theory]
    [InlineData(TransformAxis.X)]
    [InlineData(TransformAxis.Y)]
    [InlineData(TransformAxis.Z)]
    public void MirrorMeshWithoutWorldOriginNoVector(TransformAxis axis)
    {
        var file = FLVER2.Read(dataFixture.Flver2_1);
        var expected = dataFixture.Flver2_1_Read;

        var floats = MeshHelpers.CalculateMeshCenter(file.Meshes, false);
        MirrorMeshAction action = new(file.Meshes, file.Dummies, axis, false, false, () => { });
        action.Execute();

        for (int i = 0; i < expected.Meshes.Count; i++)
        {
            for (int j = 0; j < expected.Meshes[i].Vertices.Count; j++)
            {
                var verticePosition = MirrorThing(file.Meshes[i].Vertices[j].Position, axis, floats, false);
                var expectedVertice = expected.Meshes[i].Vertices[j].Position; 
                FlverTestHelper.Equal(expectedVertice, verticePosition);
            }
        }

        for (int i = 0; i < expected.Dummies.Count; i++)
        {
            var dummyPosition = MirrorThing(file.Dummies[i].Position, axis, floats, false);
            FlverTestHelper.Equal(expected.Dummies[i].Position, dummyPosition);
        }

        action.Undo();

        for (int i = 0; i < expected.Meshes.Count; i++)
        {
            FlverTestHelper.Equal(expected.Meshes[i], file.Meshes[i]);
        }

        for (int i = 0; i < expected.Dummies.Count; i++)
        {
            FlverTestHelper.Equal(expected.Dummies[i], file.Dummies[i]);
        }
    }


    private Vector3 MirrorThing(Vector3 v, TransformAxis axis, IReadOnlyList<float> totals, bool useWorldOrigin)
    {
        // center to world 
        v[(int)axis] -= !useWorldOrigin ? totals[(int)axis] : 0;
        // flip
        v[(int)axis] *= -1;
        // uncenter to world 
        v[(int)axis] += !useWorldOrigin ? totals[(int)axis] : 0;

        return v;
    }
}
