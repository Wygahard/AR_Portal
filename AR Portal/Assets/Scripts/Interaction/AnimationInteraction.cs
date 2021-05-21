using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationInteraction : MonoBehaviour, Interaction
{

    private Animator _animator;
    public string interactionAnimationName = "Interaction";
    public string idleAnimationName = "Idle";

   void Start()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void Interact()
    {
        _animator.Play(interactionAnimationName, -1, 0);
    }

    public void ResetInteraction()
    {
        _animator.Play(idleAnimationName, -1, 0);
    }

    private void OnDisable()
    {
        ResetInteraction();
    }

}
