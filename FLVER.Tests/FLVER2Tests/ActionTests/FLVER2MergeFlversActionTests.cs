using System.Numerics;
using FLVER_Editor.Actions;
using SoulsFormats;
using Xunit;

namespace FLVERS.Tests.FLVER2Tests.ActionTests;

public class FLVER2MergeFlversActionTests : IClassFixture<DataFixture>
{
    private readonly DataFixture dataFixture;

    public FLVER2MergeFlversActionTests(DataFixture dataFixture)
    {
        this.dataFixture = dataFixture;
    }

    [Fact]
    public void MergeFlversWithNoBNDAndNoTPFThenUndo()
    {
        var expected = dataFixture.Flver2_1_Double_Fused_Read;
        var undoExpected = dataFixture.Flver2_1_Read;

        var file = FLVER2.Read(dataFixture.Flver2_1);
        var mergedInFile = FLVER2.Read(dataFixture.Flver2_1);

        MergeFlversAction action = new(file, mergedInFile, null, null, () => {});
        action.Execute();

        FlverTestHelper.Equal(expected, file);
        action.Undo();

        FlverTestHelper.Equal(undoExpected, file);
    }
}
