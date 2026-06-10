using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TugasKu_TUBES_KPL
{
    // ============================================================
    // DESIGN PATTERN : Factory Method (StateFactory internal)
    // TEKNIK KPL     : Table-Driven + Automata (DAFFA & EDWARD)
    // SECURE CODING  : (EDWARD & DAFFA)
    //   [SC-5] Input length limit — Name dan Course dibatasi panjangnya
    //   [SC-6] Input sanitization — karakter kontrol di-strip dari Name/Course
    //   [SC-7] Deadline upper-bound — tidak boleh > 10 tahun ke depan
    // ============================================================

    public class TaskItem
    {
        // [SC-5] Batas panjang input yang wajar
        public const int MaxNameLength   = 200;
        public const int MaxCourseLength = 100;

        // [SC-7] Batas atas deadline yang masuk akal
        private static readonly int MaxDeadlineYearsAhead = 10;

        // ---- Backing fields dengan sanitasi ----

        private string _name   = string.Empty;
        private string _course = string.Empty;

        // [SC-5][SC-6] Setter Name: strip karakter kontrol, batasi panjang
        public string Name
        {
            get => _name;
            set => _name = SanitizeInput(value, MaxNameLength);
        }

        // [SC-5][SC-6] Setter Course: strip karakter kontrol, batasi panjang
        public string Course
        {
            get => _course;
            set => _course = SanitizeInput(value, MaxCourseLength);
        }

        public DateTime     Deadline     { get; set; }
        public TaskPriority Priority     { get; set; }
        public TaskStatus   Status       { get; private set; }
        public TaskState    CurrentState { get; private set; }

        // ---- Table-Driven: Aturan Transisi ----

        public static readonly Dictionary<TaskStatus, TaskStatus[]> TransitionTable =
            new Dictionary<TaskStatus, TaskStatus[]>
            {
                { TaskStatus.NotStarted, new[] { TaskStatus.InProgress  } },
                { TaskStatus.InProgress, new[] { TaskStatus.Done        } },
                { TaskStatus.Done,       new[] { TaskStatus.NotStarted  } },
            };

        // ---- Table-Driven: Aturan Validasi ----

        // [SC-7] ValidationRules kini juga mencakup batas atas deadline
        public static readonly Dictionary<string, Func<TaskItem, bool>> ValidationRules =
            new Dictionary<string, Func<TaskItem, bool>>
            {
                { "NamaWajib",         t => !string.IsNullOrWhiteSpace(t.Name)   },
                { "CourseWajib",       t => !string.IsNullOrWhiteSpace(t.Course) },
                { "DeadlineTidakLalu", t => t.Deadline.Date >= DateTime.Today    },
                { "DeadlineTidakTerlalu",
                    t => t.Deadline.Date <= DateTime.Today.AddYears(MaxDeadlineYearsAhead) },
            };

        // ---- Factory Method: Pembuatan State ----

        private static readonly Dictionary<TaskStatus, Func<TaskState>> StateFactory =
            new Dictionary<TaskStatus, Func<TaskState>>
            {
                { TaskStatus.NotStarted, () => new NotStartedState() },
                { TaskStatus.InProgress, () => new InProgressState() },
                { TaskStatus.Done,       () => new DoneState()        },
            };

        // ---- Constructor ----

        public TaskItem()
        {
            Status       = TaskStatus.NotStarted;
            CurrentState = new NotStartedState();
        }

        // ---- Methods ----

        public bool TryTransitionTo(TaskStatus target)
        {
            bool isAllowed = TransitionTable.TryGetValue(Status, out TaskStatus[] allowed)
                             && Array.IndexOf(allowed, target) >= 0;

            if (!isAllowed) return false;

            SetStatus(target);
            return true;
        }

        public void SetStatus(TaskStatus newStatus)
        {
            if (!StateFactory.TryGetValue(newStatus, out Func<TaskState> createState))
                throw new InvalidOperationException(
                    $"Status '{newStatus}' tidak didukung oleh state factory.");

            Status       = newStatus;
            CurrentState = createState();
        }

        public bool ValidateAll()
            => ValidationRules.Values.All(rule => rule(this));

        // ---- Private: Input Sanitization ----

        /// <summary>
        /// [SC-6] Menghapus karakter kontrol (ASCII 0-31, kecuali tab) dari input
        /// dan membatasi panjang string sesuai maxLength.
        /// Tidak melempar exception — mengembalikan string yang sudah dibersihkan.
        /// </summary>
        private static string SanitizeInput(string input, int maxLength)
        {
            if (input == null) return string.Empty;

            // Hapus karakter kontrol (newline tersembunyi, null byte, dll.)
            string cleaned = Regex.Replace(input, @"[\x00-\x08\x0B\x0C\x0E-\x1F]", string.Empty);

            // Batasi panjang
            return cleaned.Length > maxLength
                ? cleaned.Substring(0, maxLength)
                : cleaned;
        }
    }

    public enum TaskStatus   { NotStarted, InProgress, Done   }
    public enum TaskPriority { High, Medium, Low }
}
