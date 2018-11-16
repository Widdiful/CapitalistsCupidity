using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour {

    public Canvas loadingCanvas;
    public Slider loadingBar;

    public void Load(int sceneIndex) {
        if (loadingCanvas)
            loadingCanvas.enabled = true;
        StartCoroutine(LoadAsync(sceneIndex));
    }

	IEnumerator LoadAsync(int sceneIndex) {
        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneIndex);

        // Update loading bar
        while (!loading.isDone) {
            float progress = Mathf.Clamp01(loading.progress / 0.9f);

            if (loadingBar)
                loadingBar.value = progress;

            yield return null;
        }
    }
}
