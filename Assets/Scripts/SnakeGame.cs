using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class SnakeGame : MonoBehaviour
{
    public int size = 5;
    public float speed = 5f;

    [Range(0.1f, 1f)]
    public float diagonalSpeedMultiplier = 0.8f;

    public float segmentDistance = 0.5f;
    public float trailDuration = 3f;

    public GameObject snakePrefab;

    public float followerSpeed = 3f;
    public float followerStartOffset = -5f;
    private Transform followerCube;
    private float followerDistanceBehindHead;

    private List<Transform> segments = new List<Transform>();
    private List<Vector3> positionHistory = new List<Vector3>();
    private Vector2 direction = Vector2.right;

    private void Awake()
    {
        followerDistanceBehindHead = Mathf.Abs(followerStartOffset);

        GameObject snakeHead = Instantiate(snakePrefab, Vector3.zero, Quaternion.identity);
        segments.Add(snakeHead.transform);
        positionHistory.Add(snakeHead.transform.position);

        for (int i = 1; i < size; i++)
        {
            GameObject bodyPart = Instantiate(snakePrefab, Vector3.zero, Quaternion.identity);
            segments.Add(bodyPart.transform);
        }

        GameObject cubeObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cubeObj.name = "PathFollowerCube";
        cubeObj.transform.localScale = Vector3.one * 0.4f;
        followerCube = cubeObj.transform;
    }

    void Update()
    {
        Vector2 inputDir = Vector2.zero;

        if (Keyboard.current.wKey.isPressed && direction.y != -1) inputDir.y = 1;
        if (Keyboard.current.sKey.isPressed && direction.y != 1) inputDir.y = -1;
        if (Keyboard.current.dKey.isPressed && direction.x != -1) inputDir.x = 1;
        if (Keyboard.current.aKey.isPressed && direction.x != 1) inputDir.x = -1;

        if (inputDir != Vector2.zero)
        {
            direction = inputDir.normalized;
        }

        Vector3 prevHeadPos = segments[0].position;
        segments[0].position += (Vector3)direction * speed * Time.deltaTime;

        float headMoveStep = Vector3.Distance(prevHeadPos, segments[0].position);

        positionHistory.Insert(0, segments[0].position);
        Debug.DrawLine(prevHeadPos, segments[0].position, Color.white, trailDuration);

        for (int i = 1; i < segments.Count; i++)
        {
            float targetDistance = i * segmentDistance;
            float currentPathDistance = 0f;
            bool positionSet = false;

            for (int j = 0; j < positionHistory.Count - 1; j++)
            {
                float dist = Vector3.Distance(positionHistory[j], positionHistory[j + 1]);

                if (currentPathDistance + dist >= targetDistance)
                {
                    Vector3 A = positionHistory[j];
                    Vector3 B = positionHistory[j + 1];

                    float remainingDist = targetDistance - currentPathDistance;
                    float t = remainingDist / dist;

                    segments[i].position = A + (B - A) * t;

                    positionSet = true;
                    break;
                }
                currentPathDistance += dist;
            }

            if (!positionSet && positionHistory.Count > 0)
            {
                segments[i].position = positionHistory[positionHistory.Count - 1];
            }
        }

        if ( positionHistory.Count >= 2)
        {
            followerDistanceBehindHead += headMoveStep;
            followerDistanceBehindHead -= followerSpeed * Time.deltaTime;

            if (followerDistanceBehindHead < 0) followerDistanceBehindHead = 0;

            float currentCubePathDist = 0f;
            bool cubeSet = false;

            for (int j = 0; j < positionHistory.Count - 1; j++)
            {
                float dist = Vector3.Distance(positionHistory[j], positionHistory[j + 1]);

                if (currentCubePathDist + dist >= followerDistanceBehindHead)
                {
                    Vector3 A = positionHistory[j];
                    Vector3 B = positionHistory[j + 1];

                    float remainingDist = followerDistanceBehindHead - currentCubePathDist;
                    float t = remainingDist / dist;

                    followerCube.position = A + (B - A) * t;
                    cubeSet = true;
                    break;
                }
                currentCubePathDist += dist;
            }

            if (!cubeSet)
            {
                followerCube.position = positionHistory[positionHistory.Count - 1];
            }
        }

        float maxRequiredDistance = (size - 1) * segmentDistance;
        if (followerCube != null)
        {
            maxRequiredDistance = Mathf.Max(maxRequiredDistance, followerDistanceBehindHead);
        }

        float totalDist = 0f;

        for (int j = 0; j < positionHistory.Count - 1; j++)
        {
            totalDist += Vector3.Distance(positionHistory[j], positionHistory[j + 1]);

            if (totalDist > maxRequiredDistance)
            {
                int removeIndex = j + 2; 
                if (removeIndex < positionHistory.Count)
                {
                    positionHistory.RemoveRange(removeIndex, positionHistory.Count - removeIndex);
                }
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Vector3 pos in positionHistory)
        {
            Gizmos.DrawSphere(pos, 0.05f);
        }
    }
}