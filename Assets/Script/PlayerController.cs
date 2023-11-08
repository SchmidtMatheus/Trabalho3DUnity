using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum InputDirection
{
    NULL,
    Left,
    Right,
    Up,
    Down
}

public class PlayerController : MonoBehaviour {

    public float speed = 1;
    InputDirection inputDirection;
    Vector3 mousePos;
    bool activeInput;

    Position standPosition;

    Position fromPosition;
  
    Vector3 xDirection;
   
    Vector3 moveDirection;

    float jumpValue = 7;

    float gravity = 20;

 
    public bool canDoubleJump = true;

    bool doubleJump = false;


    bool isQuickMoving = false;
    float saveSpeed;
    float quickMoveDuration = 3;
    public float quickMoveTimeLeft;
    public Text statusText;

    IEnumerator quickMoveCor;
    float magnetDuration = 15;
    public float magnetTimeLeft;
    IEnumerator magnetCor;
    public GameObject MagnetCollider;


    float shoeDuration = 10;
    public float shoeTimeLeft;
    IEnumerator shoeCor;

    float multiplyDuration = 10;
    public float multiplyTimeLeft;
    IEnumerator multiplyCor;

    public static PlayerController instance;

    CharacterController characterController;

	void Start () {
        instance = this;
    
        characterController = GetComponent<CharacterController>();
       
        standPosition = Position.Middle;
        StartCoroutine(UpdateAction()); 
	}
	
    IEnumerator UpdateAction()
    {
        while (true)
        {
           // GetInputDirection(); 
            //PlayAnimation();
            MoveLeftRight();
            MoveForward();
            yield return 0;
        }
    }


    void MoveForward() 
    {
       
        if(inputDirection == InputDirection.Down)
        {
            AnimationManager.instance.animationHandler = AnimationManager.instance.PlayRoll;
        }
       
        if(characterController.isGrounded)
        {
            moveDirection = Vector3.zero;
            if(AnimationManager.instance.animationHandler != AnimationManager.instance.PlayRoll 
               && AnimationManager.instance.animationHandler != AnimationManager.instance.PlayTurnLeft
               && AnimationManager.instance.animationHandler != AnimationManager.instance.PlayTurnRight)
            {
                AnimationManager.instance.animationHandler = AnimationManager.instance.PlayRun;            
            }
            if(inputDirection == InputDirection.Up)
            {
                JumpUp();
                if (canDoubleJump)
                    doubleJump = true;
            }
        }
        else
        {   
            if(inputDirection == InputDirection.Down)
            {
                QuickGround();
            }
            if(inputDirection == InputDirection.Up)
            {
                if (doubleJump)
                {
                    JumpDouble();
                    doubleJump = false;
                }
            }
            if(AnimationManager.instance.animationHandler != AnimationManager.instance.PlayJumpUp
               && AnimationManager.instance.animationHandler != AnimationManager.instance.PlayDoubleJump
               && AnimationManager.instance.animationHandler != AnimationManager.instance.PlayRoll)
            {
                AnimationManager.instance.animationHandler = AnimationManager.instance.PlayJumpLoop;
            }
        }
        
    }

    void QuickGround()
    {
        moveDirection.y -= jumpValue * 3;
    }


    void JumpDouble()
    {
        AnimationManager.instance.animationHandler = AnimationManager.instance.PlayDoubleJump;
        moveDirection.y += jumpValue * 1.3f;
    }


    void JumpUp()
    {
        AnimationManager.instance.animationHandler = AnimationManager.instance.PlayJumpUp;
        moveDirection.y += jumpValue;

    }


    void MoveLeft()
    {
        if(standPosition != Position.Left)
        {
            GetComponent<Animation>().Stop();
            AnimationManager.instance.animationHandler = AnimationManager.instance.PlayTurnLeft;

            xDirection = Vector3.left;

            if(standPosition == Position.Middle)
            {
                standPosition = Position.Left;
                fromPosition = Position.Middle;
            }
            else if (standPosition == Position.Right)
            {
                standPosition = Position.Middle;
                fromPosition = Position.Right;
            }

        }
    }

