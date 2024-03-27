using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHover : MonoBehaviour
{
    public AudioClip hoverSound;
    public AudioClip goingtoplaceSound; 
    private AudioSource audioSource;
    private Dictionary<Card, bool> cardHoverState = new Dictionary<Card, bool>();
    private bool canPlaySound = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = hoverSound;

        Card[] cards = FindObjectsOfType<Card>();
        foreach (Card card in cards)
        {
            cardHoverState[card] = false;
        }
    }
    private IEnumerator PlayHoverSound()
    {
        canPlaySound = false;
        audioSource.PlayOneShot(hoverSound);
        yield return new WaitForSeconds(0.2f);
        canPlaySound = true;
    }
    
    private void Update()
    {
        Card[] cards;
        
        if (Input.GetMouseButtonDown(0))
        {
            cards = FindObjectsOfType<Card>();
            foreach (Card card in cards)
            {
                if (card.isHovered)
                {
                    audioSource.Stop();
                    audioSource.clip = goingtoplaceSound;
                    audioSource.Play();
                    break; 
                }
            }
        } else if (Input.GetMouseButtonUp(0))
        {
            audioSource.Stop();
        }

        cards = FindObjectsOfType<Card>();
        foreach (Card card in cards)
        {
            if (!cardHoverState.ContainsKey(card))
            {
                cardHoverState.Add(card, false);
            }

            if (card.isHovered && !cardHoverState[card] && canPlaySound)
            {
                StartCoroutine(PlayHoverSound());
                cardHoverState[card] = true;
            }
            else if (!card.isHovered && cardHoverState[card])
            {
                cardHoverState[card] = false;
            }
        }
    }
}