using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

[RequireComponent(typeof(MainScript))]
public class CharacterMovementScript : MonoBehaviour
{
    private MainScript mainScriptComponent;

    public Material MinerAnimationMaterial;
    public Texture2D MinerIdleTexture;
    public Texture2D MinerMoveTexture;
    
    private CollisionUtility collisionUtility;

    [Range(1, 10)]
    public int CAMERA_SPEED = 5;
    [Range(1, 60)]
    public int JUMP_SPEED = 30;

    public bool ShowCollisionBox;
    
    public float g = 9.81f;
    private float vertSpeed;
    private bool canJump;
    public float lastMoveDirection = 1;
    private float speedUp;

    void Start()
    {
        mainScriptComponent = GetComponent<MainScript>();

        collisionUtility = new CollisionUtility(mainScriptComponent);
    }
    
    void Update()
    {
        speedUp = Input.GetButton("Fire3")? 2 : 1;

        int xMove = Mathf.RoundToInt(Input.GetAxis("Horizontal") * CAMERA_SPEED * speedUp);
        int jumpMove = Mathf.RoundToInt(((Input.GetButton("Jump") && canJump) ? 1 : 0) * JUMP_SPEED / Time.deltaTime);

        MinerAnimationMaterial.SetFloat("_MoveSpeed", xMove);
        if (Mathf.Abs(xMove) > 0)
        {
            lastMoveDirection = Mathf.Sign(xMove);
        }
        if (transform.position.y < -mainScriptComponent.mainTexture.height)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + mainScriptComponent.mainTexture.height * 2, transform.position.z);
        }
        
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (mainScriptComponent.MAP_SCALING == 4)
            {
                mainScriptComponent.MAP_SCALING = 5;
            }
            else if (mainScriptComponent.MAP_SCALING == 5)
            {
                mainScriptComponent.MAP_SCALING = 4;
            }
        }

        MinerAnimationMaterial.SetFloat("_MoveDirection", lastMoveDirection * speedUp);
        PhysicMove(-xMove, jumpMove - g * 8);
    }

    private void PhysicMove(float sideMove, float upDownMove)
    {
        vertSpeed += upDownMove * Time.deltaTime / 2;
        AttemptMove(new Vector2(sideMove * Time.deltaTime, 0));
        canJump = false;
        if (!AttemptMove(new Vector2(0, vertSpeed * Time.deltaTime)))
        {
            if (vertSpeed <= 0) canJump = true;
            vertSpeed = 0;
        }
    }

    private bool AttemptMove(Vector2 moveVector)
    {
        (var collisionType, var collisionAmount) = collisionUtility.DetectCollision(transform.position + new Vector3(moveVector.x, moveVector.y, 0));
        switch (collisionType)
        {
            case PixelMovement.EMPTY:
                transform.position += new Vector3(moveVector.x, moveVector.y, 0);
                return true;
            case PixelMovement.LIQUID:
                transform.position += new Vector3(moveVector.x, moveVector.y, 0) * (1 - 0.5f * collisionAmount);
                return true;
            case PixelMovement.LADDER:
                if (moveVector.y >= 0)
                {
                    transform.position += new Vector3(moveVector.x, moveVector.y, 0);
                    return true;
                }
                else if (Input.GetAxis("Vertical") < 0)
                {
                    transform.position += new Vector3(moveVector.x, Mathf.Max(-speedUp, moveVector.y), 0);
                    return true;
                }
                return false;
        }
        return false;
    }

    public List<(Texture2D, Rect)> GetCollisionDebug() => collisionUtility.GetCollisionDebug();
}
