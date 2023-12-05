using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIAnimation : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cosTM;
    [SerializeField] TextMeshProUGUI sinTM;
    [SerializeField] TextMeshProUGUI PPoint;
    [SerializeField] TextMeshProUGUI degToRadTxt;
    [SerializeField] TextMeshProUGUI countTxt;
    [SerializeField] TextMeshProUGUI radiusTxt;
    int count = 10;
    int radius = 3;
    string cosText;
    string sinText;

    public Draw draw;
    // Start is called before the first frame update
    void Start()
    {
        cosText = cosTM.text;
        sinText = sinTM.text;

        cosTM.text = "";
        sinTM.text = "";      
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C)) // Write Cos equation
        {
            StartCoroutine(ConAnime());
            StartCoroutine(AddToPoint("(x,", 'x', "#F21155"));
        }

        if (Input.GetKeyDown(KeyCode.Y)) // Write Sin equation
        {
            StartCoroutine(SinAnime());
            StartCoroutine(AddToPoint("y)", 'y', "#2EFBC7"));

        }

        if (Input.GetKeyDown(KeyCode.R)) // Hide Texts
        {
            StartCoroutine(ResetText());
            HidePointText();
            HideDegToRagTxt();
        }

        if(Input.GetKeyDown(KeyCode.Space)) // Show P text
        {
            StartCoroutine(PPointAnime());

        }

        if (Input.GetKeyDown(KeyCode.D)) // add D2R to the equation
        {
            StartCoroutine(D2R());
        }

        if (Input.GetKeyDown(KeyCode.H)) // Show D2P value
        {
            ShowDegToRagTxt();
        }

        if (Input.GetKeyDown(KeyCode.B)) // Show the variables Count and Radius
        {
            ShowVariable();
        }

        if(Input.GetKeyDown(KeyCode.T)) // Show the new values of variables Count and Radius
        {
            ChangeVariables();
        }

        if(Input.GetKeyDown(KeyCode.I)) // Hide variables Count and Radius
        {
            HideVariable();
        }


    }
    void HidePointText()
    {
        PPoint.transform.DOScale(Vector2.zero, 0.25f);
    }

    void ShowDegToRagTxt()
    {
        degToRadTxt.transform.DOScale(Vector2.one, 0.25f);
    }
    void HideDegToRagTxt()
    {
        degToRadTxt.transform.DOScale(Vector2.zero, 0.25f);
    }
    IEnumerator D2R()
    {
        string d2r = ".D2R";
        string s ="";
        foreach(char c in d2r)
        {
            s += c;
            cosTM.text = "Cos(<color=white>θ</color><color=white>" +s+ "</color>).<color=white>r</color> = <color=white>P</color>x";
            sinTM.text = "Sin(<color=white>θ</color><color=white>"+s+ "</color>).<color=white>r</color> = <color=white>P</color>y";
            yield return new WaitForSeconds(0.03f);

        }
    }

    IEnumerator ConAnime()
    {
        foreach(char c in cosText)
        {
            if(c == 'r' || c == 'θ' || c=='P')
            {
                cosTM.text += "<color=white>"+c+"</color>";
            }
            else
            cosTM.text += c;
            yield return new WaitForSeconds(0.03f);
        }

    }

    IEnumerator SinAnime()
    {
        foreach (char c in sinText)
        {
            if (c == 'r' || c == 'θ' || c == 'P')
            {
                sinTM.text += "<color=white>" + c + "</color>";
            }
            else
                sinTM.text += c;
            yield return new WaitForSeconds(0.03f);
        }
    }
     
    IEnumerator AddToPoint(string s,char ch,string color)
    {
        string pAddition = s;
        foreach (char c in pAddition)
        {
            if (c == ch)
            {
                PPoint.text += "<color="+color+">" + c + "</color>";
            }
            else
                PPoint.text += c;
            yield return new WaitForSeconds(0.03f);
        }
    }

    IEnumerator ResetText()
    {
        float t = 0;
        float duration = 0.25f;
        Vector2 startScale = cosTM.transform.localScale;
        while(t < 1)
        {
            t += Time.deltaTime / duration;
            cosTM.transform.localScale =  Vector3.Lerp(startScale, Vector2.zero, t);
            sinTM.transform.localScale =  Vector3.Lerp(startScale, Vector2.zero, t);
            yield return null;
        }

        cosTM.gameObject.SetActive(false);
        sinTM.gameObject.SetActive(false);
    }

    IEnumerator PPointAnime()
    {
        PPoint.gameObject.SetActive(true);
        float t = 0;
        float duration = 0.25f;
        while (t < 1)
        {
            t += Time.deltaTime / duration;
            PPoint.transform.localScale = Vector3.Lerp(Vector2.zero, Vector3.one, t);
            yield return null;
        }
    }

    void ShowVariable()
    {
        countTxt.transform.DOScale(Vector2.one, 0.25f).SetEase(Ease.OutBounce);
        radiusTxt.transform.DOScale(Vector2.one, 0.25f).SetEase(Ease.OutBounce);
    }

    void HideVariable()
    {
        countTxt.transform.DOScale(Vector2.zero, 0.25f);
        radiusTxt.transform.DOScale(Vector2.zero, 0.25f);
    }

    void ChangeVariables()
    {
        countTxt.transform.DOScale(Vector2.zero, 0.25f).SetEase(Ease.OutBounce).OnComplete(() =>  CountText());
        radiusTxt.transform.DOScale(Vector2.zero, 0.25f).SetEase(Ease.OutBounce).OnComplete(() => RaduisText());
    }

    void CountText()
    {
        count += 10;
        countTxt.text = "count = " + draw.count_2.ToString();
        countTxt.transform.DOScale(Vector2.one, 0.25f).SetEase(Ease.OutBounce);
    }

    void RaduisText()
    {
        radius-- ;
        radiusTxt.text = "radius = " + draw.radius_2.ToString();
        radiusTxt.transform.DOScale(Vector2.one, 0.25f).SetEase(Ease.OutBounce);
    }
}
