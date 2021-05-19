using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header ("Attach Reference")]
    [SerializeField]GameObject cardforcanvaPrefab;
    [SerializeField]GameObject centerPotCanva;
    [SerializeField]GameObject centralpot;
    [SerializeField]GameObject CardPrefab;
    [SerializeField]Sprite[] HeartCard;
    [SerializeField]Sprite[] ClubsCard;
    [SerializeField]Sprite[] DimondCards;
    [SerializeField]Sprite[] SpadeCards;
    [SerializeField]Sprite[][] CardNumberSprites;
    [SerializeField]int[] CardPoints;
    [SerializeField]float AngularDisplayOfCards;
    [SerializeField]float CardThrowSpeed;
    [SerializeField]float CardThrowInterval;
    [Header("Data For Visualise")]
    [SerializeField]bool[] CardDistributed;
    [SerializeField]GameObject[] Players;
    int cardtype;
    int cardsut;
    
    // Start is called before the first frame update

    void Awake()//Taking References
    {
        AssignCards();
        CardDistributed=new bool[28];//evaluating use of card which is initially all false as no card till now is not used
        for(int i=0;i<CardDistributed.Length;i++)
        {
            CardDistributed[i]=false;
        }
        AssignPlayers();
    }
    void Start()
    {
        
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }

    void EvaluateWinner()
    {
        Debug.Log("evaluate winner");
    }

    void AssignCards()//Acceessing all card sprites of different type
    {
        CardNumberSprites=new Sprite[4][];
        CardNumberSprites[0]=HeartCard;
        CardNumberSprites[1]=ClubsCard;
        CardNumberSprites[2]=DimondCards;
        CardNumberSprites[3]=SpadeCards;
    }
    void AssignPlayers()//Accessing all players with their different tag in order to navigate between them in order
    {
        Players=new GameObject[4];
        for(int i=1;i<=4;i++)
        {
            Players[i-1]=GameObject.FindGameObjectWithTag("Player"+i);
        }
    }
    IEnumerator DistributeCardToAllPlayer(int NumberOfCards,int Distributor)
    {
        //Debug.Log("card set start");
        float time=0;//initial time set for give time buffer for card position change
        Vector3 offset=new Vector3(0,0.001f,0);//initial distributor card stack having space between cards 
        Quaternion InitialcardAngle=Quaternion.Euler(-90,180,(Distributor)*90);//rotation angle of cards initially according to distributor
        
        for(int i=0;i<NumberOfCards;i++)
        {
            for(int j=0;j<4;j++)
            {
                //Debug.Log("card "+j+" is set");
                GameObject card=Instantiate(CardPrefab,Players[Distributor].transform.GetChild(2).position-(((i*4)+j)*offset),InitialcardAngle);//cards are instatiated at distribution point
                SelectCard();//selecting a card with cardtype and card sute
                card.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite=CardNumberSprites[cardtype][cardsut];//assigning card a  sprite of  required type and sut
                card.GetComponent<Cards>().setData(CardNumberSprites[cardtype][cardsut],CardPoints[cardsut],cardtype,cardsut);//setting required data to a perticular card
                int receiver=MapPlayer(Distributor+j+1);//maping the player with proper index limit
                time+=CardThrowInterval;//assigning a time after which the card will start moving
                StartCoroutine(setTrow(card,Players[receiver].transform.GetChild(1),CardThrowSpeed,time));
               //calling a fuction which will throw the card to a required position
            }
            
        }
        //Debug.Log("cards are set");
        yield return new WaitForSeconds(time+CardThrowInterval);
        for(int i=0;i<4;i++)
        {
            //Debug.Log("bring card closer");
            Players[i].GetComponent<PlayerManager>().BringCloserReceivedCards();
            
        }
        
    }

    IEnumerator setTrow(GameObject card,Transform receiver,float cardThroeSpeed,float timegap)//Setting destination and all for throw
    {
        yield return new WaitForSeconds(timegap);
         card.GetComponent<Cards>().SetTrow(receiver,CardThrowSpeed);
    }

    public void distributecardbutton()//distribute cards to all player
    {
        //Debug.Log("distribution call");
        StartCoroutine(DistributeCardToAllPlayer(4,1));
       // Debug.Log("call successful");
    }

    public void ShowcardsToAllPlayer()//Show cards to all players
    {
        for(int i=0;i<4;i++)
        {
            Players[i].GetComponent<PlayerManager>().ShowCards();
        }
    }

    int MapPlayer(int index)//limiting the player index to certain extent
    {
        if(index>3)
        {
            index=index-4;
        }
        return index;
    }

    void SelectCard()//selecting a random card with random type and sut which has not been used till now
    {
        cardtype=Random.Range(0,4);
        cardsut=Random.Range(0,7);
        int cardnumber=(cardtype*7)+cardsut;
        while(CardDistributed[cardnumber])
        {
            cardtype=Random.Range(0,4);
            cardsut=Random.Range(0,7);
            cardnumber=(cardtype*7)+cardsut;
            
        }
        CardDistributed[cardnumber]=true;
    }

    public void displayCardOnCanva(Sprite cardSprite)// displaying card on canva after card reaches to centralpot
    {
        centerPotCanva.SetActive(true);
        GameObject card=Instantiate(cardforcanvaPrefab,centerPotCanva.transform);
        card.GetComponent<Image>().sprite=cardSprite;
        if(centerPotCanva.transform.childCount==4)// after all 4 player have thrown their cards in pot
        {
            EvaluateWinner();
            StartCoroutine(destroyAllcards(centerPotCanva,2));
            StartCoroutine(destroyAllcards(centralpot,2));
        }

    }

    IEnumerator destroyAllcards(GameObject parent,float time)// destroying all child object from parent
    {
        yield return new WaitForSeconds(time);
        int n=parent.transform.childCount;
        for(int i=0;i<n;i++)
        {

            Destroy(parent.transform.GetChild(i).gameObject);
        }
    }

}
