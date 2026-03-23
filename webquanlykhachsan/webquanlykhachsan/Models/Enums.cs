namespace webquanlykhachsan.Models
{
    public enum RoomStatus
    {
        Available,
        Occupied,
        Cleaning,
        Maintenance
    }

    public enum BookingStatus
    {
        Pending,
        Confirmed,
        CheckedIn,
        Completed,
        Cancelled
    }

    public enum InvoiceStatus
    {
        Unpaid,
        Paid,
        Cancelled
    }
}
