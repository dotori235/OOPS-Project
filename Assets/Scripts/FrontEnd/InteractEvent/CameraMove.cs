using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private float speed = 5;
    Vector3 initPos = new Vector3(0, 8, 0)
;    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = initPos;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = initPos;
        }
        dir *= speed;
        transform.position += dir*Time.deltaTime;
    }
}
