namespace BikeStore.Domain.Entities.Sales;

public partial class Order
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public OrderStatus OrderStatus { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime RequiredDate { get; set; }

    public DateTime? ShippedDate { get; set; }

    public int StoreId { get; set; }

    public int StaffId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Staff? Staff { get; set; }

    public virtual Store? Store { get; set; }
}

public enum OrderStatus : byte
{ 
    Pending = 1,
    Processing = 2,
    Rejected = 3,
    Completed = 4
}
