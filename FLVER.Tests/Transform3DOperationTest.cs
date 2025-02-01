using System.Numerics;
using FLVER_Editor;

namespace FLVERS.Tests;


public class Transform3DOperationsTest
{
    // Translation Tests
    [Theory]
    [InlineData(1, 2, 3, 0, TransformAxis.X, 1, 2, 3)] // No translation
    [InlineData(1, 2, 3, -3, TransformAxis.Y, 1, -1, 3)] // Negative translation
    [InlineData(0, 0, 0, 10, TransformAxis.Z, 0, 0, 10)] // Origin translation
    [InlineData(1.5f, -2.5f, 3.5f, 2.5f, TransformAxis.X, 4, -2.5f, 3.5f)] // Decimal translation
    public void CreateTranslationVector_WorksCorrectly(float x, float y, float z, float offset, TransformAxis axis, float ex, float ey, float ez)
    {
        var expected = Transform3DOperations.CreateTranslationVector(x, y, z, offset, (int)axis);
        var result = Transform3DOperations2.CreateTranslationVector(x, y, z, offset, axis);
        Assert.Equal(expected, result);
    }

    // Scaling Tests
    [Theory]
    [InlineData(2, 3, 4, 1.5f, new float[] { 0, 0, 0 }, TransformAxis.X, true, false, 3, 4.5f, 6)] // Uniform scaling
    [InlineData(2, 3, 4, -2, new float[] { 0, 0, 0 }, TransformAxis.Y, false, false, 2, -6, 4)] // Negative scaling
    [InlineData(1, 1, 1, 0, new float[] { 0, 0, 0 }, TransformAxis.Z, false, false, 1, 1, 1)] // No scaling
    [InlineData(2, 4, 6, 0.5f, new float[] { 5, 5, 5 }, TransformAxis.Y, false, true, 2, 2, 6)] // Inverted scaling
    public void CreateScaleVector_WorksCorrectly(float x, float y, float z, float offset, float[] totals, TransformAxis axis, bool uniform, bool invert, float ex, float ey, float ez)
    {
        var expected = Transform3DOperations.CreateScaleVector(x, y, z, offset, totals, (int)axis, uniform, invert);
        var vecTotals = new Vector3(totals[0], totals[1], totals[2]);
        var result = Transform3DOperations2.CreateScaleVector(x, y, z, offset, vecTotals, axis, uniform, invert);
        Assert.Equal(expected, result);
    }


    // Rotation Tests (Quaternion-based)
    [Theory]
    [InlineData(1, 0, 0, 1, 90, new float[] { 0, 0, 0 }, TransformAxis.Y)]
    [InlineData(0, 1, 0, 1, 180, new float[] { 5, 0, 0 }, TransformAxis.Z)]
    [InlineData(0, 0, 1, 1, 270, new float[] { 0, 5, 0 }, TransformAxis.X)]
    [InlineData(1, 1, 1, 1, 360, new float[] { 10, 10, 10 }, TransformAxis.Y)]
    public void CreateRotationVector_WorksCorrectly_Vec4(float x, float y, float z, float w, float offset, float[] totals, TransformAxis axis)
    {
        var expected = Transform3DOperations.CreateRotationVector(x, y, z, w, offset, totals, (int)axis);

        var vecTotals = new Vector3(totals[0], totals[1], totals[2]);
        var result = Transform3DOperations2.CreateRotationVector(new Vector4(x, y, z, w), offset, vecTotals, axis);

        if (expected is Vector4 vector4)
        {
            Assert.Equal(vector4, result);
            return;
        }

        Assert.Fail("Wrong dynamic type result");
    }

    [Theory]
    [InlineData(1, 0, 0, 0, 90, new float[] { 0, 0, 15 }, TransformAxis.Y)]
    [InlineData(0, 1, 0, 0, 180, new float[] { 0, 5, 0 }, TransformAxis.X)]
    [InlineData(0, 0, 1, 0, 180, new float[] { 2, 0, 0 }, TransformAxis.X)]
    public void CreateRotationVector2ErrorWhenWisZero_Vec4(float x, float y, float z, float w, float offset, float[] totals, TransformAxis axis)
    {
        var vecTotals = new Vector3(totals[0], totals[1], totals[2]);
        Assert.Throws<ArgumentException>(() => Transform3DOperations2.CreateRotationVector(new Vector4(x, y, z, w), offset, vecTotals, axis));
    }

    // Rotation Tests (Vector3-based)
    [Theory]
    [InlineData(1, 0, 0, 90, new float[] { 0, 0, 0 }, TransformAxis.Y)]
    [InlineData(0, 1, 0, 180, new float[] { 0, 0, 0 }, TransformAxis.X)]
    [InlineData(0, 0, 1, 270, new float[] { 0, 0, 0 }, TransformAxis.Z)]
    [InlineData(1, 1, 0, 360, new float[] { 5, 5, 0 }, TransformAxis.Y)]
    public void CreateRotationVector_WorksCorrectly_Vec3(float x, float y, float z, float offset, float[] totals, TransformAxis axis)
    {
        var expected = Transform3DOperations.CreateRotationVector(x, y, z, 0, offset, totals, (int)axis);

        var vecTotals = new Vector3(totals[0], totals[1], totals[2]);
        var result = Transform3DOperations2.CreateRotationVector(new Vector3(x, y, z), offset, vecTotals, axis);

        if (expected is Vector3 vector3)
        {
            Assert.Equal(vector3, result);
            return;
        }

        Assert.Fail("Wrong dynamic type result");
    }
}