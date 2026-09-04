using UnityEngine;

public static class Circle
{
   /* public static void DrawCircle(Vector3 position, float r, Color color, int segment)                                         
    {

        float angleStep = 360f / segment;

        Vector3 currentOffset = Vector3.right * r;
        Vector3 previousPoint = position + currentOffset;

        Quaternion rotation = Quaternion.Euler(0, 0, angleStep);

        for (int i = 0; i < segment; i++)
        {
            currentOffset = rotation * currentOffset;

            Vector3 nextPoint = position + currentOffset;

            Debug.DrawLine(previousPoint, nextPoint, color);

            previousPoint = nextPoint;
        }
    }


    public static void DrawCircle2(Vector3 position, float r, Color color, int segment)
    {

        float angleStep = (Mathf.PI * 2f) / segment;
    
        for (int i = 0; i < segment; i++)
        {
            float currentAngle = i * angleStep;

            float nextAngle = (i + 1) * angleStep;

            float currentX = Mathf.Cos(currentAngle) * r;
            float currentY = Mathf.Sin(currentAngle) * r;
            Vector3 currentPoint = position + new Vector3(currentX, currentY, 0f);

            float nextX = Mathf.Cos(nextAngle) * r;
            float nextY = Mathf.Sin(nextAngle) * r;
            Vector3 nextPoint = position + new Vector3(nextX, nextY, 0f);

            Debug.DrawLine(currentPoint, nextPoint, color);
        }
    }*/
}

