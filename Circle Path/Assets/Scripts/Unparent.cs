using System;
using System.Collections;
using UnityEngine;

public class Unparent : MonoBehaviour
{
    public Transform parent;
    public bool follow;

    public float destroyDelay;

    private bool done;

    private void Start()
    {
        parent = transform.root;
    }

    private void LateUpdate()
    {
        if (!done)
        {
            if (transform.root != null)
            {
                transform.SetParent(null, true);
            }

            if (follow && (bool)parent)
            {
                transform.position = parent.transform.position;
            }

            if (!parent)
            {
                StartCoroutine(DelayRemove());
                done = true;
            }
        }
    }

    private IEnumerator DelayRemove()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(base.gameObject);
    }
}