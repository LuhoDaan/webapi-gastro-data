using System;
using System.Linq;
using GastroApi.Models;

namespace GastroApi.Services{
public static class OrderMapper
{
    public static OrderDtoDb MapOrderDtoApiToOrderDtoDb(OrderDtoApi apiDto)
    {
        if (apiDto == null)
            throw new ArgumentNullException(nameof(apiDto));

        return new OrderDtoDb
        {
            Uuid = apiDto.Uuid,
            StartTime = apiDto.StartTime,
            EstimatedTime = apiDto.EstimatedTime,
            CompletionTime = apiDto.CompletionTime,
            MappedStatus = apiDto.MappedStatus,
            OrderType = apiDto.OrderType,
            PreOrderTime = apiDto.PreOrderTime,
            OrderCategory = apiDto.OrderCategory,
            Priority = apiDto.Priority,
            Cost = apiDto.Cost,
            dishesnew = new [] {1, 2 } 
        };

}
}
}
