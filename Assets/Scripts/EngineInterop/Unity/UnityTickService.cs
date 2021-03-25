using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI_Midterm_RTS.EngineInterop.Unity
{
    /// <summary>
    /// Implements a static method to access tick singletons.
    /// </summary>
    public static class UnityTickService
    {
        #region Static Fields
        private static Dictionary<UnityLoopType, ITickProvider> providers;
        static UnityTickService()
        {
            providers = new Dictionary<UnityLoopType, ITickProvider>();
        }
        #endregion
        #region ITickProvider Retriever
        /// <summary>
        /// Gets a shared instance of a tick provider using the specified loop.
        /// </summary>
        /// <param name="loopType">The Unity tick loop to use.</param>
        /// <returns>The tick provider.</returns>
        public static ITickProvider GetProvider(UnityLoopType loopType)
        {
            // Return the singleton game loop provider.
            if (providers.ContainsKey(loopType))
                return providers[loopType];
            else
            {
                // Create the singleton if it does not already exist.
                GameObject singleton;
                MonoBehaviour provider;
                switch (loopType)
                {
                    case UnityLoopType.Update:
                        singleton = new GameObject("UPDATE_SINGLETON");
                        provider = singleton.AddComponent<UnityUpdateTickProvider>();
                        GameObject.DontDestroyOnLoad(singleton);
                        providers.Add(loopType, (ITickProvider)provider);
                        break;
                    case UnityLoopType.FixedUpdate:
                        singleton = new GameObject("FIXED_UPDATE_SINGLETON");
                        provider = singleton.AddComponent<UnityFixedUpdateTickProvider>();
                        GameObject.DontDestroyOnLoad(singleton);
                        providers.Add(loopType, (ITickProvider)provider);
                        break;
                    default: throw new NotImplementedException();
                }
                return providers[loopType];
            }
        }
        #endregion
    }
}
