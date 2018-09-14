using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    // config param
    [SerializeField] Paddle paddle1;
    [SerializeField] float xInit = 2f;
    [SerializeField] float yInit = 15f;
    [SerializeField] AudioClip[]  ballAudio;
    [SerializeField] float randomFactor = 0.2f;

    private bool hasStarted = false;

    //state
    Vector2 paddleToBallVector;

    //Cached Information

    AudioSource myAudioSource;
    Rigidbody2D myRigidbody2D;

	// Use this for initialization
	void Start ()
    {
        paddleToBallVector.y = transform.position.y - paddle1.transform.position.y;
        myAudioSource = GetComponent<AudioSource>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
	}
	

	// Update is called once per frame
	void Update ()
    {
        if (!hasStarted)
        {
            LockBallToPaddle();
            LaunchBallOnClick();
        }
        
    }

    private void LockBallToPaddle()
    {
        Vector2 paddlePos = new Vector2(paddle1.transform.position.x, paddle1.transform.position.y);
        transform.position = paddlePos + paddleToBallVector;
    }

    private void LaunchBallOnClick()
    {
        if(Input.GetMouseButtonDown(0))
        {
            myRigidbody2D.velocity = new Vector2(xInit, yInit);
            hasStarted = true;
        }   
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 velocityTweak = new Vector2
            (Random.Range(0f,randomFactor),
            Random.Range(0f,randomFactor));

        if (hasStarted)
        {
            AudioClip clips = ballAudio[UnityEngine.Random.Range(0, ballAudio.Length)];
            myAudioSource.PlayOneShot(clips);
            myRigidbody2D.velocity += velocityTweak;
        }
        
    }
}
