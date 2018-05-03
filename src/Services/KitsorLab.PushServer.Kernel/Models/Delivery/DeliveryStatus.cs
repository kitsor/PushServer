namespace KitsorLab.PushServer.Kernel.Models.Delivery
{
	public enum DeliveryStatus : byte
	{
		New = 0,
		InQueue = 5,
		HasBeenSent = 10,
		UnknownError = 99,
	}
}
