using UnityEngine;
using UnityEngine.UIElements;

public class Rotator : MonoBehaviour
{
    private void Update()
    {
        this.transform.RotateAround(this.transform.position, new Vector3(0, 1, 0), 0.1f);
    }
}
