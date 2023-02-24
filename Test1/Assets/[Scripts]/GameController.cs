using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class GameController : MonoBehaviour
{

    //Singleton instance of this controller. Multiples will glitch the system.
    public static GameController Instance { get; private set; }

    public List<Transform> twoByFourLayout;
    public List<Transform> fourByFourLayout;
    public List<Transform> sixBySixLayout;

    public StandardDeck deck;

    //For match attempts
    private GameObject selectedCard1, selectedCard2;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        deck = new StandardDeck(); // example of composition
    }

    //Delay variable to wait for timer
    public bool beingDelayed = false;

    //Fail logic and win logic counters
    public int currentFails = 0, maxFails = 3;
    public int currentMatches = 0, maxMatches = 0;

    public void SelectCard(GameObject cardGameObject)
    {
        if (selectedCard1 == null)
        {
            selectedCard1 = cardGameObject;
        }
        else
        {
            selectedCard2 = cardGameObject;
            CheckChosenForMatch();
        }
    }

    public void CheckChosenForMatch()
    {
        if (selectedCard1.name == selectedCard2.name)
        {
            //deselect the cards, we found a match!
            selectedCard1 = null;
            selectedCard2 = null;

            //Add one to the score
            currentMatches++;

            //Play match sfx
            AudioController.Instance.PlaySound(CLIPS.MATCH);

            //Check for win
            if (currentMatches >= maxMatches)
            {
                beingDelayed = true;
                UIController.Instance.SetOutcome(true);
            }

            //Update score UI;
            UIController.Instance.UpdateScoreUI();
        }
        else
        {
            //Set being delayed, this will allow 3 seconds for the user to see the cards.
            beingDelayed = true;

            //Add one to current fails and check if we are over the maximum
            currentFails++;
            if (currentFails >= maxFails)
            {
                UIController.Instance.SetOutcome(false);
                currentFails = 0;
                maxFails = 3;
            }
            else
            {
                //Tell the player they flipped wrong and wait 3 seconds to flip them back.
                AudioController.Instance.PlaySound(CLIPS.MATCH_FAIL);
                Invoke("waitReset", 3);
            }
        }
    }
    //This flips all cards back, resets all selected cards, and turns off the delay
    public void waitReset()
    {
        selectedCard1.GetComponent<Card>().Flip();
        selectedCard2.GetComponent<Card>().Flip();
        selectedCard1 = null;
        selectedCard2 = null;
        beingDelayed = false;
    }
}
