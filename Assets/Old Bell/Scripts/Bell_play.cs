using UnityEngine;
using System.Collections;

public class Bell_play : MonoBehaviour {
public Texture2D myCursor; // cursor texture
public Animator myBell; // Animation Bell
static int id = 0; 
int cursorSizeX = 48;  // set to width of your cursor texture
int cursorSizeY = 48;  // set to height of your cursor texture
bool condition = true;
public AudioClip bell_audio;

void Start (){
		
		myBell = GetComponent<Animator>();
	}

void OnMouseEnter (){
	condition = false;
	//show the cursor when it is detected
	//Screen.showCursor = true;
}

void OnMouseExit (){
	condition = true;
	//hide cursor
   	//Screen.showCursor = false;
}

void OnGUI (){
    // display custom cursor
	if(!condition){
    GUI.DrawTexture ( new Rect(Input.mousePosition.x-cursorSizeX/2+ cursorSizeX/2, (Screen.height-Input.mousePosition.y)-cursorSizeY/2+ cursorSizeY/2, cursorSizeX, cursorSizeY),myCursor);
    }
}

void OnMouseDown (){

	id++;
		
		 		
	if (id == 1 ){
			// Play the bell
			myBell.SetInteger("idBell", id);
			GetComponent<AudioSource>().Play();
			
		 }
				 
		 else if (id == 2){
			// Stop the animation 
			id = 0;
			myBell.SetInteger("idBell", id);
			GetComponent<AudioSource>().Stop();
			
			}
	}
		

}