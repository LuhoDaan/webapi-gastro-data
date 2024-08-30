// SPDX-FileCopyrightText: NOI Techpark <digital@noi.bz.it>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

namespace GastroApi.Models
{
   public class Order
{
    public Guid Uuid { get; set; }
    public string ExternalOrderId { get; set; }
    public DateTime StartTime { get; set; }
    public int EstimatedTime { get; set; }
    public DateTime? CompletionTime { get; set; }
    public OrderStatus MappedStatus { get; set; }
    public OrderServingTypes OrderType { get; set; }
    public DateTime? PreOrderTime { get; set; }  // pick up time set in case the order was planed in advance by the customer
    public string OrderCategory { get; set; }  // Immediate or Preorder
    public bool Priority { get; set; }
    public int Cost { get; set; }
    // public DateTime? PackingTime { get; set; }
    public List<Dish> Dishes { get; set; }
    
    // public List<Menu> Menus { get; set; }
}
}

//BEFORE TRYNG THE BELOW CODE, SUBSTITUTE STRING WITH JSONRAW ON THE LAST FIELD (DATA)

//     // SPDX-FileCopyrightText: NOI Techpark <digital@noi.bz.it>
// //
// // SPDX-License-Identifier: AGPL-3.0-or-later
   

public enum OrderStatus
{
    Preparing,
    Ready,
    Served,
    Cancelled
}

public enum OrderServingTypes
{
    DineIn,
    Takeaway,
    LockerBox,
    Mixed  // Some dishes might be consumed directly inside the restaurant wile some others outside
}
