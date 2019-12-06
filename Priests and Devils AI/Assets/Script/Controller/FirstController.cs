using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour,ISceneController, UserAction {

	private Vector3 water_pos = new Vector3(0,0,0);

	private UserGUI userGUI;

	public CoastController leftCoast;
	public CoastController rightCoast;
	public BoatController boat;
	private CharacterController[] characters;

	private SceneActionManager actionManager;

	void Awake() {
		SSDirector director = SSDirector.getInstance ();
		director.currentSceneController = this;
		userGUI = gameObject.AddComponent <UserGUI>() as UserGUI;
		characters = new CharacterController[6];
		actionManager = gameObject.AddComponent<SceneActionManager>() as SceneActionManager;
		LoadResources ();
	}


	public void LoadResources() {
		GameObject water = Instantiate (Resources.Load ("Perfabs/Water", typeof(GameObject)), water_pos, Quaternion.identity, null) as GameObject;
		water.name = "water";

		leftCoast = new CoastController (1);
		rightCoast = new CoastController (-1);
		boat = new BoatController ();

		LoadCharacter ();
	}

	private void LoadCharacter() {
		for (int i = 0; i < 3; i++) {
			CharacterController chara = new CharacterController ("priest");
			chara.setName("priest" + i);
			chara.setPosition (leftCoast.getEmptyPosition ());
			chara.getOnCoast (leftCoast);
			leftCoast.getOnCoast (chara);

			characters [i] = chara;
		}

		for (int i = 0; i < 3; i++) {
			CharacterController chara = new CharacterController ("devil");
			chara.setName("devil" + i);
			chara.setPosition (leftCoast.getEmptyPosition ());
			chara.getOnCoast (leftCoast);
			leftCoast.getOnCoast (chara);

			characters [i+3] = chara;
		}
	}

	public void moveBoat() {
//		if (userGUI.status == 1 || userGUI.status == 2)
//			return;
		if (boat.isEmpty ())
			return;
//		boat.Move ();
//		Debug.Log(actionManager);
		actionManager.MoveBoat(boat);	
		boat.move ();	
		userGUI.status = check_game_over ();
	}

	public void characterIsClicked(CharacterController _characterCtrl) {
//		if (userGUI.status == 1 || userGUI.status == 2)
//			return;
		if (_characterCtrl.isOnBoat ()) {
			CoastController whichCoast;
			if (boat.get_to_or_from () == -1) { // to->-1; from->1
				whichCoast = rightCoast;
			} else {
				whichCoast = leftCoast;
			}

			boat.GetOffBoat (_characterCtrl.getName());
//			_characterCtrl.moveToPosition (whichCoast.getEmptyPosition ());

			actionManager.MoveCharacter (_characterCtrl, whichCoast.getEmptyPosition ());

			_characterCtrl.getOnCoast (whichCoast);
			whichCoast.getOnCoast (_characterCtrl);
	
		} else {	
			
			CoastController whichCoast = _characterCtrl.getCoastController ();

			if (boat.getEmptyIndex () == -1) {		// boat is full
				return;
			}

			if (whichCoast.get_to_or_from () != boat.get_to_or_from ())	// boat is not on the side of character
				return;
			whichCoast.getOffCoast(_characterCtrl.getName());
//			_characterCtrl.moveToPosition (boat.getEmptyPosition());
				
			actionManager.MoveCharacter (_characterCtrl,boat.getEmptyPosition());


			_characterCtrl.getOnBoat (boat);
			boat.GetOnBoat (_characterCtrl);
		}
		userGUI.status = check_game_over ();
	}

	int check_game_over() {	// 0->not finish, 1->lose, 2->win
		int from_priest = 0;
		int from_devil = 0;
		int to_priest = 0;
		int to_devil = 0;

		int[] fromCount = leftCoast.getCharacterNum ();
		from_priest += fromCount[0];
		from_devil += fromCount[1];

		int[] toCount = rightCoast.getCharacterNum ();
		to_priest += toCount[0];
		to_devil += toCount[1];

		if (to_priest + to_devil == 6)		// win
			return 2;

		int[] boatCount = boat.getCharacterNum ();
		if (boat.get_to_or_from () == -1) {	// boat at toCoast
			to_priest += boatCount[0];
			to_devil += boatCount[1];
		} else {	// boat at fromCoast
			from_priest += boatCount[0];
			from_devil += boatCount[1];
		}
		if (from_priest < from_devil && from_priest > 0) {		// lose
			return 1;
		}
		if (to_priest < to_devil && to_priest > 0) {
			return 1;
		}
		return 0;			// not finish
	}

	public void restart() {
		boat.Reset ();
		leftCoast.Reset ();
		rightCoast.Reset ();
		for (int i = 0; i < characters.Length; i++) {
			characters [i].Reset ();
		}
	}
}