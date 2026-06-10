using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace TugasKu_TUBES_KPL.Core
{
    // ============================================================
    // DESIGN PATTERN : Repository Pattern
    // TEKNIK KPL     : Parameterization / Generics (EDWARD)
    // SECURE CODING  : (EDWARD)
    //   [SC-1] Path traversal prevention — ResolveAndValidatePath()
    //   [SC-2] Defensive copy pada GetAll() mencegah external mutation
    //   [SC-3] TypeNameHandling.None mencegah JSON deserialization attack
    //   [SC-4] Batasan ukuran file (MaxFileSizeBytes) mencegah DoS
    // ============================================================

    public class GenericRepository<T> where T : class
    {
        // [SC-4] Batas maksimum ukuran file JSON yang boleh dimuat (5 MB)
        private const long MaxFileSizeBytes = 5 * 1024 * 1024;

        // [SC-1] Direktori kerja yang diizinkan — hanya folder aplikasi
        private static readonly string AllowedDirectory =
            AppDomain.CurrentDomain.BaseDirectory;

        // [SC-3] Setting JSON yang aman: TypeNameHandling.None mencegah
        // arbitrary type instantiation via JSON payload berbahaya
        private static readonly JsonSerializerSettings SafeJsonSettings =
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None,
                MaxDepth         = 32
            };

        private List<T> _items = new List<T>();

        // ---- Queries ----

        /// <summary>
        /// Mengembalikan salinan defensif dari seluruh data.
        /// [SC-2] Mencegah modifikasi langsung terhadap koleksi internal.
        /// </summary>
        public List<T> GetAll() => new List<T>(_items);

        /// <summary>Jumlah item yang tersimpan saat ini.</summary>
        public int Count => _items.Count;

        // ---- Commands ----

        /// <exception cref="ArgumentNullException">Jika item null.</exception>
        public void Add(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _items.Add(item);
        }

        /// <exception cref="ArgumentOutOfRangeException">Jika indeks di luar batas.</exception>
        /// <exception cref="ArgumentNullException">Jika item baru null.</exception>
        public void Update(int index, T item)
        {
            ValidateIndex(index, nameof(index));
            if (item == null) throw new ArgumentNullException(nameof(item));
            _items[index] = item;
        }

        /// <exception cref="ArgumentOutOfRangeException">Jika indeks di luar batas.</exception>
        public void Delete(int index)
        {
            ValidateIndex(index, nameof(index));
            _items.RemoveAt(index);
        }

        // ---- Persistensi ----

        /// <summary>
        /// Memuat data dari file JSON.
        /// [SC-1] Path divalidasi agar tidak keluar dari direktori aplikasi.
        /// [SC-3] Deserialisasi menggunakan setting aman (tanpa TypeNameHandling).
        /// [SC-4] File di atas batas ukuran ditolak.
        /// </summary>
        /// <exception cref="ArgumentException">Jika path tidak valid atau di luar direktori izin.</exception>
        public void Load(string filePath)
        {
            string safePath = ResolveAndValidatePath(filePath);

            if (!File.Exists(safePath)) return;

            // [SC-4] Tolak file yang terlalu besar sebelum dibaca ke memori
            var fileInfo = new FileInfo(safePath);
            if (fileInfo.Length > MaxFileSizeBytes)
                throw new InvalidOperationException(
                    $"File melebihi batas ukuran maksimum ({MaxFileSizeBytes / 1024 / 1024} MB).");

            try
            {
                string json = File.ReadAllText(safePath);
                // [SC-3] Gunakan SafeJsonSettings, bukan default settings
                _items = JsonConvert.DeserializeObject<List<T>>(json, SafeJsonSettings)
                         ?? new List<T>();
            }
            catch (JsonException)
            {
                // Defensive: reset ke kosong jika JSON korup / tidak terbaca
                _items = new List<T>();
            }
        }

        /// <summary>
        /// Menyimpan seluruh data ke file JSON.
        /// [SC-1] Path divalidasi agar tidak keluar dari direktori aplikasi.
        /// [SC-3] Serialisasi menggunakan setting aman.
        /// </summary>
        /// <exception cref="ArgumentException">Jika path tidak valid atau di luar direktori izin.</exception>
        public void Save(string filePath)
        {
            string safePath = ResolveAndValidatePath(filePath);
            string json     = JsonConvert.SerializeObject(_items, Formatting.Indented, SafeJsonSettings);
            File.WriteAllText(safePath, json);
        }

        // ---- Private Helpers ----

        private void ValidateIndex(int index, string paramName)
        {
            if (index < 0 || index >= _items.Count)
                throw new ArgumentOutOfRangeException(
                    paramName,
                    $"Indeks {index} di luar batas. Ukuran koleksi: {_items.Count}.");
        }

        /// <summary>
        /// [SC-1] Resolves path ke absolut dan memastikan ia berada di dalam
        /// AllowedDirectory. Mencegah path traversal (e.g. "../../etc/passwd").
        /// </summary>
        private static string ResolveAndValidatePath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Path file tidak boleh kosong.", nameof(filePath));

            string fullPath = Path.GetFullPath(
                Path.Combine(AllowedDirectory, filePath));

            if (!fullPath.StartsWith(AllowedDirectory, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException(
                    "Path file tidak diizinkan berada di luar direktori aplikasi.",
                    nameof(filePath));

            return fullPath;
        }
    }
}
