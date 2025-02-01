using SoulsFormats;

namespace FLVERS.Tests.BND3Tests;

public class BND3LoadSaveTests : IClassFixture<DataFixture>
{
    private readonly DataFixture dataFixture;

    public BND3LoadSaveTests(DataFixture dataFixture)
    {
        this.dataFixture = dataFixture;
    }

    [Fact]
    public void LoadDCX()
    {
        if (BND3.IsRead(dataFixture.Bnd3_1, out BND3 file))
        {
            Assert.Equal(dataFixture.Bnd3_1, file.Write());
            return;
        }

        Assert.Fail("Failed to load the BND");
    }
}
