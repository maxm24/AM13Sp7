using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchTrigger : MonoBehaviour
{
    public CharacterController PlayerHeight;
    public CapsuleCollider playerCol;
    public float normalHeight, crouchHeight;
    public Transform player;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            PlayerHeight.height = crouchHeight;
            playerCol.height = crouchHeight;
        }
        if(Input.GetKeyUp(KeyCode.C))
        {
            PlayerHeight.height = normalHeight;
            playerCol.height = normalHeight;
            player.position = player.position + offset;
        }
    }
}
