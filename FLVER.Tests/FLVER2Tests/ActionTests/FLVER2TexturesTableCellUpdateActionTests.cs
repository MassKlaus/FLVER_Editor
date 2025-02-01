using System.Numerics;
using FLVER_Editor.Actions;
using SoulsFormats;
using Xunit;

namespace FLVERS.Tests.FLVER2Tests.ActionTests;

public class FLVER2TexturesTableCellUpdateActionTests : IClassFixture<DataFixture>
{
    private readonly DataFixture dataFixture;

    public FLVER2TexturesTableCellUpdateActionTests(DataFixture dataFixture)
    {
        this.dataFixture = dataFixture;
    }

    [Fact]
    public void UnWrittenTest()
    {
        // This is a placeholder for future implementation.
        // Ensure that the actual logic is implemented later.
        Assert.Fail("This test needs to be implemented.");
    }
}
