using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class Brain : MonoBehaviour
{
	public float timeAlive;
	public float distanceMoved;
	public DNA dna;

    private ThirdPersonCharacter mCharacter;
	private Vector3 startPosition;
	private Vector3 vMove;
	private bool jump;
	private bool crouch;
	private bool alive;

	void OnCollisionEnter(Collision obj)
    {
		if (obj.gameObject.tag == "dead")
		{
			alive = false;
			timeAlive = Population.elapsed;
		}
    }
    
	public void Init()
	{
        dna = new DNA(1, 0, 3, 2, -1, 1);
		mCharacter = GetComponent<ThirdPersonCharacter>();
        timeAlive = Population.trialTime;
		startPosition = transform.position;
		alive = true;
	}
	private void Update()
	{
		switch (dna.GetIGene(0))
		{
			case 0: jump = crouch = false; break;
			case 1: jump = true; crouch = false; break;
			case 2: jump = false; crouch = true; break;
		}
	}
	private void FixedUpdate()
    {
        vMove = dna.GetFGene(0) * Vector3.forward + dna.GetFGene(1) * Vector3.right;
        mCharacter.Move(vMove, crouch, jump);
        jump = false;
		if (alive)
		{
			Vector3 d = transform.position - startPosition;
			distanceMoved = Mathf.Sqrt(d.x * d.x + d.z * d.z);
		}
    }
}
