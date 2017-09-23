// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JeremyTCD.DotNet.CommandLine
{
    /// <inheritdoc/>
    public class CommandDictionary : ICommandDictionary
    {
        private readonly IDictionary<string, ICommand> _commands;
        private ICommand _defaultCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandDictionary"/> class.
        /// </summary>
        internal CommandDictionary()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandDictionary"/> class.
        /// </summary>
        /// <param name="commands">The dictionary whose elements are used to populate the new <see cref="CommandDictionary"/>.</param>
        internal CommandDictionary(IDictionary<string, ICommand> commands)
        {
            _commands = commands == null ? new Dictionary<string, ICommand>() : new Dictionary<string, ICommand>(commands);
        }

        /// <inheritdoc/>
        public ICommand DefaultCommand
        {
            get
            {
                return _defaultCommand ?? (_defaultCommand = _commands.Values.Single(c => c.IsDefault));
            }
        }

        #region IDictionary<string, ICommand> member implementations

        /// <inheritdoc/>
        public ICollection<string> Keys => _commands.Keys;

        /// <inheritdoc/>
        public ICollection<ICommand> Values => _commands.Values;

        /// <inheritdoc/>
        public int Count => _commands.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => _commands.IsReadOnly;

        /// <inheritdoc/>
        public ICommand this[string key] { get => _commands[key]; set => _commands[key] = value; }

        /// <inheritdoc/>
        public void Add(string key, ICommand value)
        {
            _commands.Add(key, value);
        }

        /// <inheritdoc/>
        public bool ContainsKey(string key)
        {
            return _commands.ContainsKey(key);
        }

        /// <inheritdoc/>
        public bool Remove(string key)
        {
            return _commands.Remove(key);
        }

        /// <inheritdoc/>
        public bool TryGetValue(string key, out ICommand value)
        {
            return _commands.TryGetValue(key, out value);
        }

        /// <inheritdoc/>
        public void Add(KeyValuePair<string, ICommand> item)
        {
            _commands.Add(item);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            _commands.Clear();
        }

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<string, ICommand> item)
        {
            return _commands.Contains(item);
        }

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<string, ICommand>[] array, int arrayIndex)
        {
            _commands.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public bool Remove(KeyValuePair<string, ICommand> item)
        {
            return _commands.Remove(item);
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<string, ICommand>> GetEnumerator()
        {
            return _commands.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _commands.GetEnumerator();
        }
        #endregion
    }
}
