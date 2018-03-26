using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace ManifestSyncer{

	public class ManifestCore : MonoBehaviour {

		[SerializeField]
		[Tooltip("If set to true, will request a new Manifest from the server as soon as the object Start")]
		private bool autoUpdateManifest = false;
		[SerializeField]
		[Tooltip("If set to true, will automatically remove old unused assets and download the new ones")]
		private bool autoDownloadContent = false;

		private Manifest manifest;

		/// <summary>
		/// Boolean to limit our requests to only one concurent
		/// </summary>
		private bool requestInProgress;
		private DownloadManager downloader;

		/// <summary>
		/// The remote URL to download the Manifest
		/// To set in the Editor Inspector
		/// </summary>
		[SerializeField]
		private string manifestURL = "";



		public delegate void RequestTerminated();
		/// <summary>
		/// Occurs when a new Manifest is updated
		/// This event is not called if the Manifests are the same (local vs remote)
		/// </summary>
		public event RequestTerminated OnManifestUpdate;
		/// <summary>
		/// Occurs when on manifest failed to be downloaded
		/// </summary>
		public event RequestTerminated OnManifestFailed;
		/// <summary>
		/// Occurs when content of current Manifest is synced on the device
		/// If content is already present
		/// </summary>
		public event RequestTerminated OnContentSynced ;




		public delegate void RequestUpdate(ulong downloadedByte, Asset itemDownloaded) ;
		/// <summary>
		/// Occurs on every frame when downloading content
		/// </summary>
		public event RequestUpdate OnRequestUpdate;



		void Awake() 
		{
			requestInProgress = false;
			downloader = new DownloadManager ();

			manifest = Persistence.LoadLocalManifest ();
		}

		void Start () 
		{
			if (autoUpdateManifest) 
			{
				FetchManifestAsync();
			}
		}

		public void FetchManifestAsync()
		{
			if (requestInProgress) {
				return;
			}
			requestInProgress = true;

			downloader.OnDownloadCompletion = OnDownloadCompletion;
			StartCoroutine(downloader.DownloadAsset (manifestURL));
		}

		public void FetchManifestContentAsync()
		{
			// Fetch remote MANIFEST
			if (requestInProgress) {
				return;
			}
			requestInProgress = true;

			StartCoroutine (FetchContentForManifest (manifest));
		}







		private IEnumerator FetchContentForManifest( Manifest _manifest)
		{
			DownloadManager downloader = new DownloadManager ();

			if (_manifest.assets == null || _manifest.assets.Length <= 0) 
			{
				// Empty Manifest
				yield return null;
			}
			foreach (Asset _item in _manifest.assets) 
			{
				if (false == _item.url.hasAssetLocally) 
				{
					// Save item when download is done
					downloader.OnDownloadCompletion = delegate (UnityWebRequest request) {
						if (null != request) {
							_item.SaveAssetContent( request.downloadHandler.data );
						}
					};

					downloader.OnDownloadUpdate = delegate (UnityWebRequest request) {
						if (null != request) {
							if (null != OnRequestUpdate) {
								OnRequestUpdate (request.downloadedBytes, _item );
							}
						}
					};

					yield return downloader.DownloadAsset (_item.url.RemoteAssetURL ());
				} 
			}
			requestInProgress = false;
		}
			
		private void OnDownloadCompletion( UnityWebRequest request)
		{
			if (null == request) 
			{
				if (null != OnManifestFailed) 
				{
					OnManifestFailed ();
				}

				requestInProgress = false;
				return;
			}

			string formatedJSON = request.downloadHandler.text.Replace ("base", "baseURL");
			Manifest remoteManifest = JsonUtility.FromJson<Manifest>(formatedJSON);

			if (false == Manifest.Equals( manifest, remoteManifest) ) 
			{
				ResolveConflicts (manifest, remoteManifest);
				manifest = remoteManifest;
				Persistence.SaveManifest (manifest);

				if (null != OnManifestUpdate) 
				{
					OnManifestUpdate ();
				}
			} 
			requestInProgress = false;

			if (autoDownloadContent) {
				FetchManifestContentAsync ();
			}
		}


		/// <summary>
		/// Resolves conflicts between old and new Manifest
		/// Will delete the Assets present in old Manifest and NOT in new one
		/// </summary>
		/// <param name="oldManifest">Old manifest.</param>
		/// <param name="newManifest">New manifest.</param>
		private void ResolveConflicts( Manifest oldManifest, Manifest newManifest)
		{
			if (oldManifest.assets == null || oldManifest.assets.Length == 0) 
			{
				return;
			}

			HashSet<Asset> table = new HashSet<Asset>(newManifest.assets);
			int elemntCount = table.Count;
			foreach (Asset _item in oldManifest.assets) 
			{
				table.Add (_item);
				if (table.Count != elemntCount) 
				{
					// New element does not exist in current hash, delete it
					_item.DeleteAssetContent() ;
				}
			}
		}
			

		private void OnApplicationQuit()
		{
			// Sync Manifest onto local device when quitting
			if( manifest !=null)
			{
				Persistence.SaveManifest (manifest);
			}
		}

	}

}
