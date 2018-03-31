using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaPlayermove : MonoBehaviour {

    public bool isPlayerOne = true;
    Transform areaTransform;
    public float initPositionY, finPositionY, cursorVel; // Screen end points and velocity of cursor/
                                                                                      // Use this for initialization
    void Start () {
        areaTransform = gameObject.GetComponent<Transform>();

    }
	
	// Update is called once per frame
	void Update () {
        // Get values from horizontal and vertical inputs from joysticks, verification why joystick to get, player one or two
        float moveVertical = (isPlayerOne) ? Input.GetAxisRaw("VerticalDireitoPlayer1") : Input.GetAxisRaw("VerticalDireitoPlayer2");

        // Verify if cursor is in end of the screen. stop movement to position.
        if (areaTransform.position.y <= initPositionY && moveVertical < 0 || areaTransform.position.y >= finPositionY && moveVertical > 0)
        {
            moveVertical = 0;
        }
        //Move the cursor.
        Vector2 movement = new Vector2(0, moveVertical);
        areaTransform.Translate(movement * cursorVel * Time.deltaTime);
    }
}
