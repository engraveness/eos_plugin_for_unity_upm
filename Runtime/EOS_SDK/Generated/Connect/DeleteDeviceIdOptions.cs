// Copyright Epic Games, Inc. All Rights Reserved.
// This file is automatically generated. Changes to this file may be overwritten.

namespace Epic.OnlineServices.Connect
{
	/// <summary>
	/// Input parameters for the <see cref="ConnectInterface.DeleteDeviceId" /> function.
	/// </summary>
	public class DeleteDeviceIdOptions
	{
	}

	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	internal struct DeleteDeviceIdOptionsInternal : ISettable, System.IDisposable
	{
		private int m_ApiVersion;

		public void Set(DeleteDeviceIdOptions other)
		{
			if (other != null)
			{
				m_ApiVersion = ConnectInterface.DeletedeviceidApiLatest;
			}
		}

		public void Set(object other)
		{
			Set(other as DeleteDeviceIdOptions);
		}

		public void Dispose()
		{
		}
	}
}