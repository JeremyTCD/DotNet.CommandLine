// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JeremyTCD.DotNet.CommandLine
{
    public class CommandSet : IDictionary<string, ICommand>
    {
        private readonly IDictionary<string, ICommand> _commands;
        private ICommand _defaultCommand;

        public virtual ICommand DefaultCommand
        {
            get
            {
                return _defaultCommand ?? (_defaultCommand = _commands.Values.Single(c => c.IsDefault));
            }
        }

        internal CommandSet() : this(null) { }

        /// <summary>
        /// Creates a <see cref="CommandSet"/> instance. 
        /// </summary>
        /// <param name="commands">Cannot contain more than 1 default command.</param>
        internal CommandSet(IDictionary<string, ICommand> commands)
        {
            _commands = commands == null ? new Dictionary<string, ICommand>() : new Dictionary<string, ICommand>(commands);
        }

        #region IDictionary<string, ICommand> member implementations
        public ICollection<string> Keys => _commands.Keys;

        public ICollection<ICommand> Values => _commands.Values;

        public int Count => _commands.Count;

        public bool IsReadOnly => _commands.IsReadOnly;

        public ICommand this[string key] { get => _commands[key]; set => _commands[key] = value; }

        public virtual void Add(string key, ICommand value)
        {
            _commands.Add(key, value);
        }

        public virtual bool ContainsKey(string key)
        {
            return _commands.ContainsKey(key);
        }

        public virtual bool Remove(string key)
        {
            return _commands.Remove(key);
        }

        public virtual bool TryGetValue(string key, out ICommand value)
        {
            return _commands.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, ICommand> item)
        {
            _commands.Add(item);
        }

        public virtual void Clear()
        {
            _commands.Clear();
        }

        public virtual bool Contains(KeyValuePair<string, ICommand> item)
        {
            return _commands.Contains(item);
        }

        public virtual void CopyTo(KeyValuePair<string, ICommand>[] array, int arrayIndex)
        {
            _commands.CopyTo(array, arrayIndex);
        }

        public virtual bool Remove(KeyValuePair<string, ICommand> item)
        {
            return _commands.Remove(item);
        }

        public virtual IEnumerator<KeyValuePair<string, ICommand>> GetEnumerator()
        {
            return _commands.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _commands.GetEnumerator();
        }
#endregion
    }
}
