﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class EnemyBot : MonoBehaviour {


	public int CurrentLife = 4;
	public int MaxLife = 4;
	public bool dead = false;
	public Animator anim;
	public GameObject coracaoPrefab;
	public Vector3 direcao ;
	public float force = 250;
	//-------------
	private GameObject enemy;
	private Rigidbody2D enemyBody;
	//-------------
	private GameObject spawnerBottom;
    private GameObject player;
    //private Rigidbody2D playerBody;
    //-------------
    public float speed =3;
    //-------------
    //audio
    AudioSource audio;
    public AudioClip AudioDamage;
    public AudioClip AudioDie;
    public AudioClip AudioDash;
    //-------------
    public Vector2 direction;
    public int randomValue;
    public int randomValue2;
    //-----------------
    //casting
    public Transform pontoCastInicialDir;
    public Transform pontoCastFinalDir;
    public Transform pontoCastInicialEsq;
    public Transform pontoCastFinalEsq;
    public Transform pontoCastInicialCima;
    public Transform pontoCastFinalCima;
    public Transform pontoCastInicialBaixo;
    public Transform pontoCastFinalBaixo;
    public bool hasCollisionInCastWithObstaculo = false;
    public bool hasCollisionInCastWithObstaculoCima = false; 
    public bool hasCollisionInCastWithObstaculoBaixo = false;
    public bool hasCollisionInCastWithPlayerDir = false;
    public bool hasCollisionInCastWithPlayerEsq = false;
    public bool hasCollisionInCastWithPlayerCima = false;
    public bool hasCollisionInCastWithPlayerBaixo = false;
    public bool hasCollisionInCastWithLimiteCameraDir = false;
    public bool hasCollisionInCastWithLimiteCameraEsq = false;


    void Start(){

		enemy = GameObject.FindGameObjectWithTag("Enemy_bot");
        player = GameObject.FindGameObjectWithTag("Player");
        spawnerBottom = GameObject.FindGameObjectWithTag("spawn_bottom");
		enemyBody = enemy.GetComponent<Rigidbody2D> ();
        //playerBody = player.GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
    }

	void FixedUpdate()
	{
		Vector2 enemyVel = enemyBody.velocity;
		enemyVel.x = speed;
		enemyBody.velocity = enemyVel;

		if (enemyBody.position.y < spawnerBottom.transform.position.y && dead == false) 
		{
            enemyFly();
        }

		enemyBody.rotation = 0f;

        //Random dash
        randomValue = Random.Range(0, 50);
        randomValue2 = Random.Range(0, 500);
        if (randomValue2 == 5)
        {
            Dash(new Vector2 (Random.Range(-1f,1f), Random.Range(-1f, 1f)));
        }
        // Dash(new Vector2 (Random.Range(-1f,1f), Random.Range(-1f, 1f)));
        //Dash(new Vector2(0, 1));
        //Dash(new Vector2(1, 0)); 
        //Dash(new Vector2(1, -1));
        //Dash(new Vector2(-1, 1));

        RayCasting();

        if (dead == false)
        {
            if (hasCollisionInCastWithObstaculo || hasCollisionInCastWithObstaculoBaixo)
            {
                enemyFly();
            }
            if (hasCollisionInCastWithObstaculoCima)
            {
                Dash(new Vector2(0, -1));
                Dash(new Vector2(1, 0));
            }

            if (hasCollisionInCastWithPlayerDir && randomValue == 5)
            {
                Dash(new Vector2(1, 0));
            }
            if (hasCollisionInCastWithPlayerEsq && randomValue == 5)
            {
                Dash(new Vector2(-1, 0));
            }
            if (hasCollisionInCastWithPlayerCima && randomValue == 5)
            {
                Dash(new Vector2(0, 1));
            }
            if (hasCollisionInCastWithPlayerBaixo && randomValue == 5)
            {
                Dash(new Vector2(0, -1));
            }

            if (hasCollisionInCastWithLimiteCameraDir)
            {
                Dash(new Vector2(-1, 0));
            }
            if (hasCollisionInCastWithLimiteCameraEsq)
            {
                Dash(new Vector2(1, 0));
            }
        }
        
    }

    void RayCasting()
    {
        //RaycastHit2D hit;
        Debug.DrawLine(pontoCastInicialDir.position, pontoCastFinalDir.position, Color.red);
        Debug.DrawLine(pontoCastInicialEsq.position, pontoCastFinalEsq.position, Color.red);
        Debug.DrawLine(pontoCastInicialCima.position, pontoCastFinalCima.position, Color.red);
        Debug.DrawLine(pontoCastInicialBaixo.position, pontoCastFinalBaixo.position, Color.red);

        //hasCollisionInCast = Physics2D.Linecast (pontoCastMeioInicial.position, pontoCastMeioFinal.position, 
        //                                     1 << LayerMask.NameToLayer("bloco"));
        //hit = Physics2D.Raycast(pontoCastMeioInicial.position, pontoCastMeioFinal.position);

        hasCollisionInCastWithObstaculo = Physics2D.Linecast(pontoCastInicialDir.position, pontoCastFinalDir.position, 1 << LayerMask.NameToLayer("Obstaculo"));
        hasCollisionInCastWithObstaculoCima = Physics2D.Linecast(pontoCastInicialCima.position, pontoCastFinalCima.position, 1 << LayerMask.NameToLayer("Obstaculo"));
        hasCollisionInCastWithObstaculoBaixo = Physics2D.Linecast(pontoCastInicialBaixo.position, pontoCastFinalBaixo.position, 1 << LayerMask.NameToLayer("Obstaculo"));
        hasCollisionInCastWithPlayerDir = Physics2D.Linecast(pontoCastInicialDir.position, pontoCastFinalDir.position, 1 << LayerMask.NameToLayer("Player"));
        hasCollisionInCastWithPlayerEsq = Physics2D.Linecast(pontoCastInicialEsq.position, pontoCastFinalEsq.position, 1 << LayerMask.NameToLayer("Player"));
        hasCollisionInCastWithPlayerCima = Physics2D.Linecast(pontoCastInicialCima.position, pontoCastFinalCima.position, 1 << LayerMask.NameToLayer("Player"));
        hasCollisionInCastWithPlayerBaixo = Physics2D.Linecast(pontoCastInicialBaixo.position, pontoCastFinalBaixo.position, 1 << LayerMask.NameToLayer("Player"));
        hasCollisionInCastWithLimiteCameraDir = Physics2D.Linecast(pontoCastInicialDir.position, pontoCastFinalDir.position, 1 << LayerMask.NameToLayer("LimiteCameraDir"));
        hasCollisionInCastWithLimiteCameraEsq = Physics2D.Linecast(pontoCastInicialEsq.position, pontoCastFinalEsq.position, 1 << LayerMask.NameToLayer("LimiteCameraEsq"));
    }


    private void enemyFly()
    {
        if (enemyBody.velocity.y > 0)
        {
            enemyBody.velocity = new Vector2(enemyBody.velocity.x, 0);
        }
        enemyBody.AddForce(Vector2.up * force);
    }


    void OnCollisionEnter2D(Collision2D coll) 
	{
		if (coll.gameObject.tag == "walls") 
		{
			if(dead)
			{
                audio.PlayOneShot(AudioDie, 0.7F);
                enemy.SetActive(false);
			}
			else
			{
				takeDamage(1);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D coll)  {
		if (coll.gameObject.tag == "colectble") {
			coll.gameObject.SetActive(false);
		}
	}
	
	public void takeDamage(int damage)	
	{
		CurrentLife = CurrentLife - damage;
		anim.Play("Enemy_hit");
        audio.PlayOneShot(AudioDamage, 0.7F);

        dropCoracao();

        if (CurrentLife == 0) {
            audio.PlayOneShot(AudioDie, 0.7F);
            anim.Play("Enemy_die");
            dead = true;
            // enemy.SetActive(false);
        }
	}

    //Exemplos: dash para direita(1,0)  dash esquerda(-1,0) dash baixo(0,-1) dash cima(0,1)
    public void Dash(Vector2 direction)
    {
        audio.PlayOneShot(AudioDash, 0.7F);
        //direction = new Vector2 (player.transform.position.x,player.transform.position.y);
        if (direction.x > 1)
        {
            direction = direction.normalized;
        }
        enemyBody.position = enemyBody.position + direction * 2;
    }

	void dropCoracao()
	{
		Instantiate (coracaoPrefab, transform.position, transform.rotation);
	}
}