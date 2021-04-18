using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFllow : MonoBehaviour
{
    private Transform player;
    [Range(0f,10f)]
    public float ViewRect_X;
    [Range(0f, 10f)]
    public float ViewRect_Y;
    [Range(0f, 3f)]
    public float nearlySpeed;
    [Range(0, 10)]
    public int offset_X;
    [Range(0, 10)]
    public int offset_Y;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        //if (!JudgeSafeScreen())

        //else
            ToControllCamera(ViewRect_X, ViewRect_Y, nearlySpeed, offset_X, offset_Y);

    }

    private bool JudgeSafeScreen()
    {
        float x = this.GetComponent<Camera>().ViewportToWorldPoint(new Vector2(0, 0)).x;
        return x > -11.0f;
    }

    private void ToControllCamera(float x,float y,float nearlySpeed,float offset_x,float offset_y)
    {
        float xDirection= transform.position.x;
        float yDirection = transform.position.y;
        if (Mathf.Abs(transform.position.x - player.position.x) > x + offset_x)
        {
            xDirection = Mathf.Lerp(transform.position.x, player.position.x, Time.deltaTime * nearlySpeed);

        }
        if (Mathf.Abs(transform.position.y - player.position.y) > y + offset_y)
        {
            yDirection = Mathf.Lerp(transform.position.y, player.position.y, Time.deltaTime * nearlySpeed);
            
        }
        transform.position = new Vector3(xDirection, yDirection, -1);
    }

    IEnumerator RecordSafeArea()
    {
        while (true)
        {
            Rect area = Screen.safeArea;
            Log.Info(area.size.ToString());
            yield return new WaitForSeconds(1f);
        }
    }

}
