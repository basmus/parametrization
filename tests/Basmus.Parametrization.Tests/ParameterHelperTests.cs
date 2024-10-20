namespace Basmus.Parametrization.Tests;

[TestClass]
public class ParameterHelperTests
{
    // Named Placeholder Builder Tests
    [TestMethod]
    public void Parametrize_ReturnsEmpty_WithNamedPlaceholderBuilder_WithoutParameters()
    {
        // Arrange
        FormattableString formattableString = $"This is a simple message";
        var namedPlaceholderBuilder = new ProperNamedPlaceholderBuilder();

        // Act
        var result = ParameterHelper.Parametrize(formattableString, namedPlaceholderBuilder);

        // Assert
        result.Should().BeEmpty();
        formattableString.ToString().Should().Be($"This is a simple message");
    }

    [TestMethod]
    public void Parametrize_ReturnsEmpty_WithNamedPlaceholderBuilder_WithEmptyString()
    {
        // Arrange
        FormattableString formattableString = $"";
        var namedPlaceholderBuilder = new ProperNamedPlaceholderBuilder();

        // Act
        var result = ParameterHelper.Parametrize(formattableString, namedPlaceholderBuilder);

        // Assert
        result.Should().BeEmpty();
        formattableString.ToString().Should().Be($"");
    }

    [TestMethod]
    public void Parametrize_ReturnsCorrectParameters_WithNamedPlaceholderBuilder_WithNamedParameters()
    {
        // Arrange
        var id = 1;
        var name = "John";
        var date = DateTime.Now;
        FormattableString formattableString = $"User {name.AsParameter("name")} with ID {id.AsParameter("id")} was created on {date.AsParameter("date")}";
        var namedPlaceholderBuilder = new ProperNamedPlaceholderBuilder();

        // Act
        var result = ParameterHelper.Parametrize(formattableString, namedPlaceholderBuilder);

        // Assert
        result.Should().HaveCount(3);
        result.Should().ContainKeys("@id", "@name", "@date");
        result["@id"].Should().Be(1);
        result["@name"].Should().Be("John");
        result["@date"].Should().Be(date);
        formattableString.ToString().Should().Be($"User @name with ID @id was created on @date");
    }

    [TestMethod]
    public void Parametrize_ReturnsCorrectParameters_WithNamedPlaceholderBuilder_WithUnnamedParameters()
    {
        // Arrange
        var id = 1;
        var name = "John";
        FormattableString formattableString = $"User {name.AsParameter()} with ID {id.AsParameter()}";
        var namedPlaceholderBuilder = new ProperNamedPlaceholderBuilder();

        // Act
        var result = ParameterHelper.Parametrize(formattableString, namedPlaceholderBuilder);

        // Assert
        result.Should().HaveCount(2);
        result.Should().ContainKeys("@p0", "@p1");
        result["@p0"].Should().Be("John");
        result["@p1"].Should().Be(1);
        formattableString.ToString().Should().Be($"User @p0 with ID @p1");
    }

    [TestMethod]
    public void Parametrize_ReturnsCorrectParameters_WithNamedPlaceholderBuilder_WithMixedParameters()
    {
        // Arrange
        var id = 1;
        var name = "John";
        var date = DateTime.Now;
        FormattableString formattableString = $"User {name.AsParameter("name")} with ID {id.AsParameter()} was created on {date.AsParameter("date")}";
        var namedPlaceholderBuilder = new ProperNamedPlaceholderBuilder();

        // Act
        var result = ParameterHelper.Parametrize(formattableString, namedPlaceholderBuilder);

        // Assert
        result.Should().HaveCount(3);
        result.Should().ContainKeys("@name", "@p0", "@date");
        result["@name"].Should().Be("John");
        result["@p0"].Should().Be(1);
        result["@date"].Should().Be(date);
        formattableString.ToString().Should().Be($"User @name with ID @p0 was created on @date");
    }

