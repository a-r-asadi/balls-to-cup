using UnityEngine;

namespace Singleton
{
    public static class Singleton
    {
        public static void SetInstance<T>(this MonoBehaviour newInstance, ref T instance) where T : MonoBehaviour
        {
#if UNITY_EDITOR
            if (instance != null)
            {
                Debug.LogError("Multiple instances of singleton " + newInstance.name);
            }
#endif
            instance = (T)newInstance;
        }
    }
}
