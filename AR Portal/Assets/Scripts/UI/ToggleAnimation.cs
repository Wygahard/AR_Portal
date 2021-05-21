using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(Button))]
public class ToggleAnimation : MonoBehaviour
{

    private RectTransform rectTransform;
    private Button button;

    public AnimationCurve animationCurve;
    public float animationTime = 0.5f;
    public float animationDegrees;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
    }

    private void OnDisable()
    {
        rectTransform.eulerAngles = new Vector3(0, 0, 0);
        StopAllCoroutines();
        button.interactable = true;
    }

    public void TurnToggle()
    {
        button.interactable = false;
        StartCoroutine(AnimateTurn(animationDegrees, animationTime));
    }

    
     public IEnumerator AnimateTurn(float degrees, float time)
    {
        float currentDegrees = rectTransform.eulerAngles.z;
        float targetDegrees = currentDegrees + degrees;
        float elapsedTime = 0;

        while(elapsedTime < time)
        {
            rectTransform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(currentDegrees,
                targetDegrees, animationCurve.Evaluate(elapsedTime / time)));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();

        }

        button.interactable = true;
        rectTransform.eulerAngles = new Vector3(0, 0, targetDegrees);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
