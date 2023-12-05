using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float maxDistance;
    [SerializeField] Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        float sin = Mathf.Sin(Time.time * speed) * maxDistance;
        transform.position = direction.normalized * sin;
        transform.localScale = Vector2.one * sin;
    }
}
