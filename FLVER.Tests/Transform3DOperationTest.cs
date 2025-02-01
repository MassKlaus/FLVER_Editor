using System.Numerics;
using FLVER_Editor;

namespace FLVERS.Tests;


public class Transform3DOperationsTest
{
    [Theory]
    [InlineData(1, 2, 3, 1, TransformAxis.X, 2, 2, 3)]
    [InlineData(1, 2, 3, -1, TransformAxis.Y, 1, 1, 3)]
    [InlineData(1, 2, 3, 5, TransformAxis.Z, 1, 2, 8)]
    public void CreateTranslationVector_WorksCorrectly(float x, float y, float z, float offset, TransformAxis axis, float ex, float ey, float ez)
    {
        var expected = Transform3DOperations.CreateTranslationVector(x, y, z, offset, (int)axis);
        var result = Transform3DOperations2.CreateTranslationVector(x, y, z, offset, axis);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(1, 2, 3, 0.5f, new float[] { 0, 0, 0 }, TransformAxis.X, false, false, 0.5f, 2, 3)]
    [InlineData(2, 4, 6, 2, new float[] { 0, 0, 0 }, TransformAxis.Y, false, false, 2, 8, 6)]
    [InlineData(3, 6, 9, -1, new float[] { 0, 0, 0 }, TransformAxis.Z, false, true, 3, 6, 4.5f)]
    public void CreateScaleVector_WorksCorrectly(float x, float y, float z, float offset, float[] totals, TransformAxis axis, bool uniform, bool invert, float ex, float ey, float ez)
    {
        var expected = Transform3DOperations.CreateScaleVector(x, y, z, offset, totals, (int)axis, uniform, invert);
        var vecTotals = new Vector3(totals[0], totals[1], totals[2]);
        var result = Transform3DOperations2.CreateScaleVector(x, y, z, offset, vecTotals, axis, uniform, invert);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(1, 0, 0, 1, 90, new float[] { 0, 0, 15 }, TransformAxis.Y)]
    [InlineData(0, 1, 0, 1, 180, new float[] { 0, 5, 0 }, TransformAxis.X)]
    [InlineData(0, 0, 1, 1, 180, new float[] { 2, 0, 0 }, TransformAxis.X)]
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

    [Theory]
    [InlineData(1, 0, 0, 90, new float[] { 0, 0, 0 }, TransformAxis.Y)]
    [InlineData(0, 1, 0, 180, new float[] { 0, 0, 0 }, TransformAxis.X)]
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