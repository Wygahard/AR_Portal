using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionManager : MonoBehaviour
{
    
    void Update()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if(touch.phase == TouchPhase.Began)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(touch.position);

            if(Physics.Raycast(ray, out hit) && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                hit.transform.GetComponent<Face>().StartInteraction();
            }
        }
    }
}
