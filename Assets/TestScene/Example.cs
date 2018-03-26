using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ManifestSyncer;

public class Example : MonoBehaviour {

	[SerializeField]
	private ManifestCore _manifest;

	// Use this for initialization
	void Start () 
	{

		// Get default manifest from here
		_manifest.OnManifestUpdate+= OnManifestUpdate;
		_manifest.OnManifestFailed += OnManifestFailed;
		_manifest.OnRequestUpdate += OnRequestUpdate;
	}

	private void OnManifestUpdate() 
	{
		Debug.Log ("The local Manifest received an update from the server.");
	}

	private void OnManifestFailed()
	{
		Debug.Log ("The manifest failed to load");
	}

	private void OnRequestUpdate(ulong progress, Asset item) 
	{
		double _progress = ((double)progress /item.url.filesize);

		string log =string.Format ("Progress for file {0}: {1:0.##\\\\%}", item.id, _progress);
		Debug.Log (log);
	}


	// Example of how to call the ManifestCore class

	private void OnGUI() 
	{
		float height = Screen.height / 8f;
		float width = Screen.width / 3f;

		Rect updateManifest = new Rect (0, 0, width, height);
		if (GUI.Button(updateManifest, "UPdate Manifest"))
		{
			_manifest.FetchManifestAsync ();	
		}

		Rect updateManifestContent = new Rect (width, 0, width, height);
		if (GUI.Button(updateManifestContent, "Sync content"))
		{
			_manifest.FetchManifestContentAsync ();	
		}
	}

}
