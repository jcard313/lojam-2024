using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        // Check for input (left mouse click for example)
        if (Input.GetMouseButtonDown(0))
        {
            Swing();
        }
    }

    void Swing()
    {
        // Trigger the sword swing animation
        animator.SetTrigger("Swing");
    }
}
