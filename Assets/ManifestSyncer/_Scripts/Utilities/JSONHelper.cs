using System;
using UnityEngine;

namespace ManifestSyncer
{
	/*
	public static class JSONHelper
	{
		public static T[] FromJson<T>(string json)
		{
			string formatedJSON = json.Replace ("base", "baseURL");
			return formatedJSON;

			Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(formatedJSON);
			return wrapper.Items;
		}

		public static string ToJson<T>(T[] array)
		{
			Wrapper<T> wrapper = new Wrapper<T>();
			wrapper.Items = array;
			return JsonUtility.ToJson(wrapper);
		}

		public static string ToJson<T>(T[] array, bool prettyPrint)
		{
			Wrapper<T> wrapper = new Wrapper<T>();
			wrapper.Items = array;
			return JsonUtility.ToJson(wrapper, prettyPrint);
		}

		[Serializable]
		private class Wrapper<T>
		{
			public T[] Items;
		}
	}
	*/
}