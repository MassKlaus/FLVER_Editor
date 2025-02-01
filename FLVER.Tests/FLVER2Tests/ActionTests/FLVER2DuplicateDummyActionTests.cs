using System.Numerics;
using FLVER_Editor.Actions;
using SoulsFormats;
using Xunit;

namespace FLVERS.Tests.FLVER2Tests.ActionTests;

public class FLVER2DuplicateDummyActionTests : IClassFixture<DataFixture>
{
    private readonly DataFixture dataFixture;

    public FLVER2DuplicateDummyActionTests(DataFixture dataFixture)
    {
        this.dataFixture = dataFixture;
    }

    [Fact]
    public void DuplicateDummyAndThenRemoveIt()
    {
        FLVER2 file = FLVER2.Read(dataFixture.Flver2_1);
        FLVER2 expected = dataFixture.Flver2_1_Read;

        var copiedDummy = file.Dummies[0];

        DuplicateDummyAction action = new(file, copiedDummy, 0, () => {});
        action.Execute();

        Assert.Equal(expected.Dummies.Count + 1, file.Dummies.Count);

        var addedDummy = file.Dummies[0];

        FlverTestHelper.Equal(copiedDummy, addedDummy);
        action.Undo();

        Assert.Equal(expected.Dummies.Count, file.Dummies.Count);

        FlverTestHelper.Equal(expected, file);
    }
}
