# ManifestAssignment

Quick project to download and sync a manifest file.
And from this manifest file, downloading and saving a list of Assets.  

## TO TEST

To try out the project,  
• open the scene in Assets/TestScene/TestScene.Unity  
• press Play  
• use GUI button options to download/sync content


## The ManifestCore Prefab

In the Hierarchy of the Scene you can see the ManifestCore Prefab. It's the object you would otherwise install in your individual projects.
In the inspector you can set an array of options to customize how the object should behave:  
• Auto Update Manifest: will download a Manifest on Start if set to true  
• Auto Sync Content: will download the asset items in the Manifest and save them to disc after receiving a new Manifest


