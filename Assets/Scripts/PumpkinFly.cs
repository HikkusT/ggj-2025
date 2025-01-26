using UnityEngine;

public class PumpkinFly : MonoBehaviour
{
    [SerializeField] private float radius = 5f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float minHeight = 1f;
    [SerializeField] private float maxHeight = 3f;
    [SerializeField] private float heightSpeed = 1f;
    [SerializeField] private float pumpkinRotationSpeed;
    [SerializeField] private Transform pumpkinTransform; // Referência ao objeto filho

    private Vector3 _initialPosition;
    private float _angle;
    private float _heightCycle;

    void Start()
    {
        _initialPosition = transform.position;
    }

    void Update()
    {
        _angle += speed * Time.deltaTime;

        float x = _initialPosition.x + Mathf.Cos(_angle) * radius;
        float z = _initialPosition.z + Mathf.Sin(_angle) * radius;
        
        pumpkinTransform.Rotate(Vector3.up, speed * -pumpkinRotationSpeed * Time.deltaTime);

        _heightCycle += heightSpeed * Time.deltaTime;
        float y = Mathf.Lerp(minHeight, maxHeight, (Mathf.Sin(_heightCycle) + 1) / 2);

        transform.position = new Vector3(x, y, z);

    }
}