namespace Basmus.Parametrization.Tests;

[TestClass]
public class ParameterCollectorTests
{
    [TestMethod]
    public void Collect_ReturnsEmptyList_WhenFormattableStringHasNoArguments()
    {
        // Arrange
        FormattableString fs = $"No parameters here.";
        var collector = new ParameterCollector();

        // Act
        var parameters = collector.Collect(fs);

        // Assert
        parameters.Should().BeEmpty();
    }

    [TestMethod]
    public void Collect_ReturnsCorrectParameters_WhenFormattableStringHasArguments()
    {
        // Arrange
        var p1 = 1;
        var p2 = DateTime.Now;

        FormattableString fs = $"ID: {p1.AsParameter("id")}, Date: {p2.AsParameter()} are the parameters.";
        var collector = new ParameterCollector();

        // Act
        var parameters = collector.Collect(fs).ToArray();

        // Assert
        parameters.Should().HaveCount(2);
        parameters[0].Name.Should().Be("id");
        parameters[0].Value.Should().Be(p1);
        parameters[1].HasName.Should().BeFalse();
        parameters[1].Value.Should().Be(p2);
    }

    [TestMethod]
    public void Collect_ReturnsCorrectParameters_WhenFormattableStringHasNonParameterArguments()
    {
        // Arrange
        var p1 = 1;
        var p2 = DateTime.Now;
        var nonParameterObject = "This is a test string";

        FormattableString fs = $"ID: {p1.AsParameter("id")}, NonParameter: {nonParameterObject}, Date: {p2.AsParameter()}, Null: {null}";
        var collector = new ParameterCollector();

        // Act
        var parameters = collector.Collect(fs).ToArray();

        // Assert
        parameters.Should().HaveCount(2);
        parameters[0].Name.Should().Be("id");
        parameters[0].Value.Should().Be(p1);
        parameters[1].HasName.Should().BeFalse();
        parameters[1].Value.Should().Be(p2);
    }

    [TestMethod]
    public void Collect_ReturnsCorrectParameters_WhenFormattableStringHasNestedFormattableStringArgument()
    {
        // Arrange
        var p1 = 1;
        var p2 = "test";
        var p3 = 2.0m;
        var p4 = DateTime.Now;

        FormattableString nested = $"Nested Name: {p2.AsParameter("name")}, Sum: {p3.AsParameter()}";
        FormattableString fs = $"Outer string with ID: {p1.AsParameter("id")}, {nested}, Date: {p4.AsParameter()}";
        var collector = new ParameterCollector();

        // Act
        var parameters = collector.Collect(fs).ToArray();

        // Assert
        parameters.Should().HaveCount(4);
        parameters[0].Name.Should().Be("id");
        parameters[0].Value.Should().Be(p1);
        parameters[1].Name.Should().Be("name");
        parameters[1].Value.Should().Be(p2);
        parameters[2].HasName.Should().BeFalse();
        parameters[2].Value.Should().Be(p3);
        parameters[3].HasName.Should().BeFalse();
        parameters[3].Value.Should().Be(p4);
    }

    [TestMethod]
    public void Collect_ReturnsCorrectParameters_WhenFormattableStringHasDuplicateParameterArguments()
    {
        // Arrange
        var p1 = 1;
        var p2 = "test";
        var p3 = 2.0m;
        var p4 = DateTime.Now;

        FormattableString nested = $"Nested Name: {p2.AsParameter("name")}, Sum: {p3.AsParameter()}";
        FormattableString fs = $"Outer ID: {p1.AsParameter("id")}, {nested}, Date: {p4.AsParameter()}, {nested}";
        var collector = new ParameterCollector();

        // Act
        var parameters = collector.Collect(fs).ToArray();

        // Assert
        parameters.Should().HaveCount(6);
        parameters[0].Name.Should().Be("id");
        parameters[0].Value.Should().Be(p1);
        parameters[1].Name.Should().Be("name");
        parameters[1].Value.Should().Be(p2);
        parameters[2].HasName.Should().BeFalse();
        parameters[2].Value.Should().Be(p3);
        parameters[3].HasName.Should().BeFalse();
        parameters[3].Value.Should().Be(p4);

        parameters[4].Should().Be(parameters[1]);
        parameters[5].Should().Be(parameters[2]);

    }
}
