using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;


namespace ManifestSyncer 
{

	public class Persistence {

		private static string manifestLocalPath
		{
			get {return Application.persistentDataPath + "/appManifest.save"; }
		}


		/// <summary>
		/// Saves the data at path.
		/// </summary>
		/// <param name="_data">Data.</param>
		/// <param name="path">Path.</param>
		public static void Save_DataAtPath(byte[] _data, string path)
		{
			PreparePath (path);

			FileStream file = File.Open (path, FileMode.Create);
			var binary= new BinaryWriter(file);
			binary.Write(_data);
			file.Close();
		}

		/// <summary>
		/// Deletes the data at path.
		/// </summary>
		/// <param name="path">Path.</param>
		public static void Delete_DataAtPath( string path )
		{
			File.Delete (path);
		}
			
		/// <summary>
		/// Saves the given manifest.
		/// </summary>
		/// <param name="manifest">Manifest.</param>
		public static void SaveManifest(Manifest manifest)
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create(manifestLocalPath);
			bf.Serialize(file, manifest);
			file.Close();
		}

		/// <summary>
		/// Loads the local manifest if there is one
		/// Returns an empty one otherwise
		/// </summary>
		/// <returns>The local manifest.</returns>
		public static Manifest LoadLocalManifest() 
		{
			string persistencePath = manifestLocalPath;
			if (File.Exists (persistencePath)) {
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Open(persistencePath, FileMode.Open);
				Manifest loaded = null;

				try{
					loaded = (Manifest) bf.Deserialize(file);
				}
				catch{
					// Object is corrupted, remove
					File.Delete (persistencePath);
				}
				finally{
					if (null != file) {
						file.Close();
					}
				}

				return loaded;
			}

			// No manifest found, return blank one
			Manifest defaultManifest = new Manifest ();
			defaultManifest.assets = new Asset[0];
			return new Manifest();
		}


		private static void PreparePath(string path) 
		{
			DirectoryInfo info = Directory.CreateDirectory(Path.GetDirectoryName(path));
		}




	}
}

