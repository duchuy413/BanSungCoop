﻿using UnityEngine;
using System.Collections;

namespace MoreMountains.CorgiEngine
{	
	/// <summary>
	/// Add this component to a CorgiController2D and it will try to kill your player on sight.
	/// </summary>
	public class AIShootOnSight : MonoBehaviour 
	{
		
		/// The fire rate (in seconds)
		public float FireRate = 1;
		/// The kind of projectile shot by the agent
		public Projectile Projectile;
		/// The maximum distance at which the AI can shoot at the player
		public float ShootDistance = 10f;
		/// The offset to apply to the shoot origin point (by default the position of the object)
		public Vector2 ShootOriginOffset = new Vector2(0,0);

		// private stuff
		protected float _canFireIn;
	    protected Vector2 _direction;
	    protected Vector2 _directionLeft;
	    protected Vector2 _directionRight;
	    protected CorgiController _controller;

		/// initialization
		protected virtual void Start () 
		{
			_directionLeft = new Vector2(-1,0);
			_directionRight = new Vector2(1,0);
			// we get the CorgiController2D component
			_controller = GetComponent<CorgiController>();
		}

	    /// Every frame, check for the player and try and kill it
	    protected virtual void FixedUpdate () 
		{
			// fire cooldown
			if ((_canFireIn-=Time.deltaTime) > 0)
			{
				return;
			}

			// determine the direction of the AI
			if (transform.localScale.x < 0) 
			{
				_direction=-_directionLeft;
			}
			else
			{
				_direction=-_directionRight;
			}
			
			// we cast a ray in front of the agent to check for a Player
			Vector2 raycastOrigin = new Vector2(transform.position.x+ShootOriginOffset.x,transform.position.y+ShootOriginOffset.y);

			RaycastHit2D raycast = CorgiTools.CorgiRayCast(raycastOrigin,_direction,ShootDistance,1<<LayerMask.NameToLayer("Player"),true,Color.yellow,true);
			if (!raycast)
				return;
			
			// if the ray has hit the player, we fire a projectile
			Projectile projectile = (Projectile)Instantiate(Projectile, raycastOrigin,transform.rotation);
			projectile.Initialize(gameObject,_direction,_controller.Speed);
			_canFireIn=FireRate;
		}
	}
}