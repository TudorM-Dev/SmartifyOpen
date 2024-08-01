using UnityEngine;
using System.Collections;
using System.IO.Ports; // Import the SerialPort class
using System.Threading;
using UnityEngine.UI;
using TMPro; // Import the Threading class

public class SerialManager : MonoBehaviour
{
    // Modify these variables according to your setup
    string portName = "COM19";
    int baudRate = 9600;
    SerialPort serialPort;

    // Servo control variables
    public float servo1Position = 90;
    public float servo2Position = 90;
    public float servo3Position = 90;

    public TMP_InputField pos1;
    public TMP_InputField pos2;
    public Toggle trigger;

    Thread serialThread;
    bool isRunning = true;

    public float servo1mid = 85;
    public float servo2mid = 85;
    public float servo3load = 100;
    public float servo3unload = 65;

    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open(); // Open the serial port
        serialPort.ReadTimeout = 1000; 

        serialThread = new Thread(SerialThread);
        serialThread.Start();
    }

    // Update is called once per frame
    void Update()
    {

        servo1Position = float.Parse(pos1.text) + servo1mid;
        servo2Position = float.Parse(pos2.text) + servo2mid;

        if (trigger.isOn) servo3Position = servo3unload;
        else servo3Position = servo3load;

        if (serialPort.IsOpen)
        {
            try
            {
                SendServoPositions(-float.Parse(pos1.text) + servo1mid + 3, -float.Parse(pos2.text) + servo2mid, servo3Position);

                //string receivedMessage = serialPort.ReadLine();
                //if (!string.IsNullOrEmpty(receivedMessage))
                {
                    //Debug.Log("Received from Arduino: " + receivedMessage);
                }
            }
            catch (System.Exception e)
            {
                //Debug.LogWarning("Serial port error: " + e.Message);
            }
        }
        gameObject.SetActive(false);
    }

    void SerialThread()
    {
        while (isRunning)
        {
            if (serialPort.IsOpen)
            {
                try
                {
                    string message = serialPort.ReadLine();
                    if (!string.IsNullOrEmpty(message))
                    {
                        //Debug.Log("Received from Arduino: " + message);
                    }
                }
                catch (System.Exception e)
                {
                    //Debug.LogWarning("Serial port error: " + e.Message);
                }
            }
            Thread.Sleep(10); // Add a small delay to prevent high CPU usage
        }
    }

    public void SendServoPositions(float servo1, float servo2, float servo3)
    {
        // Construct message to send to Arduino
        string message = servo1.ToString() + "," + servo2.ToString() + "," + servo3.ToString();
        serialPort.WriteLine(message);
        Debug.Log("Sent to Arduino: " + message);
    }

    void OnApplicationQuit()
    {
        isRunning = false;
        if (serialThread != null && serialThread.IsAlive)
        {
            serialThread.Join();
        }

        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
