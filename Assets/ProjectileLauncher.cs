using TMPro;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public float initialVelocity; 
    public float startingHeight;  
    public float targetDistance = 191.0f; 

    public TMP_InputField xInput;
    public TMP_InputField yInput;

    public float xSide = 3.0f; 
    public float ySide = 4.0f; 

    public float xAngleFinal;
    public float yAngleFinal;

    public TMP_InputField yawInput;
    public TMP_InputField pitchInput;



    public void DebugLogger()
    {
        
        (float angle, float hypotenuse) = CalculateRightTriangle(xSide, ySide);
        Debug.Log("Angle opposite to the y side: " + angle.ToString("F1") + " degrees");
        Debug.Log("Length of the hypotenuse: " + hypotenuse.ToString("F2"));
        yAngleFinal = angle;
        yawInput.text = yAngleFinal.ToString();


        float _angle = CalculateLaunchAngle(hypotenuse);
        xAngleFinal = _angle; 
        pitchInput.text = xAngleFinal.ToString();
        Debug.Log("Launch angle to hit target distance: " + _angle + " degrees");
    }

    float CalculateLaunchAngle(float targetDistance)
    {
        xSide = int.Parse(xInput.text);
        ySide = int.Parse(yInput.text);

        float g = -9.8f;
        float tolerance = 0.01f; 
        float minAngle = 0.0f;
        float maxAngle = 90.0f;

        while (maxAngle - minAngle > tolerance)
        {
            float angle = (minAngle + maxAngle) / 2.0f;
            float radianAngle = angle * Mathf.Deg2Rad;
            float distance = CalculateDistance(radianAngle);


            if (distance < targetDistance)
            {
                minAngle = angle;
            }
            else
            {
                maxAngle = angle;
            }
        }

        float finalAngle = (minAngle + maxAngle) / 2.0f;
        return Mathf.Round(finalAngle * 10.0f) / 10.0f;
    }

    float CalculateDistance(float radianAngle)
    {
        float cosAngle = Mathf.Cos(radianAngle);
        float sinAngle = Mathf.Sin(radianAngle);

        // Calculate time of flight using quadratic formula for the vertical motion
        float a = 0.5f * -9.8f;
        float b = initialVelocity * sinAngle;
        float c = startingHeight;

        float discriminant = b * b - 4.0f * a * c;
        if (discriminant < 0)
        {
            return 0; 
        }

        float time1 = (-b + Mathf.Sqrt(discriminant)) / (2.0f * a);
        float time2 = (-b - Mathf.Sqrt(discriminant)) / (2.0f * a);
        float timeOfFlight = Mathf.Max(time1, time2);

        
        float distance = initialVelocity * cosAngle * timeOfFlight;
        return distance;
    }


    (float, float) CalculateRightTriangle(float xSide, float ySide)
    {
        
        float hypotenuse = Mathf.Sqrt(xSide * xSide + ySide * ySide);

        
        float angleRadians = Mathf.Atan2(ySide, xSide);
        float angleDegrees = angleRadians * Mathf.Rad2Deg;

        
        angleDegrees = Mathf.Round(angleDegrees * 10.0f) / 10.0f;

        return (angleDegrees, hypotenuse);
    }
}   

