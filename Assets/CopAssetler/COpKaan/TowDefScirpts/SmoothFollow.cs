using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Camera))]
public class SmoothFollow : MonoBehaviour
{
    public Transform followTarget;
    public bool autoFindPlayer;
    public float distance = 5.0f;
    public float height = 5.0f;
    public float characterHeight = 1.5f;
    [Range(0f, 0.5f)] public float rotationLerpSpeed = 0.1f;
    [Range(0f, 0.5f)] public float heightLerpSpeed = 0.1f;
    [HideInInspector] public bool protectFromObstacles;
    [HideInInspector] public float protectionRange = 3f;
    [HideInInspector] public float castRadius = 0.2f;

    Camera cam;
    Vector3 protectionOffset;

    Vector3 LookPos
    {
        get
        {
            return followTarget.position + Vector3.up * characterHeight;
        }
    }

    void Start()
    {
        cam = GetComponent<Camera>();

        if (autoFindPlayer)
        {
            followTarget = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        if (followTarget)
        {
            // Initialize yourself at target transform 
            Vector3 targetPos = followTarget.position - followTarget.eulerAngles.y * Vector3.forward * distance;
            targetPos.y = followTarget.position.y + height;

            transform.position = targetPos;
            transform.LookAt(followTarget);
        }
    }

    void LateUpdate()
    {
        if (!followTarget)
        {
            return;
        }

        float currentHeight = transform.position.y;
        float currentRotationAngle = transform.eulerAngles.y;

        float wantedHeight = followTarget.position.y + height;
        float wantedRotationAngle = followTarget.eulerAngles.y;

        // Damp the rotation around the y-axis
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationLerpSpeed * Time.deltaTime);
        // Damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightLerpSpeed * Time.deltaTime);

        // Convert the angle into a rotation
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        Vector3 targetPos = followTarget.position - currentRotation * Vector3.forward * distance;
        // Set the height of the camera
        targetPos.y = currentHeight;

        if (protectFromObstacles)
        {
            // Protect camera position from obstacles and return a better position 
            targetPos = ProtectFromObstacles(targetPos, LookPos, castRadius, protectionRange);
        }

        transform.position = targetPos;

        // Always look at the target
        transform.LookAt(LookPos);
    }

    Vector3 ProtectFromObstacles(Vector3 targetPos, Vector3 lookAtPos, float radius, float range)
    {
        Ray ray = new Ray(targetPos, lookAtPos - targetPos);
        // Spherecast from camera to target 
        RaycastHit[] hits = Physics.SphereCastAll(ray, radius, range);

        if (hits.Length > 0)
        {
            // Take the hit that is closest to the player
            RaycastHit hit = hits
                .OrderBy(h => Vector3.Distance(h.collider.ClosestPoint(lookAtPos), lookAtPos))
                .ToList()[0];

            Collider col = hit.collider;
            Renderer renderer = col.GetComponent<Renderer>();

            ray = new Ray(lookAtPos, targetPos - lookAtPos);
            // Spherecast from target to camera 
            Physics.SphereCast(ray, radius, out hit);

            targetPos = hit.point;
        }

        // If there wasn't any hits, return the target 
        return targetPos;
    }

    Vector3 ProtectFromObstacles(Vector3 original, float range)
    {
        // Obsolete 

        range = Mathf.Clamp(range, 0.1f, Mathf.Infinity);

        // Get all colliders within range 
        Collider[] cols = Physics.OverlapSphere(transform.position, range);

        if (cols.Length > 0)
        {
            List<Vector3> obstacleVectors = new List<Vector3>();
            float maxDamp = 10f;

            foreach (Collider col in cols)
            {
                // Closest point on collider 
                Vector3 closestPoint = col.ClosestPoint(transform.position);
                // Move vector to stay away from this collider 
                Vector3 moveVector = transform.position - closestPoint;
                // The exact point camera stops colliding the collider 
                Vector3 safePosition = transform.position + moveVector.normalized * range;
                // Damp power is increased the more you get closer to the collider  
                float dampPower = maxDamp * (1 - Mathf.Sqrt(moveVector.magnitude / range));
                Vector3 smoothPosition = Vector3.Lerp(original, safePosition, dampPower * Time.deltaTime);

#if UNITY_EDITOR
                // Visualize in editor 
                Debug.DrawLine(closestPoint, transform.position, Color.yellow);
#endif
                obstacleVectors.Add(smoothPosition);
            }

            // Calculate the average position using all smoothed positions
            Vector3 average = Vector3.zero;
            obstacleVectors.ForEach(v => average += v);
            average /= obstacleVectors.Count;

            return average;
        }
        else
        {
            return original;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, protectionRange);
    }
}
