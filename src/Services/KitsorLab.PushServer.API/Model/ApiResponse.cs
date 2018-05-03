namespace KitsorLab.PushServer.API.Model
{
	public class ApiResponse
	{
		public bool IsSuccess => string.IsNullOrEmpty(ErrorMsg) && ErrorCode == 0;

		public string ErrorMsg { get; private set; }
		public int ErrorCode { get; private set; }

		/// <param name="errorMsg"></param>
		/// <param name="errorCode"></param>
		public ApiResponse(string errorMsg = null, int errorCode = 0)
		{
			ErrorMsg = errorMsg;
			ErrorCode = errorCode;
		}
	}

	public class ApiResponse<T> : ApiResponse
	{
		public T Data { get; protected set; }

		/// <param name="data"></param>
		/// <param name="errorMsg"></param>
		/// <param name="errorCode"></param>
		public ApiResponse(T data, string errorMsg = null, int errorCode = 0)
			: base (errorMsg, errorCode)
		{
			Data = data;
		}
	}
}
