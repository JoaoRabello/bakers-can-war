using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatches : MonoBehaviour
{
    private Board board;
    public List<GameObject> currentMatches = new List<GameObject>();

    void Start()
    {
        board = FindObjectOfType<Board>();
    }

    public void FindAllMatches(){
        StartCoroutine(FindAllMatchesCo());
    }

    private IEnumerator FindAllMatchesCo(){
        yield return new WaitForSeconds(.2f);
        for(int i = 0; i < board.width; i++){
            for(int j = 0; j < board.height; j++){
                GameObject currentTopping = board.allToppings[i, j];
                if(currentTopping != null){
                    if(i > 0 && i < board.width - 1){
                        GameObject leftTopping = board.allToppings[i - 1, j];
                        GameObject rightTopping = board.allToppings[i + 1, j];
                        if(leftTopping != null && rightTopping != null){
                            if(leftTopping.tag == currentTopping.tag && rightTopping.tag == currentTopping.tag){
                                if(DetectMovement(currentTopping, leftTopping, rightTopping)){
                                    if(!currentMatches.Contains(leftTopping)){
                                        currentMatches.Add(leftTopping);
                                    }
                                    leftTopping.GetComponent<Topping>().isMatched = true;
                                    if(!currentMatches.Contains(rightTopping)){
                                        currentMatches.Add(rightTopping);
                                    }
                                    rightTopping.GetComponent<Topping>().isMatched = true;
                                    if(!currentMatches.Contains(currentTopping)){
                                        currentMatches.Add(currentTopping);
                                    }
                                    currentTopping.GetComponent<Topping>().isMatched = true;
                                }
                                else{
                                    currentTopping.GetComponent<Topping>().willMatch = true;
                                    leftTopping.GetComponent<Topping>().willMatch = true;
                                    rightTopping.GetComponent<Topping>().willMatch = true;
                                }
                            }
                        }
                    }
                    if(j > 0 && j < board.height - 1){
                        GameObject upTopping = board.allToppings[i, j + 1];
                        GameObject downTopping = board.allToppings[i, j - 1];
                        if(upTopping != null && downTopping != null){
                            if(upTopping.tag == currentTopping.tag && downTopping.tag == currentTopping.tag){
                                if(DetectMovement(currentTopping, upTopping, downTopping)){
                                    if(!currentMatches.Contains(upTopping)){
                                        currentMatches.Add(upTopping);
                                    }
                                    upTopping.GetComponent<Topping>().isMatched = true;
                                    if(!currentMatches.Contains(downTopping)){
                                        currentMatches.Add(downTopping);
                                    }
                                    downTopping.GetComponent<Topping>().isMatched = true;
                                    if(!currentMatches.Contains(currentTopping)){
                                        currentMatches.Add(currentTopping);
                                    }
                                    currentTopping.GetComponent<Topping>().isMatched = true;
                                }
                                else{
                                    currentTopping.GetComponent<Topping>().willMatch = true;
                                    upTopping.GetComponent<Topping>().willMatch = true;
                                    downTopping.GetComponent<Topping>().willMatch = true;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private bool DetectMovement(GameObject Topping1, GameObject Topping2, GameObject Topping3){
        return (Topping1.GetComponent<Topping>().movingState != MovingState.moving && Topping2.GetComponent<Topping>().movingState != MovingState.moving && Topping3.GetComponent<Topping>().movingState != MovingState.moving);
    }
}
