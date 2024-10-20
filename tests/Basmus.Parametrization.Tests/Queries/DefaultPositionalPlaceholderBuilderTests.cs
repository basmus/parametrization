using Basmus.Parametrization.Queries;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Basmus.Parametrization.Tests.Queries;

[TestClass]
public class DefaultPositionalPlaceholderBuilderTests
{
    [TestMethod]
    public void Build_WithPrefix_ShouldReturnSymbolWithIndex()
    {
        // Arrange
        var builder = new DefaultPositionalPlaceholderBuilder(":", true);
        var index = 1;

        // Act
        var result = builder.Build(index);

        // Assert
        result.Should().Be(":1");
    }

    [TestMethod]
    public void Build_WithoutPrefix_ShouldReturnSymbolOnly()
    {
        // Arrange
        var builder = new DefaultPositionalPlaceholderBuilder("?", false);
        var index = 1;

        // Act
        var result = builder.Build(index);

        // Assert
        result.Should().Be("?");
    }
} 