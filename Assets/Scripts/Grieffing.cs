﻿using UnityEngine;
using System.Collections;

public class Grieffing : MonoBehaviour {
	public KeyCode attackKey;
	public float stonedTime = .5f;
	public float reloadTime = .5f;

	float currentStonedTime = 0;
	float currentReloadTime = 0;

	void Update() {
		if (currentReloadTime >= 0) {
			currentReloadTime -= Time.deltaTime;
		}

		if (currentStonedTime >= 0) {
			currentStonedTime -= Time.deltaTime;
			if (currentStonedTime < 0) {
				GetComponent<Movement> ().enabled = true;
			}
		}

	}

	void OnTriggerStay2D(Collider2D collider) {
		Grieffing grieffing = collider.gameObject.GetComponent<Grieffing> ();
		if (grieffing && Input.GetKey(attackKey) && currentReloadTime < 0) {
			grieffing.hurt (stonedTime);
		}
	}

	void hurt(float time) {
		currentStonedTime = time;
		GetComponent<Movement> ().enabled = false;
	}
}