﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class ControleInput : MonoBehaviour {
	
	public float velocidade;
	public float forca = 250;
	public float offSet = 0.2f;
	private bool isInCantoEsquerdo;
	private bool isInCantoDireito;
	private GameObject player;
	private bool isEsquerda;
	private Rigidbody2D playerBody;
	private Player playerClass;
	private Rigidbody2D cameraBody;
	private float horzExtent;
	private float offSetEsquerda = 0;
	private Bounds playerBounds;
	private Vector2 touchPos;
	public Animator anim;
    AudioSource audio;
    public AudioClip AudioDash;

    // Use this for initialization
    void Start () {
		//camera = GetComponent.<Camera>;
		player = GameObject.Find("player");

		cameraBody = Camera.main.GetComponent<Rigidbody2D>();
		// Recupera rigidbody do player
		playerBody = player.GetComponent<Rigidbody2D> ();

		//playerBounds = player.GetComponent<BoxCollider2D>().bounds;
		playerBounds = player.GetComponent<CircleCollider2D>().bounds;

		playerClass = player.GetComponent<Player>();

		anim = player.GetComponent<Animator>();

		horzExtent = Camera.main.orthographicSize * Screen.width / Screen.height;

		playerBody.velocity =  calculaVelocidadeDireita();
        audio = GetComponent<AudioSource>();
    }

	
	// Update is called once per frame
	void Update () {

		if(!playerClass.dead)
		{

			if(offSetEsquerda == 0)
			{
				offSetEsquerda = horzExtent;
			}

			if((transform.position.x - offSetEsquerda + offSet) > (player.transform.position.x))
			{
				isInCantoEsquerdo = true;
				playerBody.velocity = new Vector2(cameraBody.velocity.x, playerBody.velocity.y) ;
			}
			else
			{
				isInCantoEsquerdo = false;
			}

			if((transform.position.x + offSetEsquerda - offSet) < (player.transform.position.x))
			{
				isInCantoDireito = true;
				playerBody.velocity = new Vector2(cameraBody.velocity.x, playerBody.velocity.y) ;
			}
			else
			{
				isInCantoDireito = false;
			}


			#if UNITY_EDITOR_WIN
			processaInputWindows();
			#endif
			processaInputMobile();

			//player.transform.Translate (Vector2.right * velocidade * Time.deltaTime);
				
		}
	}

	public void direta_touch()
	{
		voa ();
		if(isEsquerda)
		{
			flipPersonagem();
		}
		if (isInCantoDireito) 
		{
			return;
		}
		if(playerBody.velocity.x < calculaVelocidadeDireita().x)
		{
			playerBody.velocity =  calculaVelocidadeDireita();
		}
	}

	public void esquerda_touch(){

		voa ();

		if(!isEsquerda)
		{
			flipPersonagem();
		}

		if (isInCantoEsquerdo) 
		{
			return;
		}

		if(playerBody.velocity.x > (calculaVelocidadeEsquerda()).x)
		{
			playerBody.velocity =  calculaVelocidadeEsquerda();
		}

	}

	private void flipPersonagem()
	{

		isEsquerda = !isEsquerda;
		player.transform.localRotation = Quaternion.Euler(0, isEsquerda ? 180 : 0, 0);
	}

	private void voa()
	{
		if (playerBody.velocity.y > 0) {
			playerBody.velocity = new Vector2(playerBody.velocity.x, 0);
		}
		playerBody.AddForce(Vector2.up * forca);
	}

	private void processaInputWindows(){

		if (Input.GetMouseButtonDown(0))// verifica se mouse foi clicado
		{		
			Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			touchPos = new Vector2(wp.x, wp.y);
			
			if(touchPos.x > transform.position.x)
			{
				direta_touch();
			}
			else
			{
				esquerda_touch();
			}
		}
		if (Input.GetMouseButtonUp (0)) 
		{
			Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 touchPosUp = new Vector2(wp.x, wp.y);
			if(Vector2.Distance(touchPos,touchPosUp) > 4){
                audio.PlayOneShot(AudioDash, 0.7F);
                isEsquerda = ! (touchPos.x > touchPosUp.x);
				flipPersonagem();
				playerBody.velocity = isEsquerda ? calculaVelocidadeEsquerda() : calculaVelocidadeDireita();

				Vector2 dir = touchPosUp - playerBody.position;
				dir = dir.normalized;
				playerBody.position = playerBody.position + dir * 2;
			}
		}
	}

	private void processaInputMobile(){
		
		// if unity editor
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{		
			Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			touchPos = new Vector2(wp.x, wp.y);
			
			if(touchPos.x > playerBody.position.x)
			{
				direta_touch();
			}
			else
			{
				esquerda_touch();
			}
		}
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) 
		{
			//Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			Vector2 touchPosUp = new Vector2(wp.x, wp.y);
			if(Vector2.Distance(touchPos,touchPosUp) > 4){
                audio.PlayOneShot(AudioDash, 0.7F);
                isEsquerda = ! (touchPos.x > touchPosUp.x);
				flipPersonagem();
				playerBody.velocity = isEsquerda ? calculaVelocidadeEsquerda() : calculaVelocidadeDireita();
				
				Vector2 dir = touchPosUp - playerBody.position;
				dir = dir.normalized;
				playerBody.position = playerBody.position + dir * 2;
			}

		}
	}

	public Vector2 calculaVelocidadeDireita()
	{
		return ((Vector2.right * velocidade) + cameraBody.velocity);
	}

	public Vector2 calculaVelocidadeEsquerda()
	{
		return (Vector2.left * velocidade);
	}
}
