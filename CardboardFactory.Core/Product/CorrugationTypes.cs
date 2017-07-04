using System;

namespace CardboardFactory.Core.Product {
    [Flags]
    public enum CorrugationTypes {
        E = 1,
        C = E << 1,
        EAndC = C << 1,
        All = C | E | EAndC,
        Polygraphy = All << 1
    }
}
