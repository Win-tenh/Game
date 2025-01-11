using UnityEngine;

public class Domdom : MonoBehaviour
{
    public float moveSpeed;
    float xDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveSpeed = 12;
        xDirection = Input.GetAxisRaw("Horizontal");
        float moveStep = moveSpeed * xDirection * Time.deltaTime;
        if ((transform.position.x > 7f && xDirection > 0) || (transform.position.x < -7f && xDirection < 0))
            return;
        transform.position = transform.position + new Vector3(moveStep, 0, 0); // Thêm dấu chấm phẩy ở đây
    }
}
