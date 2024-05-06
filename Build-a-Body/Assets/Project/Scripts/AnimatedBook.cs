using System;
using System.Collections;
using UnityEngine;

public class AnimatedBook : MonoBehaviour
{

    public float lerpSpeed;
    public float animationSpeed;
    public float backwardsMovement;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void StartOpeningAnimation(Action animationFinishedCallback)
    {
        StartCoroutine(LerpTowardsCamera(animationFinishedCallback));
    }

    private IEnumerator LerpTowardsCamera(Action animationFinishedCallback)
    {

        yield return StartCoroutine(PickOutOfBookcase());

        anim.SetFloat("speed", animationSpeed);
        anim.Play("Book open");

        float lerp = 0;
        Transform bookTarget = Camera.main.transform.GetChild(0).transform;
        Vector3 originalPos = transform.position;
        Quaternion originalRot = transform.rotation;

        while (lerp < 1)
        {
            lerp += Time.deltaTime * lerpSpeed;
            Vector3 smoothedPosition = Vector3.Lerp(originalPos, bookTarget.position, Mathf.SmoothStep(0, 1, lerp));
            Quaternion smoothedRotation = Quaternion.Lerp(originalRot, bookTarget.rotation, Mathf.SmoothStep(0, 1, lerp));
            transform.position = smoothedPosition;
            transform.rotation = smoothedRotation;

            yield return null;
        }

        animationFinishedCallback?.Invoke();
    }

    private IEnumerator PickOutOfBookcase()
    {
        float moved = 0;
        while (moved < backwardsMovement)
        {
            moved += Time.deltaTime * (lerpSpeed * 2);
            transform.Translate(new Vector3(0, 0, -Time.deltaTime * (lerpSpeed * 2)), Space.Self);
            yield return new WaitForEndOfFrame();
        }
    }

}
