// Copyright Epic Games, Inc. All Rights Reserved.
// This file is automatically generated. Changes to this file may be overwritten.

namespace Epic.OnlineServices.TitleStorage
{
	public sealed partial class TitleStorageFileTransferRequest : Handle
	{
		public TitleStorageFileTransferRequest()
		{
		}

		public TitleStorageFileTransferRequest(System.IntPtr innerHandle) : base(innerHandle)
		{
		}

		/// <summary>
		/// Attempt to cancel this file request in progress. This is a best-effort command and is not guaranteed to be successful if the request has completed before this function is called.
		/// </summary>
		/// <returns>
		/// <see cref="Result.Success" /> if cancel is successful, <see cref="Result.NoChange" /> if request had already completed (can't be canceled), <see cref="Result.AlreadyPending" /> if it's already been canceled before (this is a final state for canceled request and won't change over time).
		/// </returns>
		public Result CancelRequest()
		{
			var funcResult = Bindings.EOS_TitleStorageFileTransferRequest_CancelRequest(InnerHandle);

			return funcResult;
		}

		/// <summary>
		/// Get the current state of a file request.
		/// </summary>
		/// <returns>
		/// <see cref="Result.Success" /> if complete and successful, <see cref="Result.RequestInProgress" /> if the request is still in progress, or another state for failure.
		/// </returns>
		public Result GetFileRequestState()
		{
			var funcResult = Bindings.EOS_TitleStorageFileTransferRequest_GetFileRequestState(InnerHandle);

			return funcResult;
		}

		/// <summary>
		/// Get the file name of the file this request is for. OutStringLength will always be set to the string length of the file name if it is not NULL.
		/// <seealso cref="TitleStorageInterface.FilenameMaxLengthBytes" />
		/// </summary>
		/// <param name="filenameStringBufferSizeBytes">The maximum number of bytes that can be written to OutStringBuffer</param>
		/// <param name="outStringBuffer">The buffer to write the NULL-terminated utf8 file name into, if successful</param>
		/// <param name="outStringLength">How long the file name is (not including null terminator)</param>
		/// <returns>
		/// <see cref="Result.Success" /> if the file name was successfully written to OutFilenameBuffer, a failure result otherwise
		/// </returns>
		public Result GetFilename(out string outStringBuffer)
		{
			System.IntPtr outStringBufferAddress = System.IntPtr.Zero;
			int outStringLength = TitleStorageInterface.FilenameMaxLengthBytes;
			Helper.TryMarshalAllocate(ref outStringBufferAddress, outStringLength);

			var funcResult = Bindings.EOS_TitleStorageFileTransferRequest_GetFilename(InnerHandle, (uint)outStringLength, outStringBufferAddress, ref outStringLength);

			Helper.TryMarshalGet(outStringBufferAddress, out outStringBuffer);
			Helper.TryMarshalDispose(ref outStringBufferAddress);

			return funcResult;
		}

		/// <summary>
		/// Free the memory used by a cloud-storage file request handle. This will not cancel a request in progress.
		/// </summary>
		public void Release()
		{
			Bindings.EOS_TitleStorageFileTransferRequest_Release(InnerHandle);
		}
	}
}