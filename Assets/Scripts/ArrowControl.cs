using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ArrowControl : MonoBehaviour
{
    public Transform bowPivot;
    public TrajectoryLine trajectoryRenderer;
    public bool shoot;
    public Vector3 handPosition;
    public float velocityDiv;
    public int trajectoryPositionID;
    public Rigidbody rb;
    public ArcheryGameManager gameManager;
    public AudioSource hitSound;
    public AudioSource resetSound;
    public BowControll bow;

    Transform firstParent;
    Vector3 firstPosition;
    Quaternion firstRotation;
    Animator targetAnimator;

    private void Start()
    {
        firstParent = transform.parent;
        firstPosition = transform.position;
        firstRotation = transform.rotation;
    }

    private void Update()
    {
        if(bowPivot)
        {
            transform.position = bowPivot.position;
            transform.rotation = bowPivot.rotation;

            trajectoryRenderer.gameObject.SetActive(true);
        }
        else if (shoot)
        {
            if(trajectoryPositionID < trajectoryRenderer.trajactoryPositions.Length)
            {
                Vector3 targetPosition = trajectoryRenderer.trajactoryPositions[trajectoryPositionID];
                rb.MovePosition(Vector3.MoveTowards(rb.position, targetPosition, Time.deltaTime * trajectoryRenderer.velocity / velocityDiv));
                if (Vector3.Distance(rb.position, targetPosition) == 0)
                {
                    trajectoryPositionID++;
                }
            }
            else
            {
                rb.position = firstPosition;
                rb.rotation = firstRotation;
                resetSound.Play();
                Debug.Log("DONE SHOOT");
                bow.ReadyToLaunch(this);
                shoot = false;
            }

            trajectoryRenderer.gameObject.SetActive(false);
        }
    }

    public void AttachToBow(Transform b)
    {
        bowPivot = b;
        trajectoryRenderer.SetVelocity(0);
    }

    public void Shoot()
    {
        Debug.Log("TEMBAK");
        bowPivot = null;
        trajectoryPositionID = 0;
        shoot = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Target")
        {
            shoot = false;

            targetAnimator = other.GetComponentInParent<Animator>();
            targetAnimator.enabled = false;
            transform.SetParent(other.transform);
            hitSound.Play();
            StartCoroutine(NextTarget(rb.position, other.transform.parent.gameObject));

        }
    }

    IEnumerator NextTarget(Vector3 pos, GameObject target)
    {
        Debug.Log("DONE SHOOT");

        gameManager.TargetShooted(pos);
        shoot = false;

        yield return new WaitForSeconds(0.4f);

        transform.SetParent(firstParent);

        yield return new WaitForSeconds(0.1f);

        rb.position = firstPosition;
        rb.rotation = firstRotation;
        targetAnimator.enabled = true;

        gameManager.NextTarget(target);

        bow.ReadyToLaunch(this);

        Debug.Log("NEXT TARGET");
    }

    public void ResetBow() {

        transform.SetParent(firstParent);
        rb.position = firstPosition;
        rb.rotation = firstRotation;
    }
}