    void MoveRight()
    {
        if (standPosition != Position.Right)
        {
            GetComponent<Animation>().Stop();
            AnimationManager.instance.animationHandler = AnimationManager.instance.PlayTurnRight;

            xDirection = Vector3.right;

            if (standPosition == Position.Middle)
            {
                standPosition = Position.Right;
                fromPosition = Position.Middle;
            }
            else if (standPosition == Position.Left)
            {
                standPosition = Position.Middle;
                fromPosition = Position.Left;
            }

        }
    }
void MoveLeftRight()
{
    if (standPosition == Position.Left)
    {
        xDirection = Vector3.left;
        if (transform.position.x <= -1.7f)
        {
            xDirection = Vector3.zero;
            transform.position = new Vector3(-1.7f, transform.position.y, transform.position.z);
            standPosition = Position.Middle; // Ao atingir o limite esquerdo, ajusta para a faixa do meio
        }
    }
    else if (standPosition == Position.Right)
    {
        xDirection = Vector3.right;
        if (transform.position.x >= 1.7f)
        {
            xDirection = Vector3.zero;
            transform.position = new Vector3(1.7f, transform.position.y, transform.position.z);
            standPosition = Position.Middle; // Ao atingir o limite direito, ajusta para a faixa do meio
        }
    }
    else
    {
        // Se já está na faixa do meio, mantenha a posição x no meio (0)
        xDirection = Vector3.zero;
    }
}





    void PlayAnimation()
    {
        if (inputDirection == InputDirection.Left)
        {
            AnimationManager.instance.animationHandler = AnimationManager.instance.PlayTurnLeft;
        }
        else if (inputDirection == InputDirection.Right)
        {
            AnimationManager.instance.animationHandler = AnimationManager.instance.PlayTurnRight;
        }
        else if(inputDirection == InputDirection.Up)
        {
            AnimationManager.instance.animationHandler = AnimationManager.instance.PlayJumpUp;
        }
        else if(inputDirection == InputDirection.Down)
        {
            AnimationManager.instance.animationHandler = AnimationManager.instance.PlayRoll;
        }


    }

    public void QuickMove()
    {
        if (quickMoveCor != null)
            StopCoroutine(quickMoveCor);
        quickMoveCor = QuickMoveCoroutine();
        StartCoroutine(quickMoveCor);
    }

    IEnumerator QuickMoveCoroutine()
    {
        quickMoveTimeLeft = quickMoveDuration;
        if(!isQuickMoving)
            saveSpeed = speed;
        speed = 20;
        isQuickMoving = true;
        while(quickMoveTimeLeft >= 0)
        {
            quickMoveTimeLeft -= Time.deltaTime;
            yield return null;
        }
        speed = saveSpeed;
        isQuickMoving = false;
    }



    public void UseMagnet()
    {
        if (magnetCor != null)
            StopCoroutine(magnetCor);
        magnetCor = MagnetCoroutine();
        StartCoroutine(magnetCor);
    }

    IEnumerator MagnetCoroutine()
    {
        magnetTimeLeft = magnetDuration;
        MagnetCollider.SetActive(true);
        while(magnetTimeLeft >= 0)
        {
            magnetTimeLeft -= Time.deltaTime;
            yield return null;
        }
        MagnetCollider.SetActive(false);

    }

    public void UseShoe()
    {
        if (shoeCor != null)
            StopCoroutine(shoeCor);
        shoeCor = ShoeCoroutine();
        StartCoroutine(shoeCor);

    }
    
    

    IEnumerator ShoeCoroutine()
    {
        shoeTimeLeft = shoeDuration;
        PlayerController.instance.canDoubleJump = true;
        while(shoeTimeLeft >= 0)
        {
            shoeTimeLeft -= Time.deltaTime;
            yield return null;
        }
        PlayerController.instance.canDoubleJump = false;
        
    }
    public void Multiply()
    {
        if (multiplyCor != null)
            StopCoroutine(multiplyCor);
        multiplyCor = MultiplyCoroutine();
        StartCoroutine(multiplyCor);
    }
    IEnumerator MultiplyCoroutine()
    {
        multiplyTimeLeft = multiplyDuration;
        GameAttribute.instance.multiply = 2;
        while(multiplyTimeLeft >= 0)
        {
            multiplyTimeLeft -= Time.deltaTime;
            yield return null;
        }
        GameAttribute.instance.multiply = 1;
    }

	void Update()
{
    moveDirection.z = speed;
    moveDirection.y -= gravity * Time.deltaTime;

    // Define a direção com base nas teclas pressionadas
    if (Input.GetKey(KeyCode.W))
    {
        inputDirection = InputDirection.Up;
        MoveForward();
    }
    else if (Input.GetKey(KeyCode.S))
    {
        inputDirection = InputDirection.Down;
        MoveForward();
    }
    else if (Input.GetKey(KeyCode.A))
    {
        inputDirection = InputDirection.Left;
        MoveLeft();
    }
    else if (Input.GetKey(KeyCode.D))
    {
        inputDirection = InputDirection.Right;
        MoveRight();
    }

    // Atualize a posição do personagem com base na direção definida
    characterController.Move((xDirection * 5 + moveDirection) * Time.deltaTime);
    statusText.text = GetTime(multiplyTimeLeft);
}


    private string GetTime(float time)
    {
        if (time <= 0)
            return "0";
        return ((int)time + 1).ToString();
    }
}

public enum Position
{
    Left,
    Middle,
    Right
}





