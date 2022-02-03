using UnityEngine;

    class PlayerAnimID
    {
        public static readonly int MOVE = Animator.StringToHash("Move");
        public static readonly int RELOAD = Animator.StringToHash("Reload");
        public static readonly int DIE = Animator.StringToHash("Die");
    }

    class ZombieAnimID
    {
        public static readonly int HAS_TARGET = Animator.StringToHash("HasTarget");
        public static readonly int DIE = Animator.StringToHash("Die");
    }
namespace Utils.AnimID
{
}
