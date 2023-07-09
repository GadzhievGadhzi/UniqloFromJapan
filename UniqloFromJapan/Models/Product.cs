using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using UniqloFromJapan.Models;

namespace UniqloFromJapan.Models {
    public class Product {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Price { get; set; }
        public string? Gender { get; set; }
        public ProductColor[] Colors { get; set; }
        public ProductSize[] Size { get; set; }
        public int Rating { get; set; }
        public string[] Images { get; set; }

        public Product() {
            Images = new string[] { };
        }
        public Product(int id, string title,  string description, string price, ProductColor[] colors, ProductSize[] size, int rating, string[] images, string? gender) {
            Id = id;
            Title = title;
            Description = description;
            Price = price;
            Colors = colors;
            Size = size;
            Rating = rating;
            Images = images;
            Gender = gender;
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