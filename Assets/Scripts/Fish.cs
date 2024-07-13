using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public Vector2 limitX;
    public Vector2 limitY;
    public Vector2 limitZ;

    public float speed;

    [Space]
    [Space]
    public Vector3 targetPosition;
    public float time;
    public float restTime;

    Vector3 GetTargetPosition() {

        Vector3 result = new Vector3();

        result.x = Random.Range(limitX.x, limitX.y);
        result.y = Random.Range(limitY.x, limitY.y);
        result.z = Random.Range(limitZ.x, limitZ.y);
        restTime = Random.Range(2, 7);
        return result;
    }

    private void Start()
    {
        targetPosition = GetTargetPosition();
    }


    private void Update()
    {
        if(Vector3.Distance(transform.position, targetPosition) > 0.03f)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            transform.LookAt(targetPosition);
            time = 0;
        }
        else
        {
            time += Time.deltaTime;
            if(time > restTime)
            {
                targetPosition = GetTargetPosition();
            }
        }

    }
}
