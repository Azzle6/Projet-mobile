using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    public Animator enemyAnim;
    public EnemyBoid enemy;
    public enum AnimState { Idle, Move, Attack,Die}
    public AnimState animState;

    private void Start()
    {
        StartCoroutine(StartMove());
    }
    private void Update()
    {
        linkBoidState();
        UpdateAnim();

    }

    IEnumerator StartMove()
    {
        yield return new WaitForSeconds(Random.value);
        animState = AnimState.Move;
    }
    void linkBoidState()
    {
        if (animState == AnimState.Idle)
            return;

        if ((int)enemy.enemyState == 0)
            animState = AnimState.Move;

        if ((int)enemy.enemyState == 1)
            animState = AnimState.Attack;

        if ((int)enemy.enemyState == 2)
            animState = AnimState.Die;
    }

    void UpdateAnim()
    {
        enemyAnim.SetInteger("State", (int)animState);
    }

}
