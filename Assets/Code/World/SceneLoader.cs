using UnityEngine;
using UnityEngine.SceneManagement;

namespace FluffyGameDev.Escapists.World
{
    public class SceneLoader : MonoBehaviour //TODO: turn into a service after locator has been implemented
    {
        public enum SceneID
        {
            PersistentScene,
            GameWorld
        }

        private void Awake()
        {
            bool isSceneLoaded = false;
            for (int i = 0; i < SceneManager.sceneCount; ++i)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene != null && scene.name == "GameWorld")
                {
                    isSceneLoaded = true;
                    break;
                }
            }

            if (!isSceneLoaded)
                SceneManager.LoadSceneAsync((int)SceneID.GameWorld, LoadSceneMode.Additive);
        }
    }
}