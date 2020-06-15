using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocAudioController : MonoBehaviour
{
    private AudioSource m_Source;

    public AudioClip[] attackClips;

    public AudioClip[] idleClips;

    public AudioClip[] killPlayerClips;

    public AudioClip[] hitPlayerClips;

    public AudioClip[] flyingAttackClips;

    private int iRand = 0;

    private void Start()
    {
        m_Source = GetComponent<AudioSource>();
    }

    public void PlayAttackAudioClip(bool bAttackingPlayer)
    {
        if (bAttackingPlayer)
        {
            iRand = Random.Range(0, attackClips.Length);

            m_Source.PlayOneShot(attackClips[iRand], Random.Range(0.8f, 1.2f));

        }
        else
        {

        }
    }

    public void PlayKillPlayerClip()
    {
        iRand = Random.Range(0, killPlayerClips.Length);

        m_Source.PlayOneShot(killPlayerClips[iRand], Random.Range(0.8f, 1.2f));
    }

    public void PlayHitPlayerClip()
    {
        iRand = Random.Range(0, hitPlayerClips.Length);

        m_Source.PlayOneShot(hitPlayerClips[iRand], Random.Range(0.8f, 1.2f));
    }

    public void PlayIdleClip()
    {
        iRand = Random.Range(0, idleClips.Length);

        m_Source.PlayOneShot(idleClips[iRand], Random.Range(0.8f, 1.2f));
    }
}
