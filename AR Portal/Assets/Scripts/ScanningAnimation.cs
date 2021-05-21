using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class ScanningAnimation : MonoBehaviour
{
    private RectTransform rectTransform;

    public Vector2 maximisedSize;
    public RectTransform maximizedTarget;

    public Vector2 minimisedSize;
    public RectTransform minimisedTarget;

    private bool firstTime = true;
    public bool isTracking = false;

    public Image icon;

    public float animationTime = 1;
    public AnimationCurve animationCurve;

    public Animator animator;

    public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    // Start is called before the first frame update
    private void Start()
    {
        icon = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        animator.SetBool("IsTracking", isTracking);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void MoveIcon(bool tracking)
    {
        if (tracking == isTracking && !firstTime)
            return;

        if (firstTime)
            firstTime = false;

        isTracking = tracking;
        animator.SetBool("IsTracking", isTracking);
        StopCoroutine("MoveAndResizeIcon");
        StartCoroutine(MoveAndResizeIcon(animationTime));      

    }

    public void ResetPosition()
    {
        rectTransform.anchoredPosition = isTracking ? minimisedTarget.anchoredPosition : maximizedTarget.anchoredPosition;
        rectTransform.sizeDelta = isTracking ? minimisedSize : maximisedSize;
    }


    public IEnumerator MoveAndResizeIcon(float time)
    {
        //Establish size and position
        Vector2 currentSizeDelta = rectTransform.sizeDelta;
        Vector2 currentPosition = rectTransform.anchoredPosition;

        float elapsedTime = 0;

        while(elapsedTime < time)
        {
            rectTransform.sizeDelta = Vector2.Lerp(currentSizeDelta, isTracking ? minimisedSize : maximisedSize,
                animationCurve.Evaluate(elapsedTime / time));

            rectTransform.anchoredPosition = Vector2.Lerp(currentPosition, isTracking ?
                minimisedTarget.anchoredPosition : maximizedTarget.anchoredPosition,
                animationCurve.Evaluate(elapsedTime / time));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
