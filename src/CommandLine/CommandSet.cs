using System.Collections;
using System.Collections.Generic;

namespace JeremyTCD.DotNet.CommandLine
{
    public class CommandSet : IDictionary<string, ICommand>
    {
        public IDictionary<string, ICommand> Commands { get; }

        /// <summary>
        /// Creates a <see cref="CommandSet"/> instance.
        /// </summary>
        internal CommandSet()
        {
            Commands = new Dictionary<string, ICommand>();
        }

        /// <summary>
        /// Creates a <see cref="CommandSet"/> instance. 
        /// </summary>
        /// <param name="commands">Cannot contain more than 1 default command.</param>
        internal CommandSet(IDictionary<string, ICommand> commands)
        {
            Commands = commands;
        }

        #region IDictionary<string, Command> member implementations
        public ICollection<string> Keys => Commands.Keys;

        public ICollection<ICommand> Values => Commands.Values;

        public int Count => Commands.Count;

        public bool IsReadOnly => Commands.IsReadOnly;

        public ICommand this[string key] { get => Commands[key]; set => Commands[key] = value; }

        public void Add(string key, ICommand value)
        {
            Commands.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return Commands.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return Commands.Remove(key);
        }

        public bool TryGetValue(string key, out ICommand value)
        {
            return Commands.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, ICommand> item)
        {
            Commands.Add(item);
        }

        public void Clear()
        {
            Commands.Clear();
        }

        public bool Contains(KeyValuePair<string, ICommand> item)
        {
            return Commands.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, ICommand>[] array, int arrayIndex)
        {
            Commands.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, ICommand> item)
        {
            return Commands.Remove(item);
        }

        public IEnumerator<KeyValuePair<string, ICommand>> GetEnumerator()
        {
            return Commands.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Commands.GetEnumerator();
        }
        #endregion
    }
}
