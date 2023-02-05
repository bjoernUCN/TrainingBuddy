using UnityEngine;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
namespace TrainingBuddy.Managers
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static object _lock = new object();

        private static T instance;

        public static T Instance
        {
            get
            {
                if (applicationIsQuitting)
                {
                    Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                                     "' already destroyed on application quit." +
                                     " Won't create again - returning null.");
                    return null;
                }

                lock (_lock)
                {
                    if (instance == null)
                    {
                        var instances = FindObjectsOfType<T>();

                        if (instances.Length == 0)
                        {
                            //GameObject singleton = new GameObject();
                            //instance = singleton.AddComponent<T>();
                            //singleton.name = "(singleton) " + typeof(T).ToString();

                            //DontDestroyOnLoad(singleton);

                            //Debug.Log("[Singleton] An instance of " + typeof(T) +
                            //          " is needed in the scene, so '" + singleton);
                            //"' was created with DontDestroyOnLoad.");
                            Debug.Log("No instance of singleton " + typeof(T));
                            return null;
                        }

                        instance = instances[0];

                        if (instances.Length > 1)
                        {
                            Debug.LogError("[Singleton] Something went really wrong " +
                                           " - there should never be more than 1 singleton!" +
                                           " Reopening the scene might fix it.");
                        }
                        else
                        {
                           // Debug.Log("[Singleton] Using instance already created: " + instance.gameObject.name);
                        }
                    }

                    return instance;
                }
            }
        }

        public static bool applicationIsQuitting;

        /// <summary>
        /// When Unity quits, it destroys objects in a random order.
        /// In principle, a Singleton is only destroyed when application quits.
        /// If any script calls Instance after it have been destroyed, 
        /// it will create a buggy ghost object that will stay on the Editor scene
        /// even after stopping playing the Application. Really bad!
        /// So, this was made to be sure we're not creating that buggy ghost object.
        /// </summary>
        protected virtual void OnDestroy()
        {
            applicationIsQuitting = true;
        }

		protected virtual void OnEnable()
		{
			applicationIsQuitting = false;
		}

        // workaround for ApplicationIsQuitting set to true on scene unload due to object being destroyed
        // On loading a new scene, a few objects (Modules) try to access managers in Awake, however
        // applicationIsQuitting is still set to false (OnEnable runs afterward)
        // this workaround will fix the issue for now - Marco
        protected virtual void Awake()
        {
            applicationIsQuitting = false;
        }
    }
}
