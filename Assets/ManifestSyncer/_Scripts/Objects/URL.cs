using System.IO;
using System.Threading;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;
using UnityEngine.Networking;

namespace ManifestSyncer 
{
	[System.Serializable]
	public class URL {

		public string path;
		public string version; 
		public ulong filesize;
		public string baseURL; 

		public bool hasAssetLocally { 
			get { 
				return File.Exists (LocalAssetPath());
			} 
		}	

		/// <summary>
		/// Path to access the URL asset if it has been downloaded
		/// </summary>
		/// <returns>The asset path.</returns>
		public string LocalAssetPath()
		{
			string localPath = string.Concat ("dwldTextures", path);
			return Path.Combine(Application.persistentDataPath, localPath);
		}

		/// <summary>
		/// Remote address to dowload URL asset 
		/// </summary>
		/// <returns>The asset URL.</returns>
		public string RemoteAssetURL()
		{
			return string.Concat (baseURL, path);
		}



		public string Description ()
		{
			string description = string.Format (
				"path:{0}\n" +
				"version:{1}\n"+
				"filesize:{2}\n" +
				"base:{3}\n", path, version, filesize, baseURL);
			return description;
		}
	}
}
