namespace Basmus.Parametrization.Tests
{
    [TestClass]
    public class ParameterExtensionsTests
    {
        [TestMethod]
        public void AsParameter_HasCorrectName_IfNameIsPassed()
        {
            // Arrange
            var value = "test";
            var name = "testName";

            // Act
            var result = value.AsParameter(name);

            // Assert
            result.HasName.Should().BeTrue();
            result.Name.Should().Be(name);
            result.Value.Should().Be(value);
        }

        [TestMethod]
        public void AsParameter_HasNoName_IfNameIsNotPassed()
        {
            // Arrange
            var value = "test";

            // Act
            var result = value.AsParameter();

            // Assert
            result.HasName.Should().BeFalse();
            result.Value.Should().Be(value);
        }
    }
}
