namespace Basmus.Parametrization.Tests;

[TestClass]
public class ParameterTests
{
    [TestMethod]
    public void Constructor_WithNameAndValue_SetsPropertiesCorrectly()
    {
        // Arrange
        var name = "testName";
        var value = "testValue";

        // Act
        var parameter = new Parameter(name, value);

        // Assert
        parameter.Name.Should().Be(name);
        parameter.Value.Should().Be(value);
        parameter.HasName.Should().BeTrue();
        parameter.HasPlaceholder.Should().BeFalse();
        parameter.PlaceholderIsApplied.Should().BeFalse();
    }

    [TestMethod]
    public void Constructor_WithValueOnly_SetsPropertiesCorrectly()
    {
        // Arrange
        var value = "testValue";

        // Act
        var parameter = new Parameter(value);

        // Assert
        parameter.Value.Should().Be(value);
        parameter.HasName.Should().BeFalse();
        parameter.HasPlaceholder.Should().BeFalse();
        parameter.PlaceholderIsApplied.Should().BeFalse();
    }

    [TestMethod]
    public void Name_WhenNotDefined_ThrowsInvalidOperationException()
    {
        // Arrange
        var parameter = new Parameter("value");

        // Act
        var act = () => { var _ = parameter.Name; };

        // Assert
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Name is not defined");
    }

    [TestMethod]
    public void Placeholder_WhenNotSet_ThrowsInvalidOperationException()
    {
        // Arrange
        var parameter = new Parameter("value");

        // Act
        var act = () => { var _ = parameter.Placeholder; };

        // Assert
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Placeholder is not set");
    }

    [TestMethod]
    public void SetPlaceholder_SetsPropertiesCorrectly()
    {
        // Arrange
        var parameter = new Parameter("value");
        var placeholder = "placeholder";

        // Act
        parameter.SetPlaceholder(placeholder);

        // Assert
        parameter.Placeholder.Value.Should().Be(placeholder);
        parameter.HasPlaceholder.Should().BeTrue();
    }

    [TestMethod]
    public void SetPlaceholder_WhenAlreadySet_ThrowsInvalidOperationException()
    {
        // Arrange
        var parameter = new Parameter("value");
        parameter.SetPlaceholder("placeholder");

        // Act
        var act = () => parameter.SetPlaceholder("placeholder");

        // Assert
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Placeholder is already set");
    }

    [TestMethod]
    public void ApplyPlaceholder_UsesPlaceholderAsValue()
    {
        // Arrange
        var parameter = new Parameter("value");
        parameter.SetPlaceholder("placeholder");

        // Act
        parameter.ApplyPlaceholder();

        // Assert
        parameter.PlaceholderIsApplied.Should().BeTrue();
    }

    [TestMethod]
    public void ApplyPlaceholder_WhenPlaceholderNotSet_ThrowsInvalidOperationException()
    {
        // Arrange
        var parameter = new Parameter("value");

        // Act
        var act = () => parameter.ApplyPlaceholder();

        // Assert
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Placeholder is not set");
    }

    [TestMethod]
    public void ApplyPlaceholder_WhenAlreadyUsedAsValue_ThrowsInvalidOperationException()
    {
        // Arrange
        var parameter = new Parameter("value");
        parameter.SetPlaceholder("placeholder");
        parameter.ApplyPlaceholder();

        // Act
        var act = () => parameter.ApplyPlaceholder();

        // Assert
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Placeholder is already applied");
    }

    [TestMethod]
    public void ToString_WhenPlaceholderNotUsedAsValue_ReturnsValue()
    {
        // Arrange
        var value = "testValue";
        var parameter = new Parameter(value);

        // Act
        var result = parameter.ToString();

        // Assert
        result.Should().Be(value);
    }

    [TestMethod]
    public void ToString_WhenPlaceholderUsedAsValue_ReturnsPlaceholder()
    {
        // Arrange
        var placeholder = "placeholder";
        var parameter = new Parameter("value");
        parameter.SetPlaceholder(placeholder);
        parameter.ApplyPlaceholder();

        // Act
        var result = parameter.ToString();

        // Assert
        result.Should().Be(placeholder);
    }
}
