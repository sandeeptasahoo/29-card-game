using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Data to be referenced")]
    [SerializeField]GameObject cardReceivingPoint;
    [SerializeField]GameObject cardHoldPoint;

    
    [Header("data shown")]
 
    [SerializeField]GameObject[] cards;
    [SerializeField]int NumberOfCards;
    
    void Awake()//Taking references
    {
        cardReceivingPoint=transform.GetChild(1).gameObject;
        cardHoldPoint=transform.GetChild(3).gameObject;
        
    }

    void SetCardsToShow()//making cards set in front of face without sprading them
    {
        Vector3 pos=new Vector3(0,0.03f,0);//giving a small offset so that card dont touch ground
        
        for(int i=0;i<NumberOfCards;i++)
        {
            cards[i].transform.localPosition=pos;
            cards[i].transform.localEulerAngles=Vector3.zero;
            pos.z-=0.002f;//layer out all the cards
        }
    }
    public void ShowCards()//Arranging and setting cards to proper position and the spading it
    {
        ArrangeCards();//arranging accoding to type and sut
        SetCardsToShow();//setting cards to proper layer before sprading
        Vector3 maxDeviationAngle=new Vector3(0,0,0);
        maxDeviationAngle.z=((float)(NumberOfCards-1)/2)*15;//giving target angle to different cards
        for(int i=0;i<NumberOfCards;i++)
        {
            cards[i].GetComponent<Cards>().showCards(maxDeviationAngle);
            maxDeviationAngle.z-=15;//angular difference between conjugate cards after sprading
        }
        
    }

    public void CloseCards()//closing cards and placing on table
    {
        for(int i=0;i<NumberOfCards;i++)
        {
            cards[i].GetComponent<Cards>().CloseCards();
        }
        StartCoroutine(PlaceCardsOnDeck(5));
    }

    IEnumerator PlaceCardsOnDeck(float time)// placing cards on table
    {
        yield return new WaitForSeconds(time);
        Vector3 pos=Vector3.zero;
        for(int i=0;i<NumberOfCards;i++)
        {
            cards[i].transform.localPosition=pos;
        }
        Vector3 faceDownAngle=new Vector3(-90,180,0); 
        for(int i=0;i<NumberOfCards;i++)
        {
            cards[i].transform.localEulerAngles= faceDownAngle;
        }
        
    }
    public void BringCloserReceivedCards()//Bringing cards to hold position
    {
        NumberOfCards=cardReceivingPoint.transform.childCount;//finding number of cards player have
        for(int i=0;i<NumberOfCards;i++)
        {
            cardReceivingPoint.transform.GetChild(0).parent=cardHoldPoint.transform;//making the player hold the cards by making cards child of hold position

        }
        
        cards=new GameObject[NumberOfCards];
        Vector3 angles=new Vector3(-90,180,0);//alligning cards in direction of player
        for(int i=0;i<NumberOfCards;i++)//setting cards allignment at new position
        {
            cards[i]=cardHoldPoint.transform.GetChild(i).gameObject;//accessing all cards in hand
            cards[i].transform.localPosition=Vector3.zero;
            cards[i].transform.localEulerAngles=angles;
            
        }
        
        
    }

    void ArrangeCards()// Arranging cards according to type and sut (used hashing method and searching for algo to optimise)
    {
        GameObject[][] AllCards=new GameObject[4][];
        for(int i=0;i<4;i++)
        {
            AllCards[i]=new GameObject[7];
        }
        for(int i=0;i<cards.Length;i++)
        {
            Cards currentcard=cards[i].GetComponent<Cards>();
            AllCards[currentcard.cardtype][currentcard.cardsut]=cards[i];
        }
        int k=0;
       
        for(int i=0;i<4;i++)
        {
            for(int j=0;j<7;j++)
            {
                if(AllCards[i][j]!=null)
                {
                    cards[k]=AllCards[i][j];
                    k++;
                }
            }
        }
    }
}
