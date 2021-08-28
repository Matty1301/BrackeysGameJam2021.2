using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : PlayerController
{
    public GameObject PrefabFireBall;
    private Rigidbody FireBallRB;

    [SerializeField] Camera TopDownCamera;
    public float AttackSpread;
    public float AttackForce;

    protected override void Attack()
    {
        if ((Input.GetButtonDown("AttackL") || Input.GetButtonDown("AttackR")) && !alreadyAttacked)
        {
            alreadyAttacked = true;
            animator.ResetTrigger("Hit");
            animator.SetTrigger("Attack");
            Invoke("ThrowBall", timeBeforeRegisterHits);
            Invoke("ResetAttack", timeBetweenAttacks);
        }
    }

    public void ThrowBall()
    {
        Ray ray = TopDownCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Walkable")))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        targetPoint = new Vector3(targetPoint.x, attackPoint.position.y, targetPoint.z);
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        float x = Random.Range(-AttackSpread, AttackSpread);
        float z = Random.Range(-AttackSpread, AttackSpread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, 0, z);
        GameObject currentBall = Instantiate(PrefabFireBall, attackPoint.position, Quaternion.identity);
        currentBall.transform.forward = directionWithSpread.normalized;

        FireBallRB = currentBall.GetComponent<Rigidbody>();
        FireBallRB.AddForce(directionWithSpread.normalized * AttackForce, ForceMode.Impulse);
    }
}
