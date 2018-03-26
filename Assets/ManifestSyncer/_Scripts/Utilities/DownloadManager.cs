using System;
using System.IO;
using System.Collections;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace ManifestSyncer 
{
	public class DownloadManager {

		public Action<UnityWebRequest> OnDownloadCompletion;
		public Action<UnityWebRequest> OnDownloadUpdate;

		public IEnumerator DownloadAsset(string path)
		{
			using (UnityWebRequest www = UnityWebRequest.Get(path))
			{
				www.SetRequestHeader ("Accept-Encoding", "gzip, deflate");
				UnityWebRequestAsyncOperation request = www.SendWebRequest ();

				while (false == request.isDone) 
				{
					yield return new WaitForEndOfFrame();

					if (OnDownloadUpdate != null) {
						OnDownloadUpdate (www);
					}
				}


				if (www.isNetworkError || www.isHttpError) 
				{
					if (OnDownloadCompletion != null) {
						OnDownloadCompletion (null);
					}
				} else 
				{
					if (OnDownloadCompletion != null) {
						OnDownloadCompletion (www);
					}
				}
			}
		}



	}	
}


