using System.Collections;
using System.Collections.Generic;

namespace JeremyTCD.DotNet.CommandLine
{
    public class OptionCollection : IOptionCollection
    {
        private List<IOption> _options = new List<IOption>();

        public IOption this[int index] { get => ((IList<IOption>)_options)[index]; set => ((IList<IOption>)_options)[index] = value; }

        public virtual int Count => ((IList<IOption>)_options).Count;

        public virtual bool IsReadOnly => ((IList<IOption>)_options).IsReadOnly;

        public virtual void Add(IOption item)
        {
            ((IList<IOption>)_options).Add(item);
        }

        public virtual void Clear()
        {
            ((IList<IOption>)_options).Clear();
        }

        public virtual bool Contains(IOption item)
        {
            return ((IList<IOption>)_options).Contains(item);
        }

        public virtual void CopyTo(IOption[] array, int arrayIndex)
        {
            ((IList<IOption>)_options).CopyTo(array, arrayIndex);
        }

        public virtual IEnumerator<IOption> GetEnumerator()
        {
            return ((IList<IOption>)_options).GetEnumerator();
        }

        public virtual int IndexOf(IOption item)
        {
            return ((IList<IOption>)_options).IndexOf(item);
        }

        public virtual void Insert(int index, IOption item)
        {
            ((IList<IOption>)_options).Insert(index, item);
        }

        public virtual bool Remove(IOption item)
        {
            return ((IList<IOption>)_options).Remove(item);
        }

        public virtual void RemoveAt(int index)
        {
            ((IList<IOption>)_options).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<IOption>)_options).GetEnumerator();
        }
    }
}
