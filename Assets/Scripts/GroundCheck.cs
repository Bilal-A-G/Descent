using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public LayerMask groundLayer;
    public int numberOfRaycasts = 5;
    public float raycastDistance = 0.1f;
    public float maxSlopeAngle = 45f;
    public bool isGrounded;

    void Update()
    {
        isGrounded = CheckGround();
    }

    bool CheckGround()
    {
        
        float coneAngle = 45f; 
        float angleIncrement = coneAngle / (numberOfRaycasts - 1);

        for (int i = 0; i < numberOfRaycasts; i++)
        {
            
            float angle = -coneAngle / 2 + i * angleIncrement;
            Vector3 rayDirection = Quaternion.Euler(angle, 0, 0) * transform.TransformDirection(Vector3.forward);

            
            RaycastHit hit;
            if (Physics.Raycast(transform.position, rayDirection, out hit, raycastDistance, groundLayer))
            {
                // Visualize the ray
                Debug.DrawRay(transform.position, rayDirection * raycastDistance, Color.green);

                
                if (Vector3.Dot(hit.normal, Vector3.up) > Mathf.Cos(maxSlopeAngle * Mathf.Deg2Rad))
                {
                    return true; 
                }
            }
            else
            {
                // Visualize the ray
                Debug.DrawRay(transform.position, rayDirection * raycastDistance, Color.red);
            }
        }

        return false; // No ray has hit the ground
    }
}
    