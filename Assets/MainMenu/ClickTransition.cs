using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClickTransition : MonoBehaviour {
	public int sceneNumber;
	void Start() {
		GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene(sceneNumber); });
	}
}