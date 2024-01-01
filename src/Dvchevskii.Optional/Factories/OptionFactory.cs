namespace Dvchevskii.Optional.Factories
{
    public delegate bool OptionFactory<in TInput, TOutput>(TInput argument, out TOutput output);
}
