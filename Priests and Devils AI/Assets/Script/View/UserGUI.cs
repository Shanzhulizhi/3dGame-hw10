using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {
	private UserAction action;
	public int status;
	GUIStyle style;
	GUIStyle buttonStyle;
	private string des="Cube represents Priest\n" +
		"Sphere represents Devil\n" +
		"One boat can load two characters at most\n";
	bool flag = false;

	public static AIState StartState = new AIState(0, 0, 3, 3, false, null);
    public static AIState endState = new AIState(3, 3, 0, 0, true, null);
 	private string hint = "";
 	public static FirstController sceneController;

	void Start() {
		action = SSDirector.getInstance ().currentSceneController as UserAction;
		sceneController = SSDirector.getInstance ().currentSceneController as FirstController;

		style = new GUIStyle();
		style.fontSize = 40;
		style.alignment = TextAnchor.MiddleCenter;
		style.normal.textColor = Color.red;

		buttonStyle = new GUIStyle("button");
		buttonStyle.fontSize = 30;
		buttonStyle.alignment = TextAnchor.MiddleCenter;
		status = 0;

	}
	void OnGUI() {
		GUIStyle Box_Style = new GUIStyle {
			fontSize=30,
			fontStyle=FontStyle.Bold
		};
		//
		Box_Style.normal.textColor = Color.black;
		//
		GUI.Box(new Rect(Screen.width/2-100, Screen.height/2-150, 100, 50),"Priest and Devil",Box_Style);

		GUIStyle description = new GUIStyle {
			fontSize=15,
//			border = new RectOffset();
		};

		description.normal.textColor = Color.blue;


		GUI.Label (new Rect (Screen.width / 2 - 400, Screen.height / 2 - 120, 100, 50), des, description);

		GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 250, 100, 50),hint, description);

		if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 90, 100, 50), "Hint", buttonStyle)) {
			if(hint!=""){
				flag = true;
			}
			// Debug.Log("StateRight: " + StartState.rightDevils + " " + StartState.rightPriests);
			// Debug.Log("StateLeft: " + StartState.leftDevils + " " + StartState.leftPriests);
			AIState temp = AIState.BFS(StartState, endState);
			// Debug.Log("NextRight: " + temp.rightDevils + " " + temp.rightPriests);
			// Debug.Log("NextLeft: " + temp.leftDevils + " " + temp.leftPriests);
			hint = "Hint:\n" + 
			"You should make the next state is like :\n"+
			"In Left Coast: "+temp.getLD()+"   Devils and  "+temp.getLP()+"   Priests:\n"+
			"In Right Coast: "+temp.getRD()+" Devils and "+temp.getRP()+"   Priests\n";
			if(flag){
				hint = "";
				flag = false;
			}
		}

//		Debug.Log (status);
		if (status == 1) {
			GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-45, 100, 50), "Gameover!", style);
			if (GUI.Button(new Rect(Screen.width/2-70, Screen.height/2+100, 140, 70), "Restart", buttonStyle)) {
				status = 0;
				action.restart ();
				StartState = new AIState(0, 0, 3, 3, false, null);
			}
		} else if(status == 2) {
			GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-45, 100, 50), "You win!", style);
			if (GUI.Button(new Rect(Screen.width/2-70, Screen.height/2+100, 140, 70), "Restart", buttonStyle)) {
				status = 0;
				action.restart ();
				StartState = new AIState(0, 0, 3, 3, false, null);
			}
		}
	}

	public CharacterController characterController;

	public void setController(CharacterController _characterCtrl) {
		characterController = _characterCtrl;
	}
		
	public void OnMouseDown() {
		Debug.Log ("OnMouseDown");
//		Debug.Log (status);
		if (status != 1 && status != 2) {
			if (gameObject.name == "boat") {
				action.moveBoat ();
				//更新StartState
			 	//获得当前信息
			 	int rightPriests = sceneController.leftCoast.getCharacterNum()[0];
				int rightDevils = sceneController.leftCoast.getCharacterNum()[1];
				int leftPriests = sceneController.rightCoast.getCharacterNum()[0];
				int leftDevils = sceneController.rightCoast.getCharacterNum()[1];
				bool direction = sceneController.boat.get_to_or_from () == 1 ? false : true;
				int priest_count = sceneController.boat.getCharacterNum()[0];
				int devil_count = sceneController.boat.getCharacterNum()[1];
				//true为船在左岸，左岸数量要加上船上数量；否则在右岸
				if (direction) {
					leftPriests += priest_count;
					leftDevils += devil_count;
				} else {
					rightPriests += priest_count;
					rightDevils += devil_count;
				}
				// Debug.Log(sceneController.boat.get_to_or_from ());
				// Debug.Log(rightDevils+","+rightPriests);
				// Debug.Log(leftDevils+","+leftPriests);
				// Debug.Log(direction);
				//更新当前状态
				StartState = new AIState(leftPriests, leftDevils, rightPriests, rightDevils, direction , null);
			} else {
				action.characterIsClicked (characterController);
			}
			
		}
	}
}