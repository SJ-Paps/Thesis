using UnityEngine;

namespace SJ.Tools
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorWrapper : SJMonoBehaviour, IAnimator
    {
        private Animator animator;

        private Animator Animator
        {
            get
            {
                if (animator == null)
                    animator = GetComponent<Animator>();

                return animator;
            }
        }

        public void ResetTrigger(int id)
        {
            Animator.ResetTrigger(id);
        }

        public void ResetTrigger(string trigger)
        {
            Animator.ResetTrigger(trigger);
        }

        public void SetTrigger(int id)
        {
            Animator.SetTrigger(id);
        }

        public void SetTrigger(string trigger)
        {
            Animator.SetTrigger(trigger);
        }
    }
}