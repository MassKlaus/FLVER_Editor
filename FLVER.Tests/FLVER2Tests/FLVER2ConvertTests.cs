using FLVER_Editor;

namespace FLVERS.Tests.FLVER2Tests;

public class FLVER2ConvertTests : IClassFixture<DataFixture>
{
    
    private readonly DataFixture dataFixture;

    public FLVER2ConvertTests(DataFixture dataFixture)
    {
        this.dataFixture = dataFixture;
    }

    [Fact]
    public void ConvertFlver2To0ThenTo2() 
    {
        var flver0 = FLVERConverter.ConvertToFLVER0(dataFixture.Flver2_1_Read);
        var flver2 = FLVERConverter.Convert(flver0);

        FlverTestHelper.Equal(dataFixture.Flver2_1_Read, flver2);
    }
}
