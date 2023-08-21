using Photon.Pun;
using UnityEngine;

public class PlayableCharacterAnimationController : MonoBehaviourPunCallbacks
{
    [SerializeField] private Animator animator;

    [PunRPC]
    private void OnSpawned() =>
        RandomIdleAnimation();

    [PunRPC]
    public void OnCharacterSelected() =>
        animator.Play(Constants.Animations.Clips.ON_SELECTED);

    [PunRPC]
    public void OnCharacterMoved()
    {
        animator.SetInteger(Constants.Animations.Parameters.IDLE, 0);
        animator.Play(Constants.Animations.Clips.RUNNING);
    }

    [PunRPC]
    public void OnCharacterStop() =>
        RandomIdleAnimation();

    private void RandomIdleAnimation()
    {
        animator.SetFloat(Constants.Animations.Parameters.IDLE_SPEED, Random.Range(1, 1.5f));
        animator.SetInteger(Constants.Animations.Parameters.IDLE,
            Random.Range(1, Constants.CHARACTERS_ANIMATIONS_IDLE_COUNT)
        );
    }
}