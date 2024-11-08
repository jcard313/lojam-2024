using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    bool mPaused = false;
    public void OnTitleButton()
    {
        SceneManager.LoadScene(0);
    }
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
            if (mPaused)
            {
                resume();
            }
            else
            {
                pause();
            }
    }
    void resume()
    {
        Time.timeScale = 1f;
        mPaused = true;
    }
    void pause()
    {
        Time.timeScale = 0f;
        mPaused = true;
    }
}
