using UnityEngine;

public class AnimationFade : MonoBehaviour
{
    [SerializeField] private Animator animator;
    // Start is called before the first frame update
    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void PlayAni(string nameAni)
    {
        animator.SetTrigger(nameAni);
    }
}
