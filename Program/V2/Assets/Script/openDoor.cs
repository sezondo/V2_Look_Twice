using UnityEngine;
using System.Collections;

public class openDoor : MonoBehaviour
{

    public Animator animatorR;
    public Animator animatorL;
    public AudioSource audioSource;

    private bool aniContor;
    private int layerMask;
    RaycastHit hit;
    public float range;
    public bool LRContor;
    private Coroutine closeCoroutine;
    public AudioClip openClip;


    void Start()
    {
        aniContor = false;
        layerMask = LayerMask.GetMask("Player");
        range = 10;
        LRContor = false;
    }

    void Update()
    {
        Vector3 dir = transform.forward.normalized;

        Debug.DrawRay(transform.position + Vector3.up * 1f, dir * range, Color.red, 0.1f);
        Debug.DrawRay(transform.position + Vector3.forward * 1f, dir * range, Color.red, 0.1f);
        Debug.DrawRay(transform.position + Vector3.forward * -1f, dir * range, Color.red, 0.1f);

        if (Physics.Raycast(transform.position + Vector3.up * 1f, dir, out hit, range, layerMask) ||
        Physics.Raycast(transform.position + Vector3.forward * 1f, dir, out hit, range, layerMask) ||
        Physics.Raycast(transform.position + Vector3.forward * -1f, dir, out hit, range, layerMask))
        {
            if (!aniContor) SoundManager.instance.PlaySFXUI(openClip);

            aniContor = true;

            if (closeCoroutine != null)
            {
                StopCoroutine(closeCoroutine);
                closeCoroutine = null;
            }
        }
        else
        {
           if (aniContor && closeCoroutine == null)
            {
                closeCoroutine = StartCoroutine(CloseDoorAfterDelay(6f));
            }
        }

        animatorR.SetBool("OPCR", aniContor);
        animatorL.SetBool("OPCR", aniContor);
    }

    IEnumerator CloseDoorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        aniContor = false;
        SoundManager.instance.PlaySFXUI(openClip); 
        closeCoroutine = null; // 끝난 후 초기화
    }
}
