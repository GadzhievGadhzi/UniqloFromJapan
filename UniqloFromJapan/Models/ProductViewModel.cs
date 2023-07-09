namespace UniqloFromJapan.Models {
    public class ProductViewModel {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Price { get; set; }
        public byte[]? Image { get; set; }
        public List<EnumColorModel>? CheckBoxColorItems { get; set; }
        public List<EnumSizeModel>? CheckBoxSizeItems { get; set; }
    }
}
