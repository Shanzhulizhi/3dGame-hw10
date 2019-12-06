using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour {

	public float speed = 20;

	private int status; 
	private Vector3 dest;
	private Vector3 middle;

	void Update() {
		if (status == 1) {
			transform.position = Vector3.MoveTowards (transform.position, middle, speed * Time.deltaTime);
			if (transform.position == middle) {
				status = 2;
			}
		} else if (status == 2) {
			transform.position = Vector3.MoveTowards (transform.position, dest, speed * Time.deltaTime);
			if (transform.position == dest) {
				status = 0;
			}
		}
	}
	public void setDestination(Vector3 _posi) {
		dest = _posi;
		middle = _posi;
		if (_posi.y == transform.position.y) //the character is on the boat
			status = 2;
		else if (_posi.y < transform.position.y) 
			middle.y = transform.position.y;  //get aboard
		else                             
			middle.x = transform.position.x; //off aboard
		status = 1;
	}

	public void Reset() {
		status = 0;
	}

}
