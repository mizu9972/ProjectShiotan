using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarcAnimation : MonoBehaviour
{
    [SerializeField]
    private GameObject AnimationModel;
    private Animator animator;

    // Start is called before the first frame update
    void Start() {
        animator = AnimationModel.GetComponent<Animator>();
    }

    public void SetIsAttack(bool Value) {
        animator.SetBool("IsAttack", Value);
    }
}
