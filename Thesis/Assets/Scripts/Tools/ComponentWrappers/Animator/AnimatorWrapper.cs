using UnityEngine;

namespace SJ.Tools
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorWrapper : SJMonoBehaviour, IAnimator
    {
        private Animator animator;

        protected override void SJAwake()
        {
            animator = GetComponent<Animator>();
        }

        public void ResetTrigger(int id)
        {
            animator.ResetTrigger(id);
        }

        public void ResetTrigger(string trigger)
        {
            animator.ResetTrigger(trigger);
        }

        public void SetTrigger(int id)
        {
            animator.SetTrigger(id);
        }

        public void SetTrigger(string trigger)
        {
            animator.SetTrigger(trigger);
        }
    }
}