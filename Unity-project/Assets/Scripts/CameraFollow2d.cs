using UnityEngine;

public class CameraFollow2d : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private string playerTag;
    [SerializeField] private float movingSpeed;

    private void Awake()
    {
        if (playerTransform != null)
        {
            if (this.playerTag != null)
            {
                this.playerTag = "Player";
            }
            this.playerTransform = GameObject.FindGameObjectWithTag(this.playerTag).transform;
        }
        this.transform.position = new Vector3()
        {
            x = this.transform.position.x,
            y = this.transform.position.y,
            z = this.transform.position.z - 10
        };
    }

    private void FixedUpdate()
    {
        if (this.playerTransform) 
        {
            Vector3 target = new ()
            {
                x = this.playerTransform.position.x,
                y = this.playerTransform.position.y,
                z = this.playerTransform.position.z - 10,
            };

            Vector3 pos = Vector3.Lerp(this.transform.position, target, movingSpeed * Time.deltaTime);
            this.transform.position = pos;
        }
    }
}
