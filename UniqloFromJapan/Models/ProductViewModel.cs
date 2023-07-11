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

        public ProductViewModel() {
            CheckBoxColorItems = new List<EnumColorModel>();
            CheckBoxSizeItems = new List<EnumSizeModel>();

            var colors = Enum.GetNames(typeof(ProductColor)).ToList();
            colors.ForEach(x => {
                CheckBoxColorItems.Add(new EnumColorModel() { Color = (ProductColor)Enum.Parse(typeof(ProductColor), x), IsColorSelected = false });
            });

            var sizes = Enum.GetNames(typeof(ProductSize)).ToList();
            sizes.ForEach(x => {
                CheckBoxSizeItems.Add(new EnumSizeModel() { Size = (ProductSize)Enum.Parse(typeof(ProductSize), x), IsSizeSelected = false });
            });
        }
    }
}
