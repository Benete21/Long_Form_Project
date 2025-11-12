using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToEndScene : MonoBehaviour
{
    [Header("References")]
    public RawImage panelImage;

    [Header("Color Settings")]
    public Color startColor;
    public Color targetColor;
    public float duration;
    private Coroutine colorChangeRoutine;


    public void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ChangeColorOverTime() );
            Debug.Log("YEah");
        }
    }



    IEnumerator ChangeColorOverTime()
    {
        float elapsed = 0f;
        panelImage.color = startColor;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            panelImage.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        panelImage.color = targetColor;
        SceneManager.LoadScene("ENDSCENE");
    }


   

}




