using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhaAnimation : MonoBehaviour
{
    private List<GameObject> AnimationModel = new List<GameObject>();
    private List<Animator> animator = new List<Animator>();

    public void InitPiranhaAnimation(List<GameObject> modelList) {
        AnimationModel = modelList;

        for(int i  = 0; i < AnimationModel.Count; i++) {
            animator.Add(AnimationModel[i].GetComponent<Animator>());
        }
    }

    public void SetIsAttack(bool Value) {
        foreach(Animator setAnimator in animator) {
            setAnimator.SetBool("IsAttack", Value);
        }
    }
}
