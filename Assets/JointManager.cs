using System;
using UnityEngine;

public class RotateGameObjects : MonoBehaviour
{
    // Public variables to set GameObjects and their rotations
    public GameObject gameObject1;
    public GameObject gameObject2;
    public float rotation1;
    public float rotation2;
    public SerialManager serialObj;

    public float offset1;
    public float offset2;

    private void Start()
    {
        //Debug.Log(CalculateLaunchAngle(191));
    }

    void Update()
    {
        rotation1 = serialObj.servo1Position - 90f + offset1;
        rotation2 = serialObj.servo2Position - 90f + offset2;

        
        if (gameObject1 != null)
        {
            
            gameObject1.transform.localRotation = Quaternion.Euler(0, 0, rotation1);
        }

        if (gameObject2 != null)
        {
            
            gameObject2.transform.localRotation = Quaternion.Euler(0, rotation2, 0);
        }
    }


}
