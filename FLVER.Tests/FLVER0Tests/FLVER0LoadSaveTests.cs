using SoulsFormats;

namespace FLVERS.Tests.FLVER0Tests;

public class FLVER0LoadSaveTests : IClassFixture<DataFixture>
{
    private readonly DataFixture dataFixture;

    public FLVER0LoadSaveTests(DataFixture dataFixture)
    {
        this.dataFixture = dataFixture;
    }

    [Fact]
    public void LoadDCX()
    {
        if (FLVER0.IsRead(dataFixture.Flver0_1, out FLVER0 file))
        {
            FlverTestHelper.Equal(dataFixture.Flver0_1_Read, file);
            return;
        }

        Assert.Fail("Failed to load the FLVER");
    }
}