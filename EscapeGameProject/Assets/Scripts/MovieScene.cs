using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovieScene : MonoBehaviour
{
    private bool isFading = false;

    public float fadeDuration = 7f;
    public CanvasGroup panelCanvasGroup;

    private bool skipTrue = false;
    private float skipTimer = 0f;
    void Start()
    {
        StartCoroutine(SceneSkip());
    }
    void Update()
    {
     if(Input.GetKeyDown(KeyCode.G)) 
        {
        skipTrue = true;
        }   
     if(skipTrue)
        {
            skipTimer += Time.deltaTime;
            if(skipTimer >= 2f) 
            {
                SceneManager.LoadScene("Main");
            }
        }
     if(Input.GetKeyUp(KeyCode.G))
        {
            skipTrue = false;
            skipTimer = 0f;
        }
    }

    IEnumerator SceneSkip()
    {
        yield return new WaitForSecondsRealtime(132);
        {
            isFading = true;

            panelCanvasGroup.gameObject.SetActive(true);
            float timer = 0f;
            while (timer < fadeDuration)
            {
                panelCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                timer += Time.deltaTime;
            }
            yield return new WaitForSecondsRealtime(3);
            SceneManager.LoadScene("Main");


        }

        
    }
}
