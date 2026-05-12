using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace TugasKu_TUBES_KPL.Core
{
    // ✅ TECHNIQUE: Parameterization / Generics
    public class GenericRepository<T> where T : class
    {
        private List<T> _items = new List<T>();

        public void Add(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _items.Add(item);
        }

        public void Update(int index, T item)
        {
            if (index < 0 || index >= _items.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (item == null) throw new ArgumentNullException(nameof(item));
            _items[index] = item;
        }

        public void Delete(int index)
        {
            if (index < 0 || index >= _items.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            _items.RemoveAt(index);
        }

        public List<T> GetAll()
        {
            // Return copy agar data internal tidak diubah langsung dari luar
            return new List<T>(_items);
        }

        public void Load(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Path tidak valid.");
            if (!File.Exists(filePath)) return;

            try
            {
                string json = File.ReadAllText(filePath);
                _items = JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
            }
            catch
            {
                // Defensive: Reset ke list kosong jika file korup
                _items = new List<T>();
            }
        }

        public void Save(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Path tidak valid.");
            string json = JsonConvert.SerializeObject(_items, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}