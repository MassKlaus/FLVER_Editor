using SoulsFormats;

namespace FLVERS.Tests.FLVER2Tests;

public class FLVER2LoadSaveTests : IClassFixture<DataFixture>
{
    private readonly DataFixture dataFixture;

    public FLVER2LoadSaveTests(DataFixture dataFixture)
    {
        this.dataFixture = dataFixture;
    }

    [Fact]
    public void LoadDCX()
    {
        if (FLVER2.IsRead(dataFixture.Flver2_1, out FLVER2 file))
        {
            FlverTestHelper.Equal(dataFixture.Flver2_1_Read, file);
            return;
        }

        Assert.Fail("Failed to load the FLVER");
    }
}
