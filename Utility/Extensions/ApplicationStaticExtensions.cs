namespace Utility.Extensions
{
    public class ApplicationStaticExtensions
    {
        public const string blankProductImageUrl = "https://res.cloudinary.com/dglgzh0aj/image/upload/v1619054413/upload/img/product_new/blank/default_product_image_nhnhhs.jpg";
        public const string blankProductImagePublicId = "upload/img/product_new/blank/default_product_image_nhnhhs";

        public static string BlankProductImageCaption(string name)
        {
            return LowerStringHasUnderscore(name) + "_blank_image";
        }

        public static string ProductImageCaption(string name, int identity)
        {
            return LowerStringHasUnderscore(name) + "_image_" + identity;
        }

        public static string LowerString(string inputString)
        {
            return inputString.ToLower();
        }

        public static string LowerStringHasUnderscore(string inputString)
        {
            return inputString.ToLower().Replace(" ", "_");
        }

        public static string LowerStringHasDash(string inputString)
        {
            return inputString.ToLower().Replace(" ", "-");
        }

        public const string AdminRole = "Admin";
        public const string EmployeeRole = "Employee";
        public const string WarehouseRole = "Warehouse Staff";
        public const string CustomerOfficerRole = "Customer Officer";
        public const string CustomerRole = "Customer";
    }
}
