using System.Collections.Generic;

namespace CavesOfQuickMenu.Utilities
{
    public class BiDictionary<Type1, Type2>
    {
        private IDictionary<Type1, Type2> oneToTwo = new Dictionary<Type1, Type2>();
        private IDictionary<Type2, Type1> twoToOne = new Dictionary<Type2, Type1>();

        public IDictionary<Type1, Type2> Data
        {
            set
            {
                oneToTwo = new Dictionary<Type1, Type2>();
                twoToOne = new Dictionary<Type2, Type1>();
                foreach (KeyValuePair<Type1, Type2> entry in value)
                {
                    oneToTwo.Add(entry.Key, entry.Value);
                    twoToOne.Add(entry.Value, entry.Key);
                }
            }
        }

        public Type2 this[Type1 key]
        {
            get { return oneToTwo[key]; }
        }

        public Type1 this[Type2 key]
        {
            get { return twoToOne[key]; }
        }

        public Type2 GetWithDefault(Type1 key, Type2 defaultValue)
        {
            if (!oneToTwo.TryGetValue(key, out Type2 result))
            {
                result = defaultValue;
            }
            return result;
        }

        public Type1 GetWithDefault(Type2 key, Type1 defaultValue)
        {
            if (!twoToOne.TryGetValue(key, out Type1 result))
            {
                result = defaultValue;
            }
            return result;
        }
    }
}
