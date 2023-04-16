using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLoader : MonoBehaviour
{
    public int nextScene;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SceneManager.LoadScene(nextScene);
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

    }
}
