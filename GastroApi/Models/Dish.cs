

namespace GastroApi.Models{

public class Dish
{
    public long Uuid { get; set; }
    public DishStatus MappedStatus { get; set; } // current state of the dish: preparing, ready, served, cancelled
    public bool Packed { get; set; }
    public int TotalResources { get; set; }
    public int Cost { get; set; }
    public long MenuItemUuid { get; set; }
    public DateTime ProductionStartTime { get; set; }
    public DateTime ProductionEndTime { get; set; }
    public string DishName { get; set; }
    public bool IsPickedUp { get; set; }
    public ServingTypes ServingType { get; set; } // How the dish is served or delivered: LockerBox, Takeaway, Dine-In
    public long OrderUuid { get; set; }
    
    // public Package Package { get; set; }
    
    //public List<ContainedResource> ContainedResources { get; set; }
    
    //public string StateMessage { get; set; }
}

public class DishDto
{
    public long Uuid { get; set; }
    public string MappedStatus { get; set; }
    public bool Packed { get; set; }
    public int TotalResources { get; set; }
    public int Cost { get; set; }
    public long MenuItemUuid { get; set; }
    public DateTime ProductionStartTime { get; set; }
    public DateTime ProductionEndTime { get; set; }
    public string DishName { get; set; }
    public bool IsPickedUp { get; set; }
    public string ServingType { get; set; }
    public long OrderUuid { get; set; }
    
}




public enum ServingTypes
{
    DineIn,
    Takeaway,
    LockerBox
}



public enum DishStatus
{
    Preparing,
    Ready,
    Served,
    Cancelled
}



}
