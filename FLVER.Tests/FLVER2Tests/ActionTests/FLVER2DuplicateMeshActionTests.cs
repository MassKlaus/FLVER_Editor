using System.Numerics;
using FLVER_Editor.Actions;
using SoulsFormats;
using Xunit;

namespace FLVERS.Tests.FLVER2Tests.ActionTests;

public class FLVER2DuplicateMeshActionTests : IClassFixture<DataFixture>
{
    private readonly DataFixture dataFixture;

    public FLVER2DuplicateMeshActionTests(DataFixture dataFixture)
    {
        this.dataFixture = dataFixture;
    }

    [Fact]
    public void DuplicateMeshAndRemoveIt()
    {
        FLVER2 file = FLVER2.Read(dataFixture.Flver2_1);
        FLVER2 expected = dataFixture.Flver2_1_Read;

        var mesh = file.Meshes[0];
        var expectedMesh = expected.Meshes[0];

        DuplicateMeshAction action = new(file, mesh, 1, () => {});
        action.Execute();

        Assert.Equal(expected.Meshes.Count + 1, file.Meshes.Count);

        var copiedMesh = file.Meshes[0];
        var duplicateMesh = file.Meshes[1];

        FlverTestHelper.EqualNoMaterialCheck(copiedMesh, duplicateMesh);
        action.Undo();

        Assert.Equal(expected.Meshes.Count, file.Meshes.Count);

        FlverTestHelper.Equal(expected, file);
    }
}
