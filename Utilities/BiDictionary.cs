using System.Collections.Generic;

namespace CavesOfQuickMenu.Utilities
{
    public class BiDictionary<Type1, Type2>
    {
        private IDictionary<Type1, Type2> keyToValue = new Dictionary<Type1, Type2>();
        private IDictionary<Type2, Type1> valueToKey = new Dictionary<Type2, Type1>();

        public IDictionary<Type1, Type2> Data
        {
            set
            {
                keyToValue = new Dictionary<Type1, Type2>();
                valueToKey = new Dictionary<Type2, Type1>();
                foreach (KeyValuePair<Type1, Type2> entry in value)
                {
                    keyToValue.Add(entry.Key, entry.Value);
                    valueToKey.Add(entry.Value, entry.Key);
                }
            }
        }

        public int Count => keyToValue.Count;

        public Type2 this[Type1 key]
        {
            get { return keyToValue[key]; }
        }

        public Type1 this[Type2 value]
        {
            get { return valueToKey[value]; }
        }

        public Type2 GetByKey(Type1 key)
        {
            return keyToValue[key];
        }

        public Type1 GetByValue(Type2 value)
        {
            return valueToKey[value];
        }

        public Type2 GetWithDefault(Type1 key, Type2 defaultValue)
        {
            if (!keyToValue.TryGetValue(key, out Type2 result))
            {
                result = defaultValue;
            }
            return result;
        }

        public Type1 GetWithDefault(Type2 value, Type1 defaultKey)
        {
            if (!valueToKey.TryGetValue(value, out Type1 result))
            {
                result = defaultKey;
            }
            return result;
        }

        public Type2 GetByKeyWithDefault(Type1 key, Type2 defaultValue)
        {
            if (!keyToValue.TryGetValue(key, out Type2 result))
            {
                result = defaultValue;
            }
            return result;
        }

        public Type1 GetByValueWithDefault(Type2 value, Type1 defaultKey)
        {
            if (!valueToKey.TryGetValue(value, out Type1 result))
            {
                result = defaultKey;
            }
            return result;
        }

        public bool ContainsKey(Type1 key)
        {
            return keyToValue.ContainsKey(key);
        }

        public bool ContainsValue(Type2 value)
        {
            return valueToKey.ContainsKey(value);
        }
    }
}
