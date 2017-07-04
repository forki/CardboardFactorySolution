using System;

namespace CardboardFactory.Core.Tools {
    public static class MathUtilities {
        public static bool AreEquals(double a, double b) {
            return Math.Abs(a - b) < double.Epsilon;
        }

        public static bool AreEquals(double? a, double? b) {
            if (a == null && b == null) { return true; }
            if (a == null || b == null) { return false; }
            return AreEquals(a.Value, b.Value);
        }

        public static double Round(this double value, int decimals) {
            return Math.Round(value, decimals);
        }

        public static double? Round(this double? value, int decimals) {
            return value?.Round(decimals);
        }
    }
}
