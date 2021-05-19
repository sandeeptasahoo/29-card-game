
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cards : MonoBehaviour
{
    
    [Header("Value shown")]
    [SerializeField]GameObject centerpot;
    [SerializeField]GameManager gameManager;
    [SerializeField] int CardPoint;
    [SerializeField]Sprite CardSprite;
    [SerializeField]public int cardtype;
    [SerializeField]public int cardsut;
    [SerializeField]Transform target;
    [SerializeField]Vector3 targetPosition;
    [SerializeField]Vector3 targetAngles;
    [SerializeField]float CardThrowSpeed;
    float cardAngularVelosity;
    Vector3 lastAngleValue;
    bool makechild;
    bool setPosition;
    bool setAngle;
    bool displayOnPlayerCanva;
    // Start is called before the first frame update
    void Awake()
    {
        //taking reference to objects
        centerpot=GameObject.FindGameObjectWithTag("Centerpot");
        gameManager=FindObjectOfType<GameManager>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(setPosition)// if position of card need to be change
        {
            if(transform.position!=targetPosition)// if position of card reach to targeted point
            {
                transform.position=Vector3.MoveTowards(transform.position,targetPosition,CardThrowSpeed*Time.deltaTime);// moving towards the target
            }
            else
            {
                setPosition=false;// stop changing target
            }
            
        }
        if(setAngle)//if angle is to be change
        {
            if(transform.localEulerAngles!=lastAngleValue)//if there is no change in angle as the object achieved targeted angle
            {
                //Debug.Log("angle rotating to "+transform.localEulerAngles);
                lastAngleValue=transform.localEulerAngles;//Assigning a value to check is there any change to the position from last update
                transform.localEulerAngles=Vector3.MoveTowards(transform.localEulerAngles,targetAngles,cardAngularVelosity*Time.deltaTime);//changing angle
                
            }
            else
            {
                setAngle=false;//stop changing angle
            }

        }
        if(!setAngle && !setPosition)//if both angle change and position change stops then close update function 
        {
            this.enabled=false;
            AfterCommand();//before closing do required work(can be also done by OnDisable())
        }
        
    }

    public void showCards(Vector3 angle)//setting cards for show
    {
        //Debug.Log("angle "+angle);
        targetAngles=angle;//giving target angle at which the card will be placed finally
        cardAngularVelosity=10;//angle rotation speed
        //Debug.Log(targetAngles);
        if(angle.z<0)//setting angle to a valid angle so that card choose shorted execution direction rather taking a longer one
        {
            transform.localEulerAngles=new Vector3(0,0,359f);
            targetAngles.z+=360;
        }
        lastAngleValue=targetAngles;//setting for checking change in angle
        setAngle=true;//activating angle change command in update
        this.enabled=true;
    }
    public void CloseCards()// setting target angles for card to close 
    {
        Debug.Log("closing card");
        
        if(transform.localEulerAngles.z>180)//if card is on right that should move towards left not right
        {
            targetAngles=new Vector3(0,0,359.99f);
        }
        else
        {
            targetAngles=Vector3.zero;//when all cards close the cand angle should be zero
        }
        cardAngularVelosity=20;
        lastAngleValue=targetAngles;
        setAngle=true;
        this.enabled=true;
    }
    public void SetTrow(Transform TargetPosition,float Speed)//it sets target position of the card where the card will be thrown to
    {
        target=TargetPosition;
        targetPosition=TargetPosition.transform.position;
        CardThrowSpeed=Speed;
        setPosition=true;
        makechild=true;// making child to target game object
        this.enabled=true;
        
    }


    public void throwcardTocenter()//throw set to center of the table
    {
        SetTrow(centerpot.transform,1);
        displayOnPlayerCanva=true;
        Vector3 angles=new Vector3(90,180,180);
        transform.localEulerAngles=angles;
    }

   
    public void setData (Sprite sprite,int point,int type,int sut)//set required datas for card
    {
        CardSprite=sprite;
        CardPoint=point;
        cardtype=type;
        cardsut=sut;
    }

    void AfterCommand()//does the same as Disable()
    {
        if(makechild)
        {
            transform.parent=target;
            transform.localPosition=Vector3.zero;
            makechild=false;
        }
        if(displayOnPlayerCanva)//displaying the cards in centralpot on canva 
        {
            gameManager.displayCardOnCanva(CardSprite);
            displayOnPlayerCanva=false;
        }
    }
}
