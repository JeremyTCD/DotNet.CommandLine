using JeremyTCD.DotNet.CommandLine.src;

namespace JeremyTCD.DotNet.CommandLine.Tests
{
    public class ModelbuilderIntegrationTests
    {

        [Verb(nameof(DummyStrings.DummyString), nameof(DummyStrings.DummyString), ResourceType = typeof(DummyStrings))]
        private class DummyModel
        {
            [Option(nameof(DummyStrings.DummyString), nameof(DummyStrings.DummyString), nameof(DummyStrings.DummyString), ResourceType = typeof(DummyStrings))]
            public string DummyProperty { get; set; }
        }
    }
}
