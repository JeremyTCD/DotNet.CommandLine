namespace JeremyTCD.DotNet.CommandLine
{
    public class OptionMetadataFactory : IOptionMetadataFactory
    {
        public OptionMetadata CreateFromAttribute(OptionAttribute optionAttribute)
        {
            return new OptionMetadata(optionAttribute.ShortName, optionAttribute.LongName, optionAttribute.Description);
        }
    }
}
