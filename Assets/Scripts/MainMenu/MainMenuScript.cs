using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
public class MainMenuScript : MonoBehaviour
{

    public GameObject imagePlay;
    public GameObject imageControlSelect;

    public GameObject playButton;

    public PlayerInputManager playerInputManager;

    private void Start()
    {
        imagePlay.SetActive(false);
        imageControlSelect.SetActive(true);
       
    }
    public void OnArcadePress()
    {
        playerInputManager.isKBM = true;
        playerInputManager.SwitchControlScheme();

        imagePlay.SetActive(true);
        imageControlSelect.SetActive(false);
        
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(playButton);
    }

    public void OnPCPress()
    {
        playerInputManager.isKBM = false;
        playerInputManager.SwitchControlScheme();

        imagePlay.SetActive(true);
        imageControlSelect.SetActive(false);
        
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(playButton);
    }

    public void BackToMainPress()
    {
        Destroy(GameObject.Find("GameManager"));
        SceneManager.LoadScene("MainMenu");
        
        Time.timeScale = 1;
    }
    public void OnPlayPress()
    {
        SceneManager.LoadScene("DeBoomgaarde");
        Time.timeScale = 1;
    }

    public void OnQuitPress()
    {
        Debug.Log("quits");
        Application.Quit();
    }
}
