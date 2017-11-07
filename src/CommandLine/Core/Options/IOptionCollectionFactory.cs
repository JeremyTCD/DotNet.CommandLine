namespace JeremyTCD.DotNet.CommandLine
{
    public interface IOptionCollectionFactory
    {

        /// <summary>
        /// Creates or retrieves an <see cref="IOptionCollection"/> for the <see cref="ICommand"/>. If the <see cref="ICommand"/> has an existing
        /// <see cref="IOptionCollection"/> from a previous call to this method, returns it. Otherwise, creates a new <see cref="IOptionCollection"/> from 
        /// the <see cref="ICommand"/>'s properties; the new <see cref="IOptionCollection"/> will contain an <see cref="IOption"/> for each property that 
        /// has an <see cref="OptionAttribute"/>.
        /// </summary>
        /// <param name="command">The <see cref="ICommand"/> to create an <see cref="IOptionCollection"/> from.</param>
        /// <returns>The new <see cref="IOptionCollection"/>.</returns>
        IOptionCollection Create(ICommand command);
    }
}
