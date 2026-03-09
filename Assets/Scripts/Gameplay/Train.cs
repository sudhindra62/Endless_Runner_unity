
using UnityEngine;

public class Train : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("The speed at which the train moves towards the player.")]
    public float movementSpeed = 20f;

    void Update()
    {
        // Move the train forward in its local Z direction.
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
    }
}