    [TestMethod]
    public void Parametrize_ReturnsEmpty_WithPositionalPlaceholderBuilder_WithoutParameters()
    {
        // Arrange
        FormattableString formattableString = $"This is a simple message";
        var positionalPlaceholderBuilder = new ProperPositionalPlaceholderBuilder();

        // Act
        var result = ParameterHelper.Parametrize(formattableString, positionalPlaceholderBuilder);

        // Assert
        result.Should().BeEmpty();
        formattableString.ToString().Should().Be($"This is a simple message");
    }

    [TestMethod]
    public void Parametrize_ReturnsEmpty_WithPositionalPlaceholderBuilder_WithEmptyString()
    {
        // Arrange
        FormattableString formattableString = $"";
        var positionalPlaceholderBuilder = new ProperPositionalPlaceholderBuilder();

        // Act
        var result = ParameterHelper.Parametrize(formattableString, positionalPlaceholderBuilder);

        // Assert
        result.Should().BeEmpty();
        formattableString.ToString().Should().Be($"");
    }

    [TestMethod]
    public void Parametrize_ReturnsCorrectParameters_WithPositionalPlaceholderBuilder_WithNamedParameters()
    {
        // Arrange
        var id = 1;
        var name = "John";
        var date = DateTime.Now;
        FormattableString formattableString = $"User {name.AsParameter("name")} with ID {id.AsParameter("id")} was created on {date.AsParameter("date")}";
        var positionalPlaceholderBuilder = new ProperPositionalPlaceholderBuilder();

        // Act
        var result = ParameterHelper.Parametrize(formattableString, positionalPlaceholderBuilder);

        // Assert
        result.Should().HaveCount(3);
        result[0].Should().Be("John");
        result[1].Should().Be(1);
        result[2].Should().Be(date);
        formattableString.ToString().Should().Be($"User @p0 with ID @p1 was created on @p2");
    }

    [TestMethod]
    public void Parametrize_ReturnsCorrectParameters_WithPositionalPlaceholderBuilder_WithUnnamedParameters()
    {
        // Arrange
        var id = 1;
        var name = "John";
        FormattableString formattableString = $"User {name.AsParameter()} with ID {id.AsParameter()}";
        var positionalPlaceholderBuilder = new ProperPositionalPlaceholderBuilder();

        // Act
        var result = ParameterHelper.Parametrize(formattableString, positionalPlaceholderBuilder);

        // Assert
        result.Should().HaveCount(2);
        result[0].Should().Be("John");
        result[1].Should().Be(1);
        formattableString.ToString().Should().Be($"User @p0 with ID @p1");
    }

    [TestMethod]
    public void Parametrize_ReturnsCorrectParameters_WithPositionalPlaceholderBuilder_WithMixedParameters()
    {
        // Arrange
        var id = 1;
        var name = "John";
        var date = DateTime.Now;
        FormattableString formattableString = $"User {name.AsParameter("name")} with ID {id.AsParameter()} was created on {date.AsParameter("date")}";
        var positionalPlaceholderBuilder = new ProperPositionalPlaceholderBuilder();

        // Act
        var result = ParameterHelper.Parametrize(formattableString, positionalPlaceholderBuilder);

        // Assert
        result.Should().HaveCount(3);
        result[0].Should().Be("John");
        result[1].Should().Be(1);
        result[2].Should().Be(date);
        formattableString.ToString().Should().Be($"User @p0 with ID @p1 was created on @p2");
    }

    private class ProperNamedPlaceholderBuilder : INamedPlaceholderBuilder
    {
        public string Build(string name, int? nameDuplicateIndex)
        {
            return nameDuplicateIndex.HasValue ? $"@{name}{nameDuplicateIndex}" : $"@{name}";
        }

        public string Build(int unnamedParameterPlaceholderIndex)
        {
            return $"@p{unnamedParameterPlaceholderIndex}";
        }
    }

    private class ProperPositionalPlaceholderBuilder : IPositionalPlaceholderBuilder
    {
        public string Build(int index)
        {
            return $"@p{index}";
        }
    }
} 