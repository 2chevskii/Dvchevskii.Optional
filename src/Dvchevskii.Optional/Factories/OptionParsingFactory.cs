namespace Dvchevskii.Optional.Factories
{
    public delegate bool OptionParsingFactory<TOutput>(string input, out TOutput output);
}
