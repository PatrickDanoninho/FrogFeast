using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class FrogBehaviour : MonoBehaviour
{
    #region States
    FrogBaseState currentState;
    public IdelFrogState idleFrogState = new IdelFrogState();
    public TongueFrogState tongueFrogState = new TongueFrogState();
    public DeadFrogState deadFrogState = new DeadFrogState();
    #endregion

    public LayerMask waterLayer;

    [Header("Frog")]
    [SerializeField] GameObject frogPrefab;
    [SerializeField] float maxDistance = 10f;
    [SerializeField] GameObject tongueTip;
    public bool isDead = false;

    [Header("Tongue")]
    public Vector3 currentValidTarget;
    public LineRenderer lineRenderer;
    public GameObject startPosition;
    public bool extended = false;
    //public GameObject tongueCollider;
    public float tongueSpeed;

    [Header("Tongue Animation")]
    [SerializeField] int segments = 8;
    [SerializeField] float waveAmplitude = 0.25f;
    [SerializeField] float waveFrequency = 20f;
    [SerializeField] float waveSpeed = 30f;
    [SerializeField] float settleSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        currentState = idleFrogState;
        currentState.EnterState(this);

        lineRenderer.SetPosition(0, startPosition.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, tongueTip.transform.position);
        currentState.UpdateState(this);
    }

    public void SwitchState(FrogBaseState frogState)
    {
        currentState = frogState;
        currentState.EnterState(this);
    }

    //Testing
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(frogPrefab.transform.position, new Vector3(20, 2, 14));
    }

    public void GetValidTap()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began)
            return;

        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, Mathf.Infinity, waterLayer))
            return;

        Vector3 frogPos = startPosition.transform.position;
        frogPos.y = 0;
        Vector3 hitPoint = hit.point;
        hitPoint.y = 0;

        Vector3 direction = hitPoint - frogPos;
        float distance = direction.magnitude;

        if (distance > maxDistance)
            direction = direction.normalized * maxDistance;

        Vector3 targetPos = frogPos + direction;
        targetPos.y = 0.5f;

        currentValidTarget = targetPos;
        SwitchState(tongueFrogState);
    }

    public void ExtendTongue()
    {
        //Rotate frog
        frogPrefab.transform.LookAt(currentValidTarget);
        //lineRenderer.positionCount = 8;
        lineRenderer.SetPosition(0, startPosition.transform.position);

        //Stretch tongue
        //Move tip to tipPosition
        StartCoroutine(MoveAndAnimateTongue(startPosition.transform.position, currentValidTarget, true));
    }

    public void RetractTongue()
    {
        //Return tongue    
        //if (extended)
        //tongueCollider.gameObject.SetActive(false);

        StartCoroutine(MoveAndAnimateTongue(currentValidTarget, startPosition.transform.position, false));
    }

    public IEnumerator MoveAndAnimateTongue(Vector3 start, Vector3 targetPos, bool tongueRetracted)
    {
        float duration = 0.15f;
        float elapsed = 0f;

        tongueTip.transform.LookAt(targetPos);

        Vector3 startPos = start;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            tongueTip.transform.position = Vector3.Lerp(start, targetPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        tongueTip.transform.position = targetPos;

        //Activate tongue collider
        //UpdateTongueCollider(startPosition.transform.position, targetPos, tongueRetracted);

        yield return new WaitForSeconds(tongueSpeed);

        if (isDead)
            yield break;

        //Retract tongue
        extended = tongueRetracted;
        if (extended)
        {
            //lineRenderer.positionCount = 2;
            SwitchState(idleFrogState);
        }
    }

    //public void UpdateTongueCollider(Vector3 start, Vector3 targetPos, bool isExtended)
    //{
    //    if (!isExtended)
    //    {
    //        return;
    //    }

    //    tongueCollider.gameObject.SetActive(true);

    //    Vector3 dir = targetPos - start;
    //    float length = dir.magnitude;

    //    //Move collider to the middle of the tongue
    //    tongueCollider.transform.position = start + dir * 0.5f;

    //    //Rotate the collider to match the tongue length
    //    tongueCollider.transform.rotation = Quaternion.LookRotation(dir);

    //    //Scale it
    //    tongueCollider.transform.localScale = new Vector3(0.2f, 0.2f, length);
    //}

    //public void UpdateTongueVisuals()
    //{
    //    Vector3 start = startPosition.transform.position;
    //    Vector3 end = tongueTip.transform.position;

    //    Vector3 dir = (end - start);
    //    float length = dir.magnitude;
    //    Vector3 forward = dir.normalized;

    //    Vector3 right = Vector3.Cross(forward, Vector3.up);

    //    for (int i = 0; i < segments; i++)
    //    {
    //        float t = i / (float)(segments-1);
    //        Vector3 point = Vector3.Lerp(start, end, t);

    //        float fade = 1f - t;

    //        float wave = Mathf.Sin(Time.time * waveSpeed + t * waveFrequency) * waveAmplitude * fade;

    //        point += right * wave;

    //        lineRenderer.SetPosition(i, point);
    //    }
    //}
}
