using System;
using UnityEngine;

namespace Json {
    public static class Loader {
        /*
            @param filePath - path starting after Resources, omit .json extension
            eg: loading Resources/Fish.json would be filePath = "Fish"
        */
        public static T[] LoadJson<T>(string filePath) {
            TextAsset textAsset = Resources.Load<TextAsset>(filePath);
            Root<T> root = JsonUtility.FromJson<Root<T>>(textAsset.text);
            return root.root;
        }
    }


    /*
    This class is only used for pulling data out of json files
    */
    [Serializable]
    public class Root<T> {
        public T[] root;
    }
}

