// Copyright Epic Games, Inc. All Rights Reserved.
// This file is automatically generated. Changes to this file may be overwritten.

namespace Epic.OnlineServices.AntiCheatCommon
{
	public struct LogPlayerTickOptions
	{
		/// <summary>
		/// Locally unique value used in RegisterClient/RegisterPeer
		/// </summary>
		public System.IntPtr PlayerHandle { get; set; }

		/// <summary>
		/// Player character's current world position as a 3D vector. This should be the center of the character.
		/// </summary>
		public Vec3f? PlayerPosition { get; set; }

		/// <summary>
		/// Player camera's current world rotation as a quaternion.
		/// </summary>
		public Quat? PlayerViewRotation { get; set; }

		/// <summary>
		/// True if the player's view is zoomed (e.g. using a sniper rifle), otherwise false
		/// </summary>
		public bool IsPlayerViewZoomed { get; set; }

		/// <summary>
		/// Player's current health value
		/// </summary>
		public float PlayerHealth { get; set; }

		/// <summary>
		/// Any movement state applicable
		/// </summary>
		public AntiCheatCommonPlayerMovementState PlayerMovementState { get; set; }

		/// <summary>
		/// Player camera's current world position as a 3D vector.
		/// </summary>
		public Vec3f? PlayerViewPosition { get; set; }
	}

	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	internal struct LogPlayerTickOptionsInternal : ISettable<LogPlayerTickOptions>, System.IDisposable
	{
		private int m_ApiVersion;
		private System.IntPtr m_PlayerHandle;
		private System.IntPtr m_PlayerPosition;
		private System.IntPtr m_PlayerViewRotation;
		private int m_IsPlayerViewZoomed;
		private float m_PlayerHealth;
		private AntiCheatCommonPlayerMovementState m_PlayerMovementState;
		private System.IntPtr m_PlayerViewPosition;

		public System.IntPtr PlayerHandle
		{
			set
			{
				m_PlayerHandle = value;
			}
		}

		public Vec3f? PlayerPosition
		{
			set
			{
				Helper.Set<Vec3f, Vec3fInternal>(ref value, ref m_PlayerPosition);
			}
		}

		public Quat? PlayerViewRotation
		{
			set
			{
				Helper.Set<Quat, QuatInternal>(ref value, ref m_PlayerViewRotation);
			}
		}

		public bool IsPlayerViewZoomed
		{
			set
			{
				Helper.Set(value, ref m_IsPlayerViewZoomed);
			}
		}

		public float PlayerHealth
		{
			set
			{
				m_PlayerHealth = value;
			}
		}

		public AntiCheatCommonPlayerMovementState PlayerMovementState
		{
			set
			{
				m_PlayerMovementState = value;
			}
		}

		public Vec3f? PlayerViewPosition
		{
			set
			{
				Helper.Set<Vec3f, Vec3fInternal>(ref value, ref m_PlayerViewPosition);
			}
		}

		public void Set(ref LogPlayerTickOptions other)
		{
			m_ApiVersion = AntiCheatCommonInterface.LogplayertickApiLatest;
			PlayerHandle = other.PlayerHandle;
			PlayerPosition = other.PlayerPosition;
			PlayerViewRotation = other.PlayerViewRotation;
			IsPlayerViewZoomed = other.IsPlayerViewZoomed;
			PlayerHealth = other.PlayerHealth;
			PlayerMovementState = other.PlayerMovementState;
			PlayerViewPosition = other.PlayerViewPosition;
		}

		public void Set(ref LogPlayerTickOptions? other)
		{
			if (other.HasValue)
			{
				m_ApiVersion = AntiCheatCommonInterface.LogplayertickApiLatest;
				PlayerHandle = other.Value.PlayerHandle;
				PlayerPosition = other.Value.PlayerPosition;
				PlayerViewRotation = other.Value.PlayerViewRotation;
				IsPlayerViewZoomed = other.Value.IsPlayerViewZoomed;
				PlayerHealth = other.Value.PlayerHealth;
				PlayerMovementState = other.Value.PlayerMovementState;
				PlayerViewPosition = other.Value.PlayerViewPosition;
			}
		}

		public void Dispose()
		{
			Helper.Dispose(ref m_PlayerHandle);
			Helper.Dispose(ref m_PlayerPosition);
			Helper.Dispose(ref m_PlayerViewRotation);
			Helper.Dispose(ref m_PlayerViewPosition);
		}
	}
}