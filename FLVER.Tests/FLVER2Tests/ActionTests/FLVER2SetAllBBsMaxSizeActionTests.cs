using System.Numerics;
using FLVER_Editor.Actions;
using SoulsFormats;
using Xunit;

namespace FLVERS.Tests.FLVER2Tests.ActionTests;

public class FLVER2SetAllBBsMaxSizeActionTests : IClassFixture<DataFixture>
{
    private readonly DataFixture dataFixture;

    public FLVER2SetAllBBsMaxSizeActionTests(DataFixture dataFixture)
    {
        this.dataFixture = dataFixture;
    }

    [Fact]
    public void ChangingSizeForBBsThenReturningIt()
    {
        var file = FLVER2.Read(dataFixture.Flver2_1);
        var expected = dataFixture.Flver2_1_Read;

        SetAllBBsMaxSizeAction action = new(file, () => { });
        action.Execute();

        FlverTestHelper.Equal(SetAllBBsMaxSizeAction.minVector, file.Header.BoundingBoxMin);
        FlverTestHelper.Equal(SetAllBBsMaxSizeAction.maxVector, file.Header.BoundingBoxMax);

        foreach (var mesh in file.Meshes)
        {
            FlverTestHelper.Equal(SetAllBBsMaxSizeAction.minVector, mesh.BoundingBox.Min);
            FlverTestHelper.Equal(SetAllBBsMaxSizeAction.maxVector, mesh.BoundingBox.Max);
        }

        action.Undo();
        FlverTestHelper.Equal(expected, file);
    }
}
