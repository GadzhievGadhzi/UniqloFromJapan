using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using UniqloFromJapan.Models;

namespace UniqloFromJapan.Models {
    public class Product {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? BigDescription { get; set; }
        public string? Materials { get; set; }
        public string? Price { get; set; }
        public string? Gender { get; set; }
        public ProductColor[] Colors { get; set; }
        public ProductSize[] Size { get; set; }
        public int Rating { get; set; }
        public string[] Images { get; set; }

        public Product() {
            Images = new string[] { };
        }
        public Product(int id, string title,  string shortDescription, string price, ProductColor[] colors, ProductSize[] size, int rating, string[] images, string? gender, string? bigDescription) {
            Id = id;
            Title = title;
            Price = price;
            Colors = colors;
            Size = size;
            Rating = rating;
            Images = images;
            Gender = gender;
            ShortDescription = shortDescription;
            BigDescription = bigDescription;
        }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProductColor {
        [Description("A+")]
        [Display(Name = "White")]
        White,
        Black,
        Red,
        Green,
        Yellow,
        Purple,
        Beige,
        Orange,
        Brown,
        Blue,
        Pink
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProductSize {
        XXS,
        XS,
        S,
        M,
        L,
        XL,
        XXL,
        UNISEX
    }
}

public static class ProductExtencions {
    public static string InHtmlColorCode(this ProductColor color) {
        return color switch {
            ProductColor.White => "#ffffff",
            ProductColor.Black => "#3d3d3d",
            ProductColor.Red => "#dd3535",
            ProductColor.Brown => "#704e38",
            ProductColor.Green => "#5e6352",
            ProductColor.Yellow => "#d6c16c",
            ProductColor.Purple => "#782e59",
            ProductColor.Beige => "#c3b7aa",
            ProductColor.Orange => "#f0a83e",
            ProductColor.Blue => "#1800f3",
            ProductColor.Pink => "#f3c0c9",
            _ => ""
        };
    }
}