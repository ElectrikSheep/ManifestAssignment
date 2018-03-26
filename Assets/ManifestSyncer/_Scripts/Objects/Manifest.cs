using System.Runtime.Serialization.Formatters.Binary;

using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ManifestSyncer 
{
	[System.Serializable]
	public class Manifest 
	{

		public Asset[] assets;
		public Meta _meta;

		public string Description() 
		{
			if (null != assets && assets.Length > 0) {
				string description = string.Format ("Items Count: {0}\nVersion:{1}", assets.Length, _meta._content_version);
				return description;
			}
			return "Empty Manifest";
		}

		public static bool Equals(Manifest m1, Manifest m2)
		{
			if (null == m1 || null == m2) {
				return false;
			}
			if (string.Equals (
				   m1._meta._content_version, 
				   m2._meta._content_version)) {
				return true;
			}
			return false;
		}

	}
		
	[System.Serializable]
	public struct Meta
	{
		public string _content_version;
	}
}

