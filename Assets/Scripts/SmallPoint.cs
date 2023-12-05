using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallPoint : MonoBehaviour
{
    [SerializeField] float scaleFactor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(Animate());
    }

    public void Init()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.one * 1.5f;
        StartCoroutine(Animate());
    }

    public void Hide()
    {
        transform.DOScale(Vector2.zero, 0.25f);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            Hide();
        }
    }


    IEnumerator Animate()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = transform.localScale * scaleFactor;
        float t = 0;
        float duration = 0.2f;

        while(t < 1)
        {
            t += Time.deltaTime / duration;

            transform.localScale = Vector3.Lerp(startScale, endScale, t);

            yield return null;
        }

        transform.localScale = endScale;
        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / duration;

            transform.localScale = Vector3.Lerp(endScale, startScale, t);

            yield return null;
        }
        transform.localScale = startScale;


    }
}
