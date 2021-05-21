using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour
{
    public string faceName = "Face";
    public Sprite faceIcon;

    List<Interaction> interactions = new List<Interaction>();

    void Start()
    {
        foreach (Interaction interaction in transform.GetComponentsInChildren<Interaction>(true))
            interactions.Add(interaction);
    }

    public void StartInteraction()
    {
       foreach(Interaction interaction in interactions)
        {
            interaction.Interact();
        }
    }

    public void ResetInteraction()
    {
        foreach (Interaction interaction in interactions)
        {
            interaction.ResetInteraction();
        }
    }

}
