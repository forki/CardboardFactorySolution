using CardboardFactory.Core.Tools;

namespace CardboardFactory.Core {
    public class BlankSizes {
        public string BlankName { get; set; } = "Заготовка";
        public double? LengthOne { get; set; }
        public double? LengthTwo { get; set; }
        public double? Area => (LengthOne * LengthTwo).Round(3);
    }
}
