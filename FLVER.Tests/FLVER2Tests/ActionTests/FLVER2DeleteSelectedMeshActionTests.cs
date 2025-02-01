using System.Numerics;
using FLVER_Editor.Actions;
using SoulsFormats;
using Xunit;

namespace FLVERS.Tests.FLVER2Tests.ActionTests;

public class FLVER2DeleteSelectedMeshActionTests : IClassFixture<DataFixture>
{
    private readonly DataFixture dataFixture;

    public FLVER2DeleteSelectedMeshActionTests(DataFixture dataFixture)
    {
        this.dataFixture = dataFixture;
    }

    [Fact]
    public void DeleteMeshRestoreItCheck()
    {
        FLVER2 file = FLVER2.Read(dataFixture.Flver2_1);
        FLVER2 expected = dataFixture.Flver2_1_Read;

        var selectedMesh = new List<FLVER2.Mesh>() { file.Meshes[0] };
        var selectedDummies = new List<FLVER.Dummy>() { file.Dummies[0] };

        DeleteSelectedMeshAction action = new(file, selectedMesh, selectedDummies, false, () => { });
        action.Execute();

        Assert.NotEqual(expected.Meshes.Count, file.Meshes.Count);
        action.Undo();

        Assert.Equal(expected.Meshes.Count, file.Meshes.Count);


        FlverTestHelper.Equal(expected, file);
    }

    [Fact]
    public void DeleteMeshFaceSetsRestoreItCheck()
    {
        FLVER2 file = FLVER2.Read(dataFixture.Flver2_1);
        FLVER2 expected = dataFixture.Flver2_1_Read;

        var selectedMesh = new List<FLVER2.Mesh>() { file.Meshes[0] };
        var selectedDummies = new List<FLVER.Dummy>() { file.Dummies[0] };

        DeleteSelectedMeshAction action = new(file, selectedMesh, selectedDummies, true, () => { });
        action.Execute();

        Assert.Equal(expected.Meshes.Count, file.Meshes.Count);
        action.Undo();

        Assert.Equal(expected.Meshes.Sum(x => x.FaceSets.Count), file.Meshes.Sum(x => x.FaceSets.Count));
        Assert.Equal(expected.Meshes.Count, file.Meshes.Count);


        FlverTestHelper.Equal(expected, file);
    }
}
