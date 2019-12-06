using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoastController:FirstController{


	private GameObject coast;
	private Vector3 start = new Vector3(8,0.5f,0);
	private Vector3 end = new Vector3(-8,0.5f,0);
	private Vector3[] positions;
	private int direction;    // left or right;

	// change frequently
	CharacterController[] passenger;

	public CoastController(int _direction) {
		positions = new Vector3[] {new Vector3(5,2,0), new Vector3(6.2F,2,0), new Vector3(7.4F,2,0), 
			new Vector3(8.6F,2,0), new Vector3(9.8F,2,0), new Vector3(11,2,0)};

		passenger = new CharacterController[6];

		if (_direction == 1) {
			coast = Object.Instantiate (Resources.Load ("Perfabs/Stone", typeof(GameObject)), start, Quaternion.identity, null) as GameObject;
			coast.name = "left";
			direction = 1;
		} else {
			coast = Object.Instantiate (Resources.Load ("Perfabs/Stone", typeof(GameObject)), end, Quaternion.identity, null) as GameObject;
			coast.name = "right";
			direction = -1;
		}
	}

	public int get_to_or_from() {
		return direction;
	}

	public int getEmptyIndex() {
		for (int i = 0; i < passenger.Length; i++) {
			if (passenger [i] == null) {
				return i;
			}
		}
		return -1;
	}

	public Vector3 getEmptyPosition() {
		Vector3 pos = positions [getEmptyIndex ()];
		pos.x *= direction;
		return pos;
	}

	public void getOnCoast(CharacterController _characterCtrl) {
		int index = getEmptyIndex ();
		passenger [index] = _characterCtrl;
	}

	public CharacterController getOffCoast(string passenger_name) {   // 0->priest, 1->devil
		for (int i = 0; i < passenger.Length; i++) {
			if (passenger [i] != null && passenger [i].getName () == passenger_name) {
				CharacterController charactorCtrl = passenger [i];
				passenger [i] = null;
				return charactorCtrl;
			}
		}
		Debug.Log ("cant find passenger on coast: " + passenger_name);
		return null;
	}


	public int[] getCharacterNum() {
		int[] count = {0, 0};
		for (int i = 0; i < passenger.Length; i++) {
			if (passenger [i] == null)
				continue;
			if (passenger [i].getType () == 0) {  // 0->priest, 1->devil
				count[0]++;
			} else {
				count[1]++;
			}
		}
		return count;
	}

	public void Reset() {
		passenger = new CharacterController[6];
	}
}
