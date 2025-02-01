using System.Numerics;
using FLVER_Editor.Actions;
using FLVER_Editor.FbxImporter.ViewModels;
using SoulsFormats;
using Xunit;

namespace FLVERS.Tests.FLVER2Tests.ActionTests;

public class FLVER2ImportMeshesToFlverActionTests : IClassFixture<DataFixture>
{
    private readonly DataFixture dataFixture;

    public FLVER2ImportMeshesToFlverActionTests(DataFixture dataFixture)
    {
        this.dataFixture = dataFixture;
    }

    [Fact]
    public void ImportFBXThenUndoTestToFlverTest()
    {
        FLVER2 file = FLVER2.Read(dataFixture.Flver2_1);
        var original = dataFixture.Flver2_1_Read;
        var original_imported = dataFixture.Flver2_1_Fbx_Imported_Read;

        Dictionary<FbxMeshDataViewModel, MeshImportOptions> meshData = dataFixture.GetFbxDictionary(file);
        ImportMeshesToFlverAction action = new(file, meshData, () => { });
        action.Execute();

        FlverTestHelper.Equal(original_imported, file);
        action.Undo();

        FlverTestHelper.Equal(original, file);
    }
}
