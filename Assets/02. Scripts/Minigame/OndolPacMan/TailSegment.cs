using System.Collections;
using UnityEngine;

public class TailSegment : MonoBehaviour
{
    public TailSegment nextSegment;
    public TailManager tailManager;

    private Collider col;
    private Animator anim;

    private void Awake()
    {
        col = GetComponent<Collider>();
        anim = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("pacPlayerHead") || other.CompareTag("pacEnemy"))
        if (other.CompareTag("pacEnemy"))
        {
            if (col != null)
                col.enabled = false;

            if (anim != null)
            {
                anim.SetBool("isWalking", true);
                anim.SetTrigger("Die");
            }

            tailManager.HandleTailCollision(this, other.gameObject);

            StartCoroutine(DelayedDestroy());
        }
    }

    public void DeleteSegment()
    {
        if (nextSegment != null)
            nextSegment.DeleteSegment();

        Destroy(gameObject);
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
