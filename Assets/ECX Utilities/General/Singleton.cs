/*
ECX UTILITY SCRIPTS
Singleton: Generic Template (Unity MonoBehaviour)
A flexible pre-made Unity singleton script that can be implemented simply by inheriting from this class.
Allows creation of multiple separate types of singleton without conflict (e.g. AudioManager singleton, UIManager singleton, etc)
Last updated: Apr 16, 2022
*/

using UnityEngine;

namespace EcxUtilities {

  /// <summary>
  /// A flexible pre-made Unity singleton script that can be implemented simply by inheriting from this class.
  /// Allows creation of multiple separate types of singleton without conflict (e.g. AudioManager singleton, UIManager singleton, etc)
  /// syntax: public class ClassName : Singleton<ClassName> {
  /// </summary>
  /// <typeparam name="T">Type</typeparam>
  public class Singleton<T> : MonoBehaviour where T : Singleton<T> {
    private static T instance;
    public static T Instance {
      get {
        // Return the singleton instance if it exists (most common scenario)
        if (instance != null) {
          return instance;
        }
        // Create singleton instance (only done once)
        else {
          // Search for all objects of this singletom type in scene
          T[] managers = Object.FindObjectsOfType(typeof(T)) as T[];
          // Create new Singleton if not found in scene
          if (managers.Length == 0) {
            GameObject go = new GameObject(typeof(T).Name, typeof(T));
            instance = go.GetComponent<T>();
            return instance;
          }
          // Found the singleton, assign it to instance
          else if (managers.Length == 1) {
            instance = managers[0];
            instance.gameObject.name = typeof(T).Name;
            return instance;
          }
          // Multiple singletons found...no bueno.  Should still work without crashing so send warning message.
          else {
            Debug.LogWarning("WARNING: Multiple copies of singleton: " + typeof(T).Name + " found. Make sure there is only one.");
            instance = managers[managers.Length-1]; // this should return topmost item in Unity hierarchy
            instance.gameObject.name = typeof(T).Name;
            return instance;
          }
        }
      }
      set {
        instance = value as T;
      }
    }
  }

}
