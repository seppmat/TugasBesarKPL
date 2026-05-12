using System;

namespace TugasKu_TUBES_KPL.Core
{
    // ✅ TECHNIQUE: Parameterization / Generics
    public static class GenericValidator<T> where T : class
    {
        // Validasi menggunakan delegate/predicate
        public static bool IsValid(T item, Func<T, bool> condition)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (condition == null) throw new ArgumentNullException(nameof(condition));
            return condition(item);
        }

        // Guard clause generik untuk null check
        public static T EnsureNotNull(T item, string paramName)
        {
            if (item == null) throw new ArgumentNullException(paramName);
            return item;
        }
    }
}