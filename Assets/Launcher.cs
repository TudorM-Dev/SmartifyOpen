using UnityEngine;
using UnityEngine.UI;

public class Laucher : MonoBehaviour
{
    public Transform launchPoint;
    public Transform launchPointLine;
    public GameObject projectile;
    public float launchSpeed = 1000f;
    public float launchSpeedLine = 10f;

    [Header("****Trajectory Display****")]
    public LineRenderer lineRenderer;
    public int linePoints = 175;
    public float timeIntervalInPoints = 0.01f;
    public Toggle toggle;

    void Update()
    {
        if (lineRenderer != null)
        {
            if (Input.GetMouseButton(1))
            {
                DrawTrajectory();
                lineRenderer.enabled = true;
            }
            else
                lineRenderer.enabled = false;
        }
    }

    public void shoot()
    {
        if (!toggle.isOn) return;
        var _projectile = Instantiate(projectile, launchPoint.position, launchPoint.rotation);
        _projectile.GetComponent<Rigidbody>().AddForce(launchSpeed * 10000f * launchPoint.up);
    }

    void DrawTrajectory()
    {
        Vector3 origin = launchPointLine.position;
        Vector3 startVelocity = launchSpeedLine  * 20f * launchPoint.up;
        lineRenderer.positionCount = linePoints;
        float time = 0;
        for (int i = 0; i < linePoints; i++)
        {
            // s = u*t + 1/2*g*t*t
            var x = (startVelocity.x * time) + (Physics.gravity.x / 2 * time * time);
            var y = (startVelocity.y * time) + (Physics.gravity.y / 2 * time * time);
            Vector3 point = new Vector3(x, y, 0);
            lineRenderer.SetPosition(i, origin + point);
            time += timeIntervalInPoints;
        }
    }
}