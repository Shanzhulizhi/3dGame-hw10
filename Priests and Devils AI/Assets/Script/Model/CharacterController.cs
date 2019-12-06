using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController{

	private GameObject character;
//	private Moveable moveableScript;
	private UserGUI clickGUI;
	private int type; // 0->priest, 1->devil

	// change frequently
	bool _isOnBoat;
	CoastController coastController;


	public CharacterController(string _character) {

		if (_character == "priest") {
			character = Object.Instantiate (Resources.Load ("Perfabs/Priest", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
			type = 0;
		} else {
			character = Object.Instantiate (Resources.Load ("Perfabs/Devil", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
			type = 1;
		}
//		moveableScript = character.AddComponent (typeof(Moveable)) as Moveable;

		clickGUI = character.AddComponent (typeof(UserGUI)) as UserGUI;
		clickGUI.setController (this);
	}

	public void setName(string _name) {
		character.name = _name;
	}

	public GameObject getGameobj() {
		return this.character;
	}

	public string getName() {
		return character.name;
	}

	public int getType() {  // 0->priest, 1->devil
		return type;
	}

	public Vector3 getPos() {
		return this.character.transform.position;
	}

	public CoastController getCoastController() {
		return coastController;
	}

	public bool isOnBoat() {
		return _isOnBoat;
	}

	public void setPosition(Vector3 _pos) {
		character.transform.position = _pos;
	}

//	public void moveToPosition(Vector3 _destination) {
//		moveableScript.setDestination(_destination);
//	}
//
	public void getOnBoat(BoatController _boatCtrl) {
		coastController = null;
		character.transform.parent = _boatCtrl.getGameobj().transform;//add character as the son of boat
		_isOnBoat = true;
	}

	public void getOnCoast(CoastController _coastCtrl) {
		coastController = _coastCtrl;
		character.transform.parent = null;
		_isOnBoat = false;
	}


	public void Reset() {
//		moveableScript.Reset();
		coastController = (SSDirector.getInstance ().currentSceneController as FirstController).leftCoast;
		getOnCoast (coastController);
		setPosition (coastController.getEmptyPosition ());
		coastController.getOnCoast (this);
	}

	
}
