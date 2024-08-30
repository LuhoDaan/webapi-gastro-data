namespace GastroApi.Models{



public class MenuItem
{
    public Guid Uuid { get; set; }
    public string ResourceAlias { get; set; } // Display name or title for the menu item, it can be different from the actual name of the Item Resource
    public ResourceTypes ResourceType { get; set; } // Product, SubProduct, Ingredient
    
    // public Guid MenuId { get; set; }
    public int CurrentCost { get; set; }
    
    //public List<Package> Packages { get; set; }
    //public List<Variant> Variants { get; set; }
    //public List<MenuItemAvailability> MenuItemAvailabilities { get; set; }
    //public List<AddOn> AddOns { get; set; }
    public ItemResource Resource { get; set; }
}


}