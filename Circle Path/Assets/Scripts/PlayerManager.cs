using System;
using System.Collections;
using Sonity;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    
    public CharacterData data;
    
    public ParticleSystem diePart;

    public SoundEvent spawnSound;
    public SoundEvent dieSound;

    public Transform spawnPoint;
    
    public AnimationCurve playerMoveCurve;
    
    public Action reviveAction;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Revive();
    }

    public void LevelComplete()
    {
        StartCoroutine(GoToNextLevel());
    }

    private IEnumerator GoToNextLevel()
    {
        data.canMove = false;
        data.anim.SetTrigger("IsComplete");
        yield return new WaitForSeconds(1f);
        Revive();
    }

    public void Die()
    {
        data.dead = true;
        diePart.transform.position = data.transform.position;
        diePart.Play();
        SoundManager.Instance.Play(dieSound, transform);
        data.gameObject.SetActive(false);
        StartCoroutine(DelayedRevive());
    }

    private IEnumerator DelayReviveAction()
    {
        yield return new WaitForSecondsRealtime(2f);
        if (reviveAction != null)
        {
            reviveAction();
        }
    }

    public void Revive()
    {
        StartCoroutine(DelayReviveAction());
        data.gameObject.SetActive(true);
        MovePlayer(spawnPoint);
        SoundManager.Instance.Play(spawnSound, transform);
        data.playerVel.rb.velocity = Vector2.zero;
        data.playerVel.rb.angularVelocity = 0;
        data.isPlaying = true;
        data.dead = false;
        data.canMove = true;
    }
    
    private IEnumerator DelayedRevive()
    {
        yield return new WaitForSeconds(1f); // Задержка в секунду
        Revive();
    }

    public void MovePlayer(Transform spawnPoint)
    {
        StartCoroutine(Move(data.playerVel, spawnPoint.localPosition));
        //SoundManager.Instance.Play(spawnSound, transform);
    }

    private IEnumerator Move(PlayerVelocity player, Vector3 targetPos)
    {
        data.isPlaying = false;
        player.rb.simulated = false;
        player.rb.isKinematic = true;
        Vector3 distance = targetPos - player.transform.position;
        Vector3 targetStartPos = player.transform.position;
        float t = playerMoveCurve.keys[playerMoveCurve.keys.Length - 1].time;
        float c = 0f;
        while (c < t)
        {
            c += Mathf.Clamp(Time.unscaledDeltaTime, 0f, 0.02f);
            player.transform.position = targetStartPos + distance * playerMoveCurve.Evaluate(c);
            yield return null;
        }
        player.transform.position = targetStartPos + distance;
        player.rb.simulated = true;
        player.rb.isKinematic = false;
    }
}
