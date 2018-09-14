using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    [SerializeField] AudioClip breakSound;
    [SerializeField] GameObject blockVFX;
    [SerializeField] Sprite[] hitSprites;

    [SerializeField] float minPos = 1f;
    [SerializeField] float maxPos = 15f;
    [SerializeField] float speed = 1f;
    bool isMoveRight = true;

    //Cached Reference
    Level level;

    //state variables
    [SerializeField] int timesHit = 0;

    private void Start()
    {
        CountBreakableBlocks();
    }

    private void CountBreakableBlocks()
    {
        level = FindObjectOfType<Level>();
        if (tag == "Breakable" || tag == "Movable")
        {
            level.CountBlocks();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tag == "Breakable" || tag == "Movable")
        {
            HandleHit();
        }
    }

    private void HandleHit()
    {
        timesHit++;
        int maxHits = hitSprites.Length + 1;
        if (timesHit >= maxHits)
        {
            DestroyBlock();
            FindObjectOfType<GameStatus>().AddToScore();
        }
        else
        {
            ShowNextHitSprite();
        }
    }

    private void ShowNextHitSprite()
    {
        int spriteIndex = timesHit - 1;
        if (hitSprites[spriteIndex] != null)
        {
            GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
        }
        else
        {
            Debug.Log("Block sprite is missing an array" + gameObject.name);
        }
    }

    private void DestroyBlock()
    {
        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position);
        Destroy(gameObject);
        level.BlockDestroyed();
        TriggerVFX();
    }

    private void TriggerVFX()
    {
        GameObject sparkles = Instantiate(blockVFX,transform.position, transform.rotation);
        Destroy(sparkles, 1f);
    }

    private void Update()
    {
        if (tag == "Movable")
        {
            Vector2 blockPos = new Vector2(transform.position.x, transform.position.y);
            IsMovingRight(blockPos);
            if (isMoveRight)
            {
                transform.Translate(speed * Time.deltaTime, 0, 0);
            }
            else
            {
                transform.Translate(speed * Time.deltaTime * -1, 0, 0);
            }
           
        }
    }
    

    private Vector2 IsMovingRight(Vector2 blockPos)
    {
        if (blockPos.x > maxPos)
        {
            isMoveRight = false;
        }

        if (blockPos.x < minPos)
        {
            isMoveRight = true;
        }
        else
        {
            blockPos.x++;
        }

        return blockPos;
    }

}
