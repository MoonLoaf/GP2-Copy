using TMPro;
using UnityEngine;
public class UIManager : MonoBehaviour {

    [SerializeField] private TMP_InputField scenetext;
    public void LoadGame() {
        SceneManager.Instance.LoadScene("OSkar");
    }

    public void LoadCustomScene() {
        SceneManager.Instance.LoadScene(scenetext.text);
        Debug.Log("Tried Loading scene " + scenetext.text);
    }
    public void QuitGame() {
        Application.Quit();
    }
}