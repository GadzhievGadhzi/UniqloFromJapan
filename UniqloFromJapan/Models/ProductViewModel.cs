namespace UniqloFromJapan.Models {
    public class ProductViewModel {
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? BigDescription { get; set; }
        public string? Materials { get; set; }
        public string? Price { get; set; }
        public byte[]? Image { get; set; }
        public string? Gender { get; set; }
        public List<EnumColorModel>? CheckBoxColorItems { get; set; }
        public List<EnumSizeModel>? CheckBoxSizeItems { get; set; }
    }
}
