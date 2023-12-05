using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Draw : MonoBehaviour
{
    [SerializeField] LineRenderer circleLine;
    [SerializeField] LineRenderer lineX; // red Line 
    [SerializeField] LineRenderer lineY; // green line
    [SerializeField] LineRenderer lineXPosition; 
    [SerializeField] LineRenderer lineYPosition; 
    [SerializeField] LineRenderer angleLine; // blue line
    [SerializeField] LineRenderer angleLines;
    [SerializeField] LineRenderer pointAngle;
    [SerializeField] LineRenderer sinWave;
    [SerializeField] LineRenderer radiusLine;
    [SerializeField] LineRenderer gridLine;

    [SerializeField] LineRenderer firstAngleLine;
    [SerializeField] LineRenderer SecondAngleLine;

    [SerializeField] GameObject angleText;
    [SerializeField] float lineWidth;
    [SerializeField] SmallPoint circle;
    [SerializeField] int steps;
    [SerializeField] float radius;

    [SerializeField] List<TextMeshProUGUI> angleTxt;
    [SerializeField] List<TextMeshProUGUI> RadTxt;
    [SerializeField] TextMeshProUGUI radiusTxt;



    public int count_2;
    public float radius_2;


    bool once;
    int divideLineResets;
    // Start is called before the first frame update
    void Start()
    {
        DrawCircle();

        SetLineWidth(lineX);
        SetLineWidth(lineY);
        SetLineWidth(angleLine);
        SetLineWidth(lineYPosition);
        SetLineWidth(lineXPosition);
        SetLineWidth(radiusLine);
    }

    void SetLineWidth(LineRenderer line)
    {
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
    }

    // Update is called once per frame
    void Update()
    {
        if (once == false && Input.GetKeyDown(KeyCode.S)) // Draw the First Circle
        {
            StartCoroutine(InitialAnimation());
            once = true;
        }

        DrawCircle();

        if (Input.GetKeyDown(KeyCode.X)) // Draw The Red and Green Center Line
        {
            DrawLine(lineY, 90, 270);
            DrawLine(lineX, 180, 0);
        }

      

        if (Input.GetKeyDown(KeyCode.Space)) // Positionate  The Circle at the angle you want
        {
            PositionateCircle();
        }

        if (Input.GetKeyDown(KeyCode.A)) // Draw The blue angle line and circle of the point
        {
            DrawAngleLine(45f);
            StartCoroutine(DrawAngleCircle());
        }

        if (Input.GetKeyDown(KeyCode.K)) // divide the circle to ten pieces
        {
            StartCoroutine(DivideCircle(10));
        }

        if(Input.GetKeyDown(KeyCode.Y)) // Draw a green line from the y position of the point to the center
        {
            DrawPositionLines(lineYPosition,new Vector2(0,GetPosition(45f).y),45f);
        }

        if (Input.GetKeyDown(KeyCode.C)) //Draw a red line from the x position of the point to the center
        {
            DrawPositionLines(lineXPosition, new Vector2(GetPosition(45f).x, 0),45f);
        }

        if (Input.GetKeyDown(KeyCode.R)) // Reset lines and point and texts
        {
            ResetLines();
        }

        if (Input.GetKeyDown(KeyCode.P)) // Reset divide lines and 
        {
            if(divideLineResets == 0)
            {
                ResetDivideLines();
            }
            else
            {
                StartCoroutine(ResetDivideLinesCorou()); // Reset divide lines and big Circle
            }
            divideLineResets++;
        }


        if (Input.GetKeyDown(KeyCode.J)) // switch Deg
        {
            SwitchToRad();
        }

        //------------------------  Second Phase -----------------------

        if (Input.GetKeyDown(KeyCode.N)) // rotate a line 360° and back to 0
        {
            StartCoroutine(RotateLine());
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
           StartCoroutine(DrawFirstAngleLine());
        }

        if(Input.GetKeyDown(KeyCode.U))
        {
            SpawnAngleDegTxt();
        }


        if (Input.GetKeyDown(KeyCode.O)) // spawn points
        {
            StartCoroutine(SpawnCircles(radius_2, count_2));
        }
    }
    #region FirstPhase
    IEnumerator InitialAnimation()
    {
        // increase The Radius
        float r = 3f;
        float t = 0;
        float duration = 0.3f;

        while (t < 1)
        {
            t += Time.deltaTime / duration;

            radius = Mathf.Lerp(0, r, t);
            yield return null;
        }
        t = 0;

        // radius fully increased

        yield return new WaitForSeconds(0.3f);

        //Animate a line goes from center to the edge of circle

        while (t < 1)
        {
            t += Time.deltaTime / duration;
            float x = Mathf.Lerp(0, radius, t);
            radiusLine.SetPosition(1, new Vector2(x, 0));
            yield return null;
        }

        ShowRadiusText(); 

        // end radius line animation

        yield return new WaitForSeconds(0.3f);
        radius = r;
        SpawnAngleDegTxt(); // Show 0, 90 , 180 ,270 close to the circle
        t = 0;
        yield return new WaitForSeconds(1f);

        // Animate the radius from the edge of the circle to the centre, until it Disappears
        while (t < 1)
        {
            t += Time.deltaTime / duration;

            Vector2 p = Vector2.Lerp(Vector2.zero, radiusLine.GetPosition(1), t);
            radiusLine.SetPosition(0, p);
            if (t > 0.5f)
            {
                HideRadiusTxt();
            }
            yield return null;
        }
        DrawLine(lineY, 90, 270);
        DrawLine(lineX, 180, 0);

    }

    void ShowRadiusText()
    {
        radiusTxt.transform.DOScale(Vector2.one, 0.5f).SetEase(Ease.OutBounce);
    }

    void HideRadTxt() // hide 0, Pi/2, Pi, 3Pi/2 Texts
    {
        foreach (TextMeshProUGUI t in RadTxt)
        {
            t.transform.DOScale(Vector2.zero, 0.25f);
        }
    }

    void HideRadiusTxt() 
    {
        radiusTxt.transform.DOScale(Vector2.zero, 0.5f).SetEase(Ease.OutBounce);
    }
    void ResetLines() // Reset all the line of the initial animations and the Radians values
    {
        Vector2 linePosX = lineX.GetPosition(1);
        StartCoroutine(LineAnimation(lineX, linePosX, lineX.GetPosition(0), 1));

        Vector2 linePosY = lineY.GetPosition(1);
        StartCoroutine(LineAnimation(lineY, linePosY, lineY.GetPosition(0), 1));

        Vector2 angleLinePos = angleLine.GetPosition(1);
        StartCoroutine(LineAnimation(angleLine, angleLinePos, angleLine.GetPosition(0), 1));

        Vector2 conLine = lineXPosition.GetPosition(1);
        StartCoroutine(LineAnimation(lineXPosition, conLine, lineXPosition.GetPosition(0), 1));

        Vector2 sinLine = lineYPosition.GetPosition(1);
        StartCoroutine(LineAnimation(lineYPosition, sinLine, lineYPosition.GetPosition(0), 1));


        StartCoroutine(ResetSmallPoint());

        StartCoroutine(ResetAngleCircle());

        angleText.transform.DOScale(Vector2.zero, 0.25f);

        HideRadTxt();

    }

    void SwitchToRad() // Switch texts from degrees values to radians values
    {
        foreach (TextMeshProUGUI t in angleTxt)
        {
            t.transform.DOScale(Vector2.zero, 0.25f).OnComplete(() =>
            {
                foreach (TextMeshProUGUI r in RadTxt)
                {
                    r.transform.DOScale(Vector2.one, 0.25f);
                }
            });
        }
    }

    IEnumerator ResetSmallPoint() // make the small circle hide
    {

        // do a little InBounce animation 
        Vector2 startScale = circle.transform.localScale;

        Vector2 endScale = circle.transform.localScale * 1.5f;
        float t = 0;
        float duration = 0.25f;

        while (t < 1)
        {
            t += Time.deltaTime / duration;

            circle.transform.localScale = Vector2.Lerp(startScale, endScale, t);

            yield return null;
        }

        // end InBounce Animation 

        // start Hide Animation
        t = 0;
        startScale = endScale;
        endScale = Vector2.zero;

        while (t < 1)
        {
            t += Time.deltaTime / duration;

            circle.transform.localScale = Vector2.Lerp(startScale, endScale, t);

            yield return null;
        }

        circle.transform.localScale = Vector2.zero;
        circle.gameObject.SetActive(false);
    }

    void DrawCircle() // Draw The Big Circle based on radius
    {
        circleLine.positionCount = steps;

        float angleSteps = (Mathf.PI * 2) / steps;
        float angle = angleSteps;

        for (int i = 0; i < steps; i++)
        {
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);

            Vector2 pos = new Vector2(x, y) * radius;

            circleLine.SetPosition(i, pos);

            angle += angleSteps;
        }
    }

    IEnumerator DrawAngleCircle()
    {

        yield return new WaitForSeconds(0.5f);
        pointAngle.positionCount = steps;

        float angleSteps = (Mathf.PI * 0.25f) / steps;
        float angle = angleSteps;

        for (int i = 0; i < steps; i++)
        {
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);

            Vector2 pos = new Vector2(x, y) * 0.8f;

            pointAngle.SetPosition(i, pos);

            angle += angleSteps;

            yield return null;
        }

        angleText.transform.DOScale(Vector2.one, 0.25f);
    }

    IEnumerator ResetAngleCircle()
    {

        for (int i = steps; i > 0; i--)
        {
            pointAngle.positionCount--;

            yield return new WaitForSeconds(0.001f);
        }
    }

    Vector2 GetPosition(float angle)
    {

        float x = Mathf.Cos(angle * Mathf.Deg2Rad);
        float y = Mathf.Sin(angle * Mathf.Deg2Rad);

        Vector2 pos = new Vector2(x, y) * radius;

        return pos;
    }

    void PositionateCircle()
    {
        Vector2 pos = GetPosition(45f);

        circle.transform.position = pos;

        circle.gameObject.SetActive(true);

    }

    void DrawLine(LineRenderer line, float angle_1, float angle_2)
    {
        Vector2 pos_1 = GetPosition(angle_1);
        Vector2 pos_2 = GetPosition(angle_2);

        line.SetPosition(0, pos_1);
        StartCoroutine(LineAnimation(line, pos_1, pos_2, 1));
    }

    void DrawPositionLines(LineRenderer line, Vector2 endPos, float angle)
    {
        Vector2 pos = GetPosition(angle);

        line.SetPosition(0, pos);

        StartCoroutine(LineAnimation(line, pos, endPos, 1));
    }

    IEnumerator LineAnimation(LineRenderer line, Vector2 pos_1, Vector2 pos_2, int index, float dur = 0.5f)
    {
        float t = 0;
        float duration = dur;

        while (t < 1)
        {
            t += Time.deltaTime / duration;
            Vector2 lerpedPos = Vector2.Lerp(pos_1, pos_2, t);
            line.SetPosition(index, lerpedPos);
            yield return null;
        }

        line.SetPosition(index, pos_2);
    }
    private void ResetDivideLines()
    {
        float angleSteps = 360 / 10;
        float angle = angleSteps;
        for (int i = 0; i < angleLines.positionCount; i += 2)
        {
            Vector2 pos = angleLines.GetPosition(i + 1);

            StartCoroutine(LineAnimation(angleLines, pos, angleLines.GetPosition(i), i + 1, 0.2f));

            angle += angleSteps;
        }
    }
    IEnumerator ResetDivideLinesCorou()
    {
        float angleSteps = 360 / 10;
        float angle = angleSteps;
        for (int i = 0; i < angleLines.positionCount; i += 2)
        {
            Vector2 pos = angleLines.GetPosition(i + 1);

            StartCoroutine(LineAnimation(angleLines, pos, angleLines.GetPosition(i), i + 1, 0.2f));

            angle += angleSteps;
        }

        yield return new WaitForSeconds(0.5f);
        HideAngleText();
        StartCoroutine(ResetBigCircle());


    }

    IEnumerator ResetBigCircle()
    {
        float t = 0;
        float duration = 0.5f;
        float r = radius;
        while (t < 1)
        {
            t += Time.deltaTime / duration;
            radius = Mathf.Lerp(r, 0, t);
            yield return null;
        }
    }

    void DrawAngleLine(float angle)
    {
        Vector2 pos = GetPosition(angle);

        angleLine.SetPosition(0, Vector2.zero);

        StartCoroutine(LineAnimation(angleLine, Vector2.zero, pos, 1));
    }

    int index;
    IEnumerator DivideCircle(int count)
    {
        float angleSteps = 360 / count;
        float angle = angleSteps;


        for (int i = 0; i < 10; i++)
        {
            angleLines.positionCount += 2;

            angleLines.SetPosition(angleLines.positionCount - 2, Vector2.zero);

            Vector2 pos = GetPosition(angle);
            if (index > 0)
            {
                SmallPoint cir = Instantiate(circle, pos, Quaternion.identity);
                cir.Init();
            }

            StartCoroutine(LineAnimation(angleLines, Vector2.zero, pos, angleLines.positionCount - 1, 0.2f));
            angle += angleSteps;

            yield return new WaitForSeconds(0.2f);
        }

        index++;
    }



    void SpawnAngleDegTxt()
    {

        foreach (TextMeshProUGUI t in angleTxt)
        {
            t.transform.DOScale(Vector2.one, 0.5f).SetEase(Ease.OutBounce);
        }
    }

    void HideAngleText()
    {
        foreach (TextMeshProUGUI t in angleTxt)
        {
            t.transform.DOScale(Vector2.zero, 0.5f).SetEase(Ease.OutBounce);
        }
    }

    #endregion
    #region SecondPhase

    IEnumerator SpawnCircles(float radius, int count)
    {
        float angleSteps = (Mathf.PI * 2) / count;
        float angle = angleSteps;

        float t = 0.1f;

        if (radius == 4) t = 0.05f;
        for (int i = 0; i < count; i++)
        {
            float x = Mathf.Cos(angle) ;
            float y = Mathf.Sin(angle) ;
            Vector2 pos = new Vector2(x,y) * radius;

            SmallPoint sp = Instantiate(circle, pos, Quaternion.identity);
            sp.Init();

            angle += angleSteps;

            yield return new WaitForSeconds(t);
        }

    }

    [SerializeField] LineRenderer rotateLine;
    [SerializeField] LineRenderer rotateAngleLine;
    IEnumerator RotateLine()
    {
        StartCoroutine(LineAnimation(rotateLine, Vector2.zero, new Vector2(3,0), 1, 0.25f));

        yield return new WaitForSeconds(0.3f);

        float angle = 0;
        float speed = 6;
        int i = 0;
        while(angle <= 2 * Mathf.PI)
        {
            angle += Time.deltaTime * speed;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            Vector2 pos = new Vector2(x, y);
            Vector2 circlePos = pos * 0.2f;
            rotateAngleLine.SetPosition(i, circlePos);
            rotateLine.SetPosition(1, pos);
            speed -= Time.deltaTime * angle;
            speed = Mathf.Clamp(speed, 1, speed);
            i++;
            rotateAngleLine.positionCount++;
            //angle = Mathf.Clamp(angle, 0, 2 * Mathf.PI);
            yield return null;
        }

        yield return new WaitForSeconds(0.01f);

         angle = 2 *Mathf.PI;
        speed = 0;
        i = 1;
        while (angle > 0)
        {
            angle -= Time.deltaTime * speed;
            angle = Mathf.Clamp(angle, 0, angle);
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            Vector2 pos = new Vector2(x, y);
            Vector2 circlePos = pos * 0.2f;
            //rotateAngleLine.SetPosition(i, circlePos);
            rotateLine.SetPosition(1, pos);
            speed += Time.deltaTime * angle * 2f;
            speed = Mathf.Clamp(speed, 1, speed);
            rotateAngleLine.positionCount -= 1;

            yield return null;
        }
        rotateAngleLine.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.01f);

        StartCoroutine(LineAnimation(rotateLine, Vector2.zero, rotateLine.GetPosition(1), 0, 0.25f));

    }


    IEnumerator DrawFirstAngleLine()
    {
        Vector2 pos = GetPosition(36);
        StartCoroutine(LineAnimation(firstAngleLine, Vector2.zero, pos,1,0.5f));

        yield return new WaitForSeconds(0.5f);

        DrawPositionLines(lineXPosition, new Vector2(GetPosition(36f).x, 0), 36f);
        DrawPositionLines(lineYPosition, new Vector2(0, GetPosition(36f).y), 36f);

        yield return new WaitForSeconds(1f);

        SmallPoint sp = Instantiate(circle, pos, Quaternion.identity);
        sp.Init();

        yield return new WaitForSeconds(1f);

        StartCoroutine(LineAnimation(firstAngleLine, Vector2.zero, firstAngleLine.GetPosition(1), 0));
        Vector2 conLine = lineXPosition.GetPosition(1);
        StartCoroutine(LineAnimation(lineXPosition, conLine, lineXPosition.GetPosition(0), 1));

        Vector2 sinLine = lineYPosition.GetPosition(1);
        StartCoroutine(LineAnimation(lineYPosition, sinLine, lineYPosition.GetPosition(0), 1));
        sp.transform.DOScale(Vector2.zero, 0.25f);
        StartCoroutine(DrawSecondAngleLine());

    }
    IEnumerator DrawSecondAngleLine()
    {
        yield return new WaitForSeconds(1f);

        Vector2 pos = GetPosition(72);
        StartCoroutine(LineAnimation(SecondAngleLine, Vector2.zero, pos, 1, 0.5f));

        yield return new WaitForSeconds(0.5f);

        DrawPositionLines(lineXPosition, new Vector2(GetPosition(72f).x, 0), 72f);
        DrawPositionLines(lineYPosition, new Vector2(0, GetPosition(72f).y), 72f);

        yield return new WaitForSeconds(1f);
        SmallPoint sp = Instantiate(circle, pos, Quaternion.identity);
        sp.Init();

        yield return new WaitForSeconds(2f);

        StartCoroutine(LineAnimation(SecondAngleLine, Vector2.zero, SecondAngleLine.GetPosition(1), 0));
        Vector2 conLine = lineXPosition.GetPosition(1);
        StartCoroutine(LineAnimation(lineXPosition, conLine, lineXPosition.GetPosition(0), 1));

        Vector2 sinLine = lineYPosition.GetPosition(1);
        StartCoroutine(LineAnimation(lineYPosition, sinLine, lineYPosition.GetPosition(0), 1));
        sp.transform.DOScale(Vector2.zero, 0.25f);

        yield return new WaitForSeconds(0.5f);

        Vector2 linePosX = lineX.GetPosition(1);
        StartCoroutine(LineAnimation(lineX, linePosX, lineX.GetPosition(0), 1));

        Vector2 linePosY = lineY.GetPosition(1);
        StartCoroutine(LineAnimation(lineY, linePosY, lineY.GetPosition(0), 1));

    }
    #endregion

    
}
