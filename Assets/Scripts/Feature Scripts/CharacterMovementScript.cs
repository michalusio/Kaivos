using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MainScript))]
public class CharacterMovementScript : MonoBehaviour
{
    private MainScript mainScriptComponent;

    public Material MinerAnimationMaterial;
    public Texture2D MinerIdleTexture;
    public Texture2D MinerMoveTexture;
    
    private Texture2D collisionTexture;
    private int[] collisionArray = new int[8];
    private readonly Rect[] collisionRects = new Rect[8];

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

        for (int i = 0; i < 8; i++)
        {
            collisionRects[i] = new Rect((i % 2) * 16, ((7 - i) / 2) * 16, 16, 16);
        }

        collisionTexture = new Texture2D(2, 4, TextureFormat.RGBAFloat, false, false);
        collisionTexture.Apply();
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
        (var collisionType, var collisionAmount) = DetectCollision(transform.position + new Vector3(moveVector.x, moveVector.y, 0));
        switch (collisionType)
        {
            case 0:
                transform.position += new Vector3(moveVector.x, moveVector.y, 0);
                return true;
            case 1:
                transform.position += new Vector3(moveVector.x, moveVector.y, 0) * (1 - 0.5f * collisionAmount);
                return true;
            case 2:
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

    private (int, float) DetectCollision(Vector3 newPosition)
    {
        collisionArray = GetFromTexture(new Vector2(mainScriptComponent.mainTexture.width / 2 - newPosition.x - 0.5f, mainScriptComponent.mainTexture.height / 2 - newPosition.y - 1));
        
        return (collisionArray.Max(), collisionArray.Count(c => c == 1) / 8f);
    }

    private int[] GetFromTexture(Vector2 position)
    {
        var rectReadTexture = new Rect(position, new Vector2(2, 4));
        
        RenderTexture.active = mainScriptComponent.mainTexturePrevFrame;
        
        collisionTexture.ReadPixels(rectReadTexture, 0, 0);
        var pixels = collisionTexture.GetPixels();
        GL.Flush();
        RenderTexture.active = null;

        return pixels.Select(p => DecodePixel(p)).ToArray();
    }

    private int DecodePixel(Color p)
    {
        if (p.a < 0.5f) return 0;//empty
        if (p.a == 1 && p.g == 0.6f && p.b == 0.6f) return 0;//belts
        if (p.a == 1 && p.g == 0.1f && p.b == 0.1f) return 0;//shop
        if (p.a == 1 && p.g == 0.5f && p.b == 0.5f) return 0;//mined
        if (p.a == 1 && p.r == 0.1f && p.g == 0.3f && p.b == 0.3f) return 0;//torch
        if (p.a == 1 && p.g == 0.7f && p.b == 0.7f) return 1;//liquid
        if (p.a == 1 && p.r == 0 && p.g == 0.3f && p.b == 0.3f) return 2;//ladder
        return 3;
    }

    public List<(Texture2D, Rect)> GetCollisionDebug()
    {
        return collisionArray.Select(c => c == 0 ? Texture2D.whiteTexture : Texture2D.normalTexture).Zip(collisionRects, (a, b) => (a, b)).ToList();
    }
}
