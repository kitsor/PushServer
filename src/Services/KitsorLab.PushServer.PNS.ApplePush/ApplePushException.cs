namespace KitsorLab.PushServer.PNS.ApplePush
{
	using System;
	using System.Net;
	using System.Net.Http.Headers;

	public class ApplePushException : Exception
	{
		public HttpStatusCode StatusCode { get; set; }
		public HttpResponseHeaders Headers { get; set; }
		public string DeviceToken { get; set; }

		public ApplePushException(string message, HttpStatusCode statusCode, HttpResponseHeaders headers, 
			string deviceToken) : base(message)
		{
			StatusCode = statusCode;
			Headers = headers;
			DeviceToken = deviceToken;
		}
	}
}
