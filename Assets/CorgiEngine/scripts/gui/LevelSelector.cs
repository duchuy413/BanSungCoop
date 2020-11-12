using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace MoreMountains.CorgiEngine
{
	public class LevelSelector : MonoBehaviour
	{
	    public string LevelName;

	    public virtual void GoToLevel()
	    {
	        LevelManager.Instance.GotoLevel(LevelName);
	    }

	    public virtual void RestartLevel()
	    {
	        GameManager.Instance.UnPause();
	       SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	    }
		
	}
}