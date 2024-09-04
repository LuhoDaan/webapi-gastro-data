namespace GastroApi.Models{


public class ItemResource
{
    // Properties of the actual resource (Product, Subproduct, etc.)
    public Guid Uuid { get; set; }
    public string Name { get; set; }
    public ResourceTypes Type { get; set; }
    
    //public string ImagePath { get; set; }
    
    //public List<Diet> Diets { get; set; }
    
    // ... other properties
}

public enum ResourceTypes
{
    Product,
    SubProduct,
    Ingredient
}

}