namespace Basmus.Parametrization.Tests;

[TestClass]
public class PositionalParametrizerTests
{
    [TestMethod]
    public void Apply_WithOutParameters_ShouldNotThrow()
    {
        // Arrange
        var parameterKeyValueBuilder = new UniquePositionalPlaceholderBuilder();
        var parametrizer = new PositionalParametrizer(parameterKeyValueBuilder);

        // Act
        var act = () => parametrizer.PreparePlaceholders(Array.Empty<Parameter>());

        // Assert
        act.Should().NotThrow();
    }

    [TestMethod]
    public void Apply_WithParameters_WithUniquePlaceholders_PlaceholdersAndIndexesShouldBeCorrect()
    {
        // Arrange
        var parameterKeyValueBuilder = new UniquePositionalPlaceholderBuilder();
        var parametrizer = new PositionalParametrizer(parameterKeyValueBuilder);
        var parameter1 = new Parameter("id", 1);
        var parameter2 = new Parameter(2.3);
        var parameter3 = new Parameter("date", DateTime.Now);
        var parameter4 = new Parameter("string value");

        // Act
        parametrizer.PreparePlaceholders([parameter1, parameter2, parameter3, parameter4]);

        // Assert
        parameter1.HasPlaceholder.Should().BeTrue();
        parameter1.Placeholder.Value.Should().Be("$0");
        parameter1.Placeholder.Index.Should().Be(0);
        parameter2.HasPlaceholder.Should().BeTrue();
        parameter2.Placeholder.Value.Should().Be("$1");
        parameter2.Placeholder.Index.Should().Be(1);
        parameter3.HasPlaceholder.Should().BeTrue();
        parameter3.Placeholder.Value.Should().Be("$2");
        parameter3.Placeholder.Index.Should().Be(2);
        parameter4.HasPlaceholder.Should().BeTrue();
        parameter4.Placeholder.Value.Should().Be("$3");
        parameter4.Placeholder.Index.Should().Be(3);
    }

    [TestMethod]
    public void Apply_WithParameters_WithSamePlaceholders_PlaceholdersAndIndexesShouldBeCorrect()
    {
        // Arrange
        var parameterKeyValueBuilder = new SamePositionalPlaceholderBuilder();
        var parametrizer = new PositionalParametrizer(parameterKeyValueBuilder);
        var parameter1 = new Parameter("id", 1);
        var parameter2 = new Parameter(2.3);
        var parameter3 = new Parameter("date", DateTime.Now);
        var parameter4 = new Parameter("string value");
        // Act
        parametrizer.PreparePlaceholders([parameter1, parameter2, parameter3, parameter4]);
        // Assert
        parameter1.HasPlaceholder.Should().BeTrue();
        parameter1.Placeholder.Value.Should().Be("?");
        parameter1.Placeholder.Index.Should().Be(0);
        parameter2.HasPlaceholder.Should().BeTrue();
        parameter2.Placeholder.Value.Should().Be("?");
        parameter2.Placeholder.Index.Should().Be(1);
        parameter3.HasPlaceholder.Should().BeTrue();
        parameter3.Placeholder.Value.Should().Be("?");
        parameter3.Placeholder.Index.Should().Be(2);
        parameter4.HasPlaceholder.Should().BeTrue();
        parameter4.Placeholder.Value.Should().Be("?");
        parameter4.Placeholder.Index.Should().Be(3);
    }

    public class UniquePositionalPlaceholderBuilder : IPositionalPlaceholderBuilder
    {
        public string Build(int index)
        {
            return $"${index}";
        }
    }

    public class SamePositionalPlaceholderBuilder : IPositionalPlaceholderBuilder
    {
        public string Build(int index)
        {
            return "?";
        }
    }
}
