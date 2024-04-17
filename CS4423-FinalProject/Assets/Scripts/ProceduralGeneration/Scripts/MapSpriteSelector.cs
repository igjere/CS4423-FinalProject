using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpriteSelector : MonoBehaviour {
	
	public Sprite 	spU, spD, spR, spL,
			spUD, spRL, spUR, spUL, spDR, spDL,
			spULD, spRUL, spDRU, spLDR, spUDRL;
	public bool up, down, left, right;
	public int type; // 0: normal, 1: enter
	public Color normalColor, enterColor, currentRoomPlayerColor, bossRoomColor, itemRoomColor, shopRoomColor;
	Color mainColor;
	public SpriteRenderer rend;
	void Awake() {
    	InitializeRenderer();
	}
	private void InitializeRenderer() {
		if (rend == null) {
			rend = GetComponent<SpriteRenderer>();
		}
	}
	void Start () {
		rend = GetComponent<SpriteRenderer>();
		mainColor = normalColor;
		PickSprite();
		PickColor();
	}
	void PickSprite(){ //picks correct sprite based on the four door bools
		if (up){
			if (down){
				if (right){
					if (left){
						rend.sprite = spUDRL;
					}else{
						rend.sprite = spDRU;
					}
				}else if (left){
					rend.sprite = spULD;
				}else{
					rend.sprite = spUD;
				}
			}else{
				if (right){
					if (left){
						rend.sprite = spRUL;
					}else{
						rend.sprite = spUR;
					}
				}else if (left){
					rend.sprite = spUL;
				}else{
					rend.sprite = spU;
				}
			}
			return;
		}
		if (down){
			if (right){
				if(left){
					rend.sprite = spLDR;
				}else{
					rend.sprite = spDR;
				}
			}else if (left){
				rend.sprite = spDL;
			}else{
				rend.sprite = spD;
			}
			return;
		}
		if (right){
			if (left){
				rend.sprite = spRL;
			}else{
				rend.sprite = spR;
			}
		}else{
			rend.sprite = spL;
		}
	}

	void PickColor(){ //changes color based on what type the room is
		if (rend == null){
			Debug.Log("rend null"); 
			return;
		}
		if (type == 0){
			mainColor = normalColor;
			rend.color = mainColor;
		}else if (type == 1){
			mainColor = enterColor;
			rend.color = mainColor;
		}
		else if(type == 2){
			mainColor = currentRoomPlayerColor;
			rend.color = mainColor;
			rend.color = mainColor;
			rend.sortingOrder = 1;
		}

		else if (type == 3){
			mainColor = itemRoomColor;
			rend.color = mainColor;
		}
		else if (type == 4){
			mainColor = shopRoomColor;
			rend.color = mainColor;
		}
		else if (type == 6){
			mainColor = bossRoomColor;
			rend.color = mainColor;
		}
		
	}
	public void UpdateColor() {
    	PickColor(); // Call this to update the color based on the current type
	}

	public void SetCurrentRoomColor() {
        // Set the color of the current room to something distinct
        rend.color = currentRoomPlayerColor;
    }

    public void ResetColor() {
        // Reset the color back to its original state based on the room type
        PickColor(); // This will re-apply the original color based on room type
    }
}