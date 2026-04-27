using UnityEngine;

public class RotatePlatform : MonoBehaviour
{
    public int speed;
    public GameObject customPivot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * speed);
        customPivot.transform.rotation = transform.rotation;
        
    }
}
