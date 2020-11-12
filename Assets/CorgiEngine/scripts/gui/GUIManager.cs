using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MoreMountains.CorgiEngine
{	
	/// <summary>
	/// Handles all GUI effects and changes
	/// </summary>
	public class GUIManager : MonoBehaviour 
	{
		/// the game object that contains the heads up display (avatar, health, points...)
		public GameObject HUD;
		/// the pause screen game object
		public GameObject PauseScreen;	
		/// the time splash gameobject
		public GameObject TimeSplash;
		/// The mobile buttons
		public GameObject Buttons;
		/// The mobile movement pad
		public GameObject Pad;
		/// the points counter
		public Text PointsText;
		/// the level display
		public Text LevelText;
		/// the screen used for all fades
		public Image Fader;
		/// the jetpack bar
		public GameObject JetPackBar;

	    protected static GUIManager _instance;
		
		// Singleton pattern
		public static GUIManager Instance
		{
			get
			{
				if(_instance == null)
					_instance = GameObject.FindObjectOfType<GUIManager>();
				return _instance;
			}
		}

	    /// <summary>
	    /// Initialization
	    /// </summary>
	    protected virtual void Start()
		{
			RefreshPoints();
			
		}

	    /// <summary>
	    /// Sets the HUD active or inactive
	    /// </summary>
	    /// <param name="state">If set to <c>true</c> turns the HUD active, turns it off otherwise.</param>
	    public virtual void SetHUDActive(bool state)
	    {
	        if (HUD!= null)
	        { 
	            HUD.SetActive(state);
	        }
	        if (PointsText!= null)
	        { 
	            PointsText.enabled = state;
	        }
	        if (LevelText!= null)
	        { 
	            LevelText.enabled = state;
	        }
	    }

	    /// <summary>
	    /// Sets the avatar active or inactive
	    /// </summary>
	    /// <param name="state">If set to <c>true</c> turns the HUD active, turns it off otherwise.</param>
	    public virtual void SetAvatarActive(bool state)
	    {
	        if (HUD != null)
	        {
	            HUD.SetActive(state);
	        }
	    }

	    public virtual void SetMobileControlsActive(bool state)
		{
			Pad.SetActive(state);
			Buttons.SetActive(state);
		}

		/// <summary>
		/// Sets the pause.
		/// </summary>
		/// <param name="state">If set to <c>true</c>, sets the pause.</param>
		public virtual void SetPause(bool state)
		{
	        if (PauseScreen!= null)
	        { 
	    		PauseScreen.SetActive(state);
	        }
	    }

		/// <summary>
		/// Sets the jetpackbar active or not.
		/// </summary>
		/// <param name="state">If set to <c>true</c>, sets the pause.</param>
		public virtual void SetJetpackBar(bool state)
		{
	        if (JetPackBar != null)
	        { 
			    JetPackBar.SetActive(state);
	        }
	    }

		/// <summary>
		/// Sets the time splash.
		/// </summary>
		/// <param name="state">If set to <c>true</c>, turns the timesplash on.</param>
		public virtual void SetTimeSplash(bool state)
		{
	        if (TimeSplash != null)
	        {
	            TimeSplash.SetActive(state);
	        }
		}
		
		/// <summary>
		/// Sets the text to the game manager's points.
		/// </summary>
		public virtual void RefreshPoints()
		{
	        if (PointsText!= null)
	        { 
	    		PointsText.text="$"+GameManager.Instance.Points.ToString("000000");
	        }
	    }
		
		/// <summary>
		/// Sets the level name in the HUD
		/// </summary>
		public virtual void SetLevelName(string name)
		{
	        if (LevelText!= null)
	        { 
	    		LevelText.text=name;
	        }
	    }
		
		/// <summary>
		/// Fades the fader in or out depending on the state
		/// </summary>
		/// <param name="state">If set to <c>true</c> fades the fader in, otherwise out if <c>false</c>.</param>
		public virtual void FaderOn(bool state,float duration)
		{
	        if (Fader!= null)
	        { 
			    Fader.gameObject.SetActive(true);
			    if (state)
				    StartCoroutine(CorgiTools.FadeImage(Fader,duration, new Color(0,0,0,1f)));
			    else
				    StartCoroutine(CorgiTools.FadeImage(Fader,duration,new Color(0,0,0,0f)));
	        }
	    }
		

	}
}