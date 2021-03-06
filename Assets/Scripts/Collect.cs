﻿using UnityEngine;
using System.Collections;

public class Collect : MonoBehaviour {
	public KeyCode passif = KeyCode.A;
	public KeyCode actif = KeyCode.E;
	public int joystick;
	public AudioClip clip;

	AudioSource source;
	GameObject obj = null;
	bool changed = false;

	Vector3 lastPos;

	void Awake() {
		source = GetComponent<AudioSource> ();
	}

	void FixedUpdate() {
		if (obj) {
			obj.transform.position = new Vector3(transform.position.x, transform.position.y, obj.transform.position.z);
		}
	}

	void Update() {
		if (!changed && obj) {
			//On le pose
			if (Input.GetKeyDown (passif) || Input.GetButtonDown("P" + joystick.ToString())) {
				obj.GetComponent<Collectable> ().owner = null;
				obj = null;
			}
			//On le jete
			if (Input.GetKeyDown (actif) || Input.GetButtonDown("A" + joystick.ToString())) {
				source.clip = clip;
				source.Play ();
				Vector3 moving = transform.position - lastPos;
				obj.GetComponent<Collectable> ().Throw (moving.x > 0, moving.x < 0, moving.y > 0, moving.y < 0, 1);
				obj = null;
			}
		} else {
			changed = false;
		}

		lastPos = transform.position;
	}

	void OnTriggerEnter2D(Collider2D collider) {
		Collectable collectable = collider.gameObject.GetComponent<Collectable> ();

		//Si c'est un objet collectable et qu'on porte rien
		if (collectable) {
			if (gameObject.GetComponent<ShortLife> () && collectable.eatPoint > 0) {
				gameObject.GetComponent<Score> ().addScore(collectable.eatPoint);
				Destroy(collider.gameObject);
			}
			//Si auto collect
			else if ((collectable.autoCollect || Input.GetKey (passif)) || Input.GetAxis("P" + joystick.ToString()) > 0 && collectable.isCollectable()) {
				collectObject (collider.gameObject);
			}
		}
	}

	void OnTriggerStay2D(Collider2D collider) {
		Collectable collectable = collider.gameObject.GetComponent<Collectable> ();
		if (collectable && (Input.GetKeyDown (passif) || Input.GetAxis("P" + joystick.ToString()) > 0) && collectable.isCollectable()) {
			collectObject (collider.gameObject);
		}
	}

	void collectObject(GameObject collectingObj) {
		if (!obj) {
			obj = collectingObj.gameObject;
			obj.GetComponent<Collectable> ().owner = gameObject;
			changed = true;
		}
	}
}
