namespace Data.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public string ImageUrl { get; set; }
        public string PublicId { get; set; }
        public string Caption { get; set; }
        public int SortOrder { get; set; }
    }
}
