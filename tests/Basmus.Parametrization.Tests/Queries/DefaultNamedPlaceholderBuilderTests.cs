using Basmus.Parametrization.Queries;

namespace Basmus.Parametrization.Tests.Queries;

[TestClass]
public class DefaultNamedPlaceholderBuilderTests
{
    [TestMethod]
    public void Build_WithNameAndNoDuplicateIndex_ShouldReturnPrefixedName()
    {
        // Arrange
        var builder = new DefaultNamedPlaceholderBuilder("@", "p");
        var name = "test";

        // Act
        var result = builder.Build(name, null);

        // Assert
        result.Should().Be("@test");
    }

    [TestMethod]
    public void Build_WithNameAndDuplicateIndex_ShouldReturnPrefixedNameWithIndex()
    {
        // Arrange
        var builder = new DefaultNamedPlaceholderBuilder("@", "p");
        var name = "test";
        var duplicateIndex = 1;

        // Act
        var result = builder.Build(name, duplicateIndex);

        // Assert
        result.Should().Be("@test1");
    }

    [TestMethod]
    public void Build_WithUnnamedParameterIndex_ShouldReturnPrefixedParameterNameWithIndex()
    {
        // Arrange
        var builder = new DefaultNamedPlaceholderBuilder("@", "p");
        var index = 1;

        // Act
        var result = builder.Build(index);

        // Assert
        result.Should().Be("@p1");
    }
} 