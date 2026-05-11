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

    private void Start()
    {
        imagePlay.SetActive(false);
        imageControlSelect.SetActive(true);
    }
    public void OnArcadePress()
    {
        imagePlay.SetActive(true);
        imageControlSelect.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(playButton);
    }

    public void OnPCPress()
    {
        imagePlay.SetActive(true);
        imageControlSelect.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(playButton);
    }
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
