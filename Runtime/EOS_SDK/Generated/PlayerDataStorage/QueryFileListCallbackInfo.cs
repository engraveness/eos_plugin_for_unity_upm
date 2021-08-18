// Copyright Epic Games, Inc. All Rights Reserved.
// This file is automatically generated. Changes to this file may be overwritten.

namespace Epic.OnlineServices.PlayerDataStorage
{
	/// <summary>
	/// Data containing information about a query file list request
	/// </summary>
	public class QueryFileListCallbackInfo : ICallbackInfo, ISettable
	{
		/// <summary>
		/// Result code for the operation. <see cref="Result.Success" /> is returned for a successful request, other codes indicate an error
		/// </summary>
		public Result ResultCode { get; private set; }

		/// <summary>
		/// Client-specified data passed into the file query request
		/// </summary>
		public object ClientData { get; private set; }

		/// <summary>
		/// The Product User ID of the local user who initiated this request
		/// </summary>
		public ProductUserId LocalUserId { get; private set; }

		/// <summary>
		/// A count of files that were found, if successful
		/// </summary>
		public uint FileCount { get; private set; }

		public Result? GetResultCode()
		{
			return ResultCode;
		}

		internal void Set(QueryFileListCallbackInfoInternal? other)
		{
			if (other != null)
			{
				ResultCode = other.Value.ResultCode;
				ClientData = other.Value.ClientData;
				LocalUserId = other.Value.LocalUserId;
				FileCount = other.Value.FileCount;
			}
		}

		public void Set(object other)
		{
			Set(other as QueryFileListCallbackInfoInternal?);
		}
	}

	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	internal struct QueryFileListCallbackInfoInternal : ICallbackInfoInternal
	{
		private Result m_ResultCode;
		private System.IntPtr m_ClientData;
		private System.IntPtr m_LocalUserId;
		private uint m_FileCount;

		public Result ResultCode
		{
			get
			{
				return m_ResultCode;
			}
		}

		public object ClientData
		{
			get
			{
				object value;
				Helper.TryMarshalGet(m_ClientData, out value);
				return value;
			}
		}

		public System.IntPtr ClientDataAddress
		{
			get
			{
				return m_ClientData;
			}
		}

		public ProductUserId LocalUserId
		{
			get
			{
				ProductUserId value;
				Helper.TryMarshalGet(m_LocalUserId, out value);
				return value;
			}
		}

		public uint FileCount
		{
			get
			{
				return m_FileCount;
			}
		}
	}
}