using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovingState{
    moving,
    idle
}


public class Topping : MonoBehaviour
{   public MovingState movingState = MovingState.idle;
    private bool movingX = false;
    private bool movingY = false;
    public bool willMatch = false;
    public int column;
    public int row;
    public int targetX;
    public int targetY;
    public bool isMatched = false;
    public int previousColumn;
    public int previousRow;
    public string toppingMatchType = "none";
    public int toppingMatchLength = 0;
    private FindMatches findMatches;
    //public float boardMultiplier;
    private Board board;
    private GameObject otherTopping;
    private Vector3 initialPosition;
    private Vector3 finalPosition;
    private Vector3 temPosition;
    public float swipeAngle = 0f;
    public float swipeResist = 2f;
    void Start()
    {
        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
        //boardMultiplier = board.distanceMultiplier;
        //targetX = (int)transform.position.x;
        //targetY = (int)transform.position.y;
        //row = targetY;
        //column = targetX;
        //previousColumn = column;
        //previousRow = row;
    }
    void Update()
    {   //FindMatches();
        if (isMatched){
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            StartCoroutine(MatchedVanish());
        }
        targetX = column;
        targetY = row;
        if (Mathf.Abs(targetX - transform.position.x) > .1){
            movingState = MovingState.moving;
            movingX = true;
            temPosition = new Vector3(targetX, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, temPosition, Time.deltaTime* 3.8f);
            if(board.allToppings[column, row]!= this.gameObject){
                board.allToppings[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();
        }
        else{
            movingX = false;
            temPosition = new Vector3(targetX, transform.position.y, transform.position.z);
            transform.position = temPosition;
        }
        if (Mathf.Abs(targetY - transform.position.y) > .1){
            movingState = MovingState.moving;
            movingY = true;
            temPosition = new Vector3(transform.position.x, targetY, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, temPosition, Time.deltaTime* 3.8f);
            if(board.allToppings[column, row]!= this.gameObject){
                board.allToppings[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();
        }
        else{
            movingY = false;
            temPosition = new Vector3(transform.position.x, targetY, transform.position.z);
            transform.position = temPosition;
        }
        if(!movingX && !movingY){
            movingState = MovingState.idle;
        }
    }

    public IEnumerator MatchedVanish(){
        yield return new WaitForSeconds(.2f);
        board.DestroyMatches();
    }

    public IEnumerator CheckMoveCo(){
        yield return new WaitForSeconds(.3f);
        if(otherTopping != null){
            if(!isMatched && !otherTopping.GetComponent<Topping>().isMatched){
                if(!willMatch && !otherTopping.GetComponent<Topping>().willMatch){
                    otherTopping.GetComponent<Topping>().row = row;
                    otherTopping.GetComponent<Topping>().column = column;
                    row = previousRow;
                    column = previousColumn;
                    yield return new WaitForSeconds(.5f);
                    board.currentState = GameState.move;
                }
            }
            else
            {
                board.DestroyMatches();
            }
            otherTopping = null;
        }
    }

    private void OnMouseDown() 
    {
        //if (board.currentState == GameState.move){
            initialPosition = Input.mousePosition;
        //}
    }
    private void OnMouseUp() 
    {   
        //if (board.currentState == GameState.move){
            finalPosition = Input.mousePosition;
            CalculateAngle();
        //}
    }
    void CalculateAngle(){
        if(Mathf.Abs(finalPosition.y - initialPosition.y) > swipeResist || Mathf.Abs(finalPosition.x - initialPosition.x) > swipeResist){
            swipeAngle = Mathf.Atan2(finalPosition.y - initialPosition.y,finalPosition.x - initialPosition.x) * 180/Mathf.PI;
            MovePieces();
            board.currentState = GameState.wait;
        }else{
            board.currentState = GameState.move;
        }
    }

    void MovePieces(){
        if(swipeAngle > -45 && swipeAngle <= 45 && column < board.width-1){
            //Right
            otherTopping = board.allToppings[column + 1,row];
            previousColumn = column;
            previousRow = row;
            otherTopping.GetComponent<Topping>().column -=1;
            column += 1;
        } else if(swipeAngle > 45 && swipeAngle <= 135  && row < board.height-1){
            //Up
            otherTopping = board.allToppings[column,row +1];
            previousColumn = column;
            previousRow = row;
            otherTopping.GetComponent<Topping>().row -=1;
            row += 1;
        } else if((swipeAngle > 135 || swipeAngle <= -135)  && column > 0){
            //Left
            otherTopping = board.allToppings[column - 1,row];
            previousColumn = column;
            previousRow = row;
            otherTopping.GetComponent<Topping>().column +=1;
            column -= 1;
        } else if(swipeAngle < -45 && swipeAngle >= -135  && row > 0){
            //Down
            otherTopping = board.allToppings[column,row -1];
            previousColumn = column;
            previousRow = row;
            otherTopping.GetComponent<Topping>().row +=1;
            row -= 1;
        }
        StartCoroutine(CheckMoveCo());
    }

    void FindMatches(){
        if(column > 0 && column < board.width -1){
            GameObject leftTopping1 = board.allToppings[column - 1, row];
            GameObject rightTopping1 = board.allToppings[column + 1, row];
            if(leftTopping1 != null && rightTopping1 != null && this.gameObject != null){ 
                if (leftTopping1.tag == this.gameObject.tag && rightTopping1.tag == this.gameObject.tag){
                    leftTopping1.GetComponent<Topping>().isMatched = true;
                    rightTopping1.GetComponent<Topping>().isMatched = true;
                    isMatched = true;
                }
            }
        }
        if(row > 0 && row < board.height -1){
            GameObject upTopping1 = board.allToppings[column, row + 1];
            GameObject downTopping1 = board.allToppings[column, row - 1];
            if(upTopping1 != null && downTopping1 != null && this.gameObject != null){ 
                if (upTopping1.tag == this.gameObject.tag && downTopping1.tag == this.gameObject.tag){
                    upTopping1.GetComponent<Topping>().isMatched = true;
                    downTopping1.GetComponent<Topping>().isMatched = true;
                    isMatched = true;
                }
            }
        }
    }
}
