// Copyright Epic Games, Inc. All Rights Reserved.
// This file is automatically generated. Changes to this file may be overwritten.

namespace Epic.OnlineServices.Ecom
{
	/// <summary>
	/// Input parameters for the <see cref="EcomInterface.QueryEntitlementToken" /> function.
	/// </summary>
	public struct QueryEntitlementTokenOptions
	{
		/// <summary>
		/// The Epic Account ID of the local user whose Entitlements you want to retrieve
		/// </summary>
		public EpicAccountId LocalUserId { get; set; }

		/// <summary>
		/// An array of Entitlement Names that you want to check
		/// </summary>
		public Utf8String[] EntitlementNames { get; set; }
	}

	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	internal struct QueryEntitlementTokenOptionsInternal : ISettable<QueryEntitlementTokenOptions>, System.IDisposable
	{
		private int m_ApiVersion;
		private System.IntPtr m_LocalUserId;
		private System.IntPtr m_EntitlementNames;
		private uint m_EntitlementNameCount;

		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref m_LocalUserId);
			}
		}

		public Utf8String[] EntitlementNames
		{
			set
			{
				Helper.Set(value, ref m_EntitlementNames, out m_EntitlementNameCount);
			}
		}

		public void Set(ref QueryEntitlementTokenOptions other)
		{
			m_ApiVersion = EcomInterface.QueryentitlementtokenApiLatest;
			LocalUserId = other.LocalUserId;
			EntitlementNames = other.EntitlementNames;
		}

		public void Set(ref QueryEntitlementTokenOptions? other)
		{
			if (other.HasValue)
			{
				m_ApiVersion = EcomInterface.QueryentitlementtokenApiLatest;
				LocalUserId = other.Value.LocalUserId;
				EntitlementNames = other.Value.EntitlementNames;
			}
		}

		public void Dispose()
		{
			Helper.Dispose(ref m_LocalUserId);
			Helper.Dispose(ref m_EntitlementNames);
		}
	}
}