namespace UniqloFromJapan.Models {
    public class ProductAllViewModel {
        public Product[]? Products { get; set; }
        public int[]? WishList { get; set; }

        public ProductAllViewModel() { }
        public ProductAllViewModel(Product[]? products, int[] wishlist) {
            Products = products;
            WishList = wishlist;
        }
    }
}