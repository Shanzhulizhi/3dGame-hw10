using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneController {
	void LoadResources ();
}

public interface UserAction {
	void moveBoat();
	void characterIsClicked(CharacterController characterCtrl);
	void restart();
}