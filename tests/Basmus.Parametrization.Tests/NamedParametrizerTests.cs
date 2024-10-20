namespace Basmus.Parametrization.Tests;

[TestClass]
public class NamedParametrizerTests
{
    [TestMethod]
    public void Apply_WithOutParameters_ShouldNotThrow()
    {
        // Arrange
        var parameterKeyValueBuilder = new ProperNamedPlaceholderBuilder();
        var parametrizer = new NamedParametrizer(parameterKeyValueBuilder);

        // Act
        var act = () => parametrizer.PreparePlaceholders(Array.Empty<Parameter>());

        // Assert
        act.Should().NotThrow();
    }

    [TestMethod]
    public void Apply_WithNamedAndUnnamedParameters_KeysShouldBeCorrect()
    {
        // Arrange
        var parameterKeyValueBuilder = new ProperNamedPlaceholderBuilder();
        var parametrizer = new NamedParametrizer(parameterKeyValueBuilder);
        var namedParameter1 = new Parameter("id", 1);
        var unnamedParameter1 = new Parameter(2.3);
        var namedParameter2 = new Parameter("date", DateTime.Now);
        var unnamedParameter2 = new Parameter("string value");

        // Act
        parametrizer.PreparePlaceholders([namedParameter1, unnamedParameter1, namedParameter2, unnamedParameter2]);

        // Assert
        namedParameter1.HasPlaceholder.Should().BeTrue();
        namedParameter1.Placeholder.Value.Should().Be("@id");
        unnamedParameter1.HasPlaceholder.Should().BeTrue();
        unnamedParameter1.Placeholder.Value.Should().Be("@p0");
        namedParameter2.HasPlaceholder.Should().BeTrue();
        namedParameter2.Placeholder.Value.Should().Be("@date");
        unnamedParameter2.HasPlaceholder.Should().BeTrue();
        unnamedParameter2.Placeholder.Value.Should().Be("@p1");
    }

    [TestMethod]
    public void Apply_WithDuplicateParameters_KeysShouldBeCorrect()
    {
        // Arrange
        var parameterKeyValueBuilder = new ProperNamedPlaceholderBuilder();
        var parametrizer = new NamedParametrizer(parameterKeyValueBuilder);
        var namedParameter1 = new Parameter("id", 1);
        var unnamedParameter1 = new Parameter(2.3);

        // Act
        parametrizer.PreparePlaceholders([namedParameter1, namedParameter1, unnamedParameter1, unnamedParameter1]);

        // Assert
        namedParameter1.HasPlaceholder.Should().BeTrue();
        namedParameter1.Placeholder.Value.Should().Be("@id");
        unnamedParameter1.HasPlaceholder.Should().BeTrue();
        unnamedParameter1.Placeholder.Value.Should().Be("@p0");
    }

    [TestMethod]
    public void Apply_WithNamedParametersHavingConflictNames_ConflictShouldBeResolved()
    {
        // Arrange
        var parameterKeyValueBuilder = new ProperNamedPlaceholderBuilder();
        var parametrizer = new NamedParametrizer(parameterKeyValueBuilder);
        var namedParameter1 = new Parameter("id", 1);
        var conflictedParameter = new Parameter("id", 2);

        // Act
        parametrizer.PreparePlaceholders([namedParameter1, conflictedParameter]);

        // Assert
        namedParameter1.HasPlaceholder.Should().BeTrue();
        namedParameter1.Placeholder.Value.Should().Be("@id0");
        conflictedParameter.HasPlaceholder.Should().BeTrue();
        conflictedParameter.Placeholder.Value.Should().Be("@id1");
    }

