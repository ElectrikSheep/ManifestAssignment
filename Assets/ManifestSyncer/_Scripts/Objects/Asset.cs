using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManifestSyncer
{

	[System.Serializable]
	public class Asset 
	{
		public string id;
		public URL url;

		public void SaveAssetContent( byte[] rawData )
		{
			Persistence.Save_DataAtPath (rawData, url.LocalAssetPath());
		}

		public void DeleteAssetContent()
		{
			Persistence.Delete_DataAtPath (url.LocalAssetPath());
		}

	}
}


