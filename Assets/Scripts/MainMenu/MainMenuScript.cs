using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
  
    public void OnPlayPress()
    {
        SceneManager.LoadScene("DeBoomgaarde");
    }

    public void OnQuitPress()
    {
        Debug.Log("quits");
        Application.Quit();
    }
}
