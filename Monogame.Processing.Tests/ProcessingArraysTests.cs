using System.Runtime.CompilerServices;
using Xunit;

namespace Monogame.Processing.Tests;

public class ProcessingArraysTests
{
    private sealed class TestProcessing : Processing
    {
    }

    [Theory]
    [InlineData(0, new[] { 9, 1, 2, 3 })]
    [InlineData(2, new[] { 1, 2, 9, 3 })]
    [InlineData(3, new[] { 1, 2, 3, 9 })]
    public void SpliceSingleValueInsertsAtRequestedIndex(int index, int[] expected)
    {
        var processing = (TestProcessing)RuntimeHelpers.GetUninitializedObject(typeof(TestProcessing));

        var result = processing.splice([1, 2, 3], 9, index);

        Assert.Equal(expected, result);
    }
}
