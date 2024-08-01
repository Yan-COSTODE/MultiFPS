using UnityEngine;

public class Sway : MonoBehaviour
{
    [SerializeField] private float fSwayClamp = 0.09f;
    [SerializeField] private float fSmoothing = 3f;
    private Vector3 origin;
    
    private void Start()
    {
        origin = transform.localPosition;
    }

    private void Update()
    {
        Vector2 _input = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        _input.x = Mathf.Clamp(_input.x, -fSwayClamp, fSwayClamp);
        _input.y = Mathf.Clamp(_input.y, -fSwayClamp, fSwayClamp);
        Vector3 _target = new Vector3(-_input.x, -_input.y, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, _target + origin, Time.deltaTime * fSmoothing);
    }
}
