using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ButtonHandler : MonoBehaviour
{
    public void Restart_OnClick()
    {
        GameObject.Find("GamePlay").GetComponent<GamePlayController>().Restart();
    }

	public void PlayOnePlayer_OnClick()
	{
		SceneManager.LoadScene("PlayScene", LoadSceneMode.Single);
		//GameObject.Find("GamePlay").GetComponent<GamePlayController>().Restart();
		ScenesManager.Inst.IsOnePlayer = true;
	}

	public void PlayTwoPlayer_OnClick()
	{
		SceneManager.LoadScene("PlayScene", LoadSceneMode.Single);
		//GameObject.Find("GamePlay").GetComponent<GamePlayController>().Restart();
		ScenesManager.Inst.IsOnePlayer = false;
	}

	public void GoToMainMenu_OnClick()
	{
		GameObject.Find("GamePlay").GetComponent<GamePlayController>().Restart();
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}
}
