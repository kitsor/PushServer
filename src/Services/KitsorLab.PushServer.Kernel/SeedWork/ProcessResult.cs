using System;
using System.Collections.Generic;
using System.Linq;

namespace KitsorLab.PushServer.Kernel.SeedWork
{
	public class ProcessResult
	{
		public bool IsSuccess => string.IsNullOrEmpty(ErrorMsg) && ErrorCode == 0;

		public string ErrorMsg { get; private set; }
		public int ErrorCode { get; private set; }

		/// <param name="errorMsg">The error MSG.</param>
		/// <param name="errorType">Type of the error.</param>
		/// <param name="errorCode">The error code.</param>
		public void SetErrorInfo(string errorMsg, int errorCode = 0)
		{
			if (string.IsNullOrWhiteSpace(errorMsg) && errorCode == 0)
				throw new ArgumentNullException("errorMsg");

			ErrorMsg = errorMsg;
			ErrorCode = errorCode;
		}

		/// <param name="source"></param>
		public void PopulateErrorFromResult(ProcessResult source)
		{
			if (source == null)
			{
				return;
			}
			ErrorCode = source.ErrorCode;
			ErrorMsg = source.ErrorMsg;
		}
	}

	public class ProcessResult<T> : ProcessResult
	{
		public T ReturnValue { get; set; }

		public ProcessResult()
		{
		}

		/// <param name="returnValue">The return value.</param>
		public ProcessResult(T returnValue)
		{
			ReturnValue = returnValue;
		}
	}

	public static class ProcessResultExtensions
	{
		/// <param name="results">The results.</param>
		/// <returns></returns>
		public static IEnumerable<ProcessResult> Successes(this IEnumerable<ProcessResult> results)
		{
			if (results == null)
				return null;

			return results.Where(x => x.IsSuccess);
		}

		/// <param name="results">The results.</param>
		/// <returns></returns>
		public static IEnumerable<ProcessResult> Failures(this IEnumerable<ProcessResult> results)
		{
			if (results == null)
				return null;

			return results.Where(x => !x.IsSuccess);
		}

		/// <typeparam name="T"></typeparam>
		/// <param name="results">The results.</param>
		/// <returns></returns>
		public static IEnumerable<T> Successes<T>(this IEnumerable<ProcessResult<T>> results)
		{
			if (results == null)
				return null;

			return results.Where(r => r.IsSuccess).Select(r => r.ReturnValue);
		}

		/// <typeparam name="T"></typeparam>
		/// <param name="results">The results.</param>
		/// <returns></returns>
		public static IEnumerable<ProcessResult<T>> Failures<T>(this IEnumerable<ProcessResult<T>> results)
		{
			if (results == null)
				return null;

			return results.Where(x => !x.IsSuccess);
		}
	}
}
