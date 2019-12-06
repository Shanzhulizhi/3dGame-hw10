using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoatController {


	private GameObject boat;
//	private Moveable moveableScript;
	private UserGUI clickGUI;
	private Vector3 leftPosition = new Vector3 (2.5f, 0.5f, 0);
	private Vector3 rightPosition = new Vector3 (-2.5f, 0.5f, 0);
	private Vector3[] left_positions;
	private Vector3[] right_positions;

	// change frequently
	private int direction;
	CharacterController[] passenger = new CharacterController[2];

	public BoatController() {
		direction = 1;

		left_positions = new Vector3[] { new Vector3 (2, 1.5f, 0), new Vector3 (3.2f, 1.5f, 0) };
		right_positions = new Vector3[] { new Vector3 (-3.2f, 1.5f, 0), new Vector3 (-2, 1.5f, 0) };

		boat = Object.Instantiate (Resources.Load ("Perfabs/Boat", typeof(GameObject)), leftPosition, Quaternion.identity, null) as GameObject;
		boat.name = "boat";

//		moveableScript = boat.AddComponent (typeof(Moveable)) as Moveable;
		clickGUI = boat.AddComponent (typeof(UserGUI)) as UserGUI;
	}


//	public void Move() {
//		if (direction == -1) {
//			moveableScript.setDestination(leftPosition);
//			direction = 1;
//		} else {
//			moveableScript.setDestination(rightPosition);
//			direction = -1;
//		}
//	}

	public Vector3 getDestination(){
		if (direction == -1) {
			return leftPosition;
		} else {
			return rightPosition;
		}
	}

	public void move() {
		if (direction == -1) {
			direction = 1;
		} else {
			direction = -1;
		}
	}

	public int getEmptyIndex() {
		for (int i = 0; i < passenger.Length; i++) {
			if (passenger [i] == null) {
				return i;
			}
		}
		return -1;
	}

	public GameObject getGameobj() {
		return this.boat;
	}

	public int get_to_or_from() { 
		return direction;
	}

	public bool isEmpty() {
		for (int i = 0; i < passenger.Length; i++) {
			if (passenger [i] != null) {
				return false;
			}
		}
		return true;
	}

	public Vector3 getEmptyPosition() {
		Vector3 pos;
		int emptyIndex = getEmptyIndex ();
		if (direction == -1) {
			pos = right_positions[emptyIndex];
		} else {
			pos = left_positions[emptyIndex];
		}
		return pos;
	}

	public void GetOnBoat(CharacterController _characterCtrl) {
		int index = getEmptyIndex ();
		passenger [index] = _characterCtrl;
	}

	public CharacterController GetOffBoat(string passenger_name) {
		for (int i = 0; i < passenger.Length; i++) {
			if (passenger [i] != null && passenger [i].getName () == passenger_name) {
				CharacterController charactorCtrl = passenger [i];
				passenger [i] = null;
				return charactorCtrl;
			}
		}
		Debug.Log ("Cant find passenger in boat: " + passenger_name);
		return null;
	}

	public int[] getCharacterNum() {
		int[] count = {0, 0};
		for (int i = 0; i < passenger.Length; i++) {
			if (passenger [i] == null)
				continue;
			if (passenger [i].getType () == 0) {    // 0->priest, 1->devil
				count[0]++;
			} else {
				count[1]++;
			}
		}
		return count;
	}

	public void Reset() {
//		moveableScript.Reset ();
//		if (direction == -1) {
//			Move ();
//		}
//		passenger = new CharacterController[2];
		if (direction == -1) {
			move ();
		}
		boat.transform.position = leftPosition;
		passenger = new CharacterController[2];
	}

}
