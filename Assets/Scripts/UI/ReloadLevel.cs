using UnityEngine;

public class ReloadLevel : MonoBehaviour
{
    public void ReloadScene(int sceneID)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneID);
    }
}