    [TestMethod]
    public void Apply_WithNamedAndUnnamedParametersHavingConflictNames_KeysShouldBeAdjusted()
    {
        // Arrange
        var parameterKeyValueBuilder = new ProperNamedPlaceholderBuilder();
        var parametrizer = new NamedParametrizer(parameterKeyValueBuilder);
        var unnamedParameter1 = new Parameter(2.3);
        var conflictedNamedParameter1 = new Parameter("p0", 2);
        var unnamedParameter2 = new Parameter("string value");
        var conflictedNamedParameter2 = new Parameter("p2", 2);

        // Act
        parametrizer.PreparePlaceholders([unnamedParameter1, conflictedNamedParameter1, unnamedParameter2, conflictedNamedParameter2]);

        // Assert
        unnamedParameter1.HasPlaceholder.Should().BeTrue();
        unnamedParameter1.Placeholder.Value.Should().Be("@p1");
        conflictedNamedParameter1.HasPlaceholder.Should().BeTrue();
        conflictedNamedParameter1.Placeholder.Value.Should().Be("@p0");
        unnamedParameter2.HasPlaceholder.Should().BeTrue();
        unnamedParameter2.Placeholder.Value.Should().Be("@p3");
        conflictedNamedParameter2.HasPlaceholder.Should().BeTrue();
        conflictedNamedParameter2.Placeholder.Value.Should().Be("@p2");
    }

    [TestMethod]
    public void Apply_WhenKeyValueBuilderIsImproper_ShouldThrowExceptionForConflictedNamedKeys()
    {
        // Arrange
        var parameterKeyValueBuilder = new ImproperNamedPlaceholderBuilder();
        var parametrizer = new NamedParametrizer(parameterKeyValueBuilder);
        var sameNameParameter1 = new Parameter("id", 2);
        var sameNameParameter2 = new Parameter("id", DateTime.Now);

        // Act
        var act = () => parametrizer.PreparePlaceholders([sameNameParameter1, sameNameParameter2]);

        // Assert
        act
            .Should()
            .Throw<ImproperPlaceholderBuilderException>()
            .WithMessage("Named placeholder '@id' is already in use. Name: 'id', Index: '1'");
    }

    [TestMethod]
    public void Apply_WhenKeyValueBuilderIsImproper_ShouldThrowExceptionForUnresolvedNameConflicts()
    {
        // Arrange
        var parameterKeyValueBuilder = new ImproperNamedPlaceholderBuilder();
        var parametrizer = new NamedParametrizer(parameterKeyValueBuilder);
        var conflictedNamedParameter = new Parameter("p", 2);
        var unnamedParameter = new Parameter(2.3);

        // Act
        var act = () => parametrizer.PreparePlaceholders([conflictedNamedParameter, unnamedParameter]);

        // Assert
        act
            .Should()
            .Throw<ImproperPlaceholderBuilderException>()
            .WithMessage("Placeholder conflict was not resolved in the expected number of attempts. The last unsuccessful placeholder is '@p'");
    }

    [TestMethod]
    public void Apply_WhenKeyValueBuilderIsImproper_ShouldThrowExceptionForConflictedUnnamedKeys()
    {
        // Arrange
        var parameterKeyValueBuilder = new ImproperNamedPlaceholderBuilder();
        var parametrizer = new NamedParametrizer(parameterKeyValueBuilder);
        var unnamedParameter1 = new Parameter(2.3);
        var unnamedParameter2 = new Parameter("string value");

        // Act
        var act = () => parametrizer.PreparePlaceholders([unnamedParameter1, unnamedParameter2]);

        // Assert
        act
            .Should()
            .Throw<ImproperPlaceholderBuilderException>()
            .WithMessage("Unnamed placeholder '@p' is already in use. Index: '1'");
    }

    public class ProperNamedPlaceholderBuilder : INamedPlaceholderBuilder
    {
        public string Build(string name, int? nameDuplicateIndex)
        {
            var key = nameDuplicateIndex.HasValue
                ? $"@{name}{nameDuplicateIndex}"
                : $"@{name}";

            return key;
        }

        public string Build(int unnamedParameterPlaceholderIndex)
        {
            return $"@p{unnamedParameterPlaceholderIndex}";
        }
    }

    public class ImproperNamedPlaceholderBuilder : INamedPlaceholderBuilder
    {
        public string Build(string name, int? nameDuplicateIndex)
        {
            return $"@{name}";
        }

        public string Build(int unnamedParameterPlaceholderIndex)
        {
            return "@p";
        }
    }
}
