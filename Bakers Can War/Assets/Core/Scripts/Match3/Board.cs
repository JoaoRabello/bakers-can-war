using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState{
    wait,
    move
}

public class Board : MonoBehaviour
{
    public GameState currentState = GameState.move;
    public int height = 8;
    public int width = 7;
    public int offSet = 20;
    //public float distanceMultiplier = 1.5f;
    public Dictionary<string, int> matchedDict = new Dictionary<string, int>();
    public string[] toppingsList;
    public GameObject prefab;
    public GameObject[] toppings;

    // private BackgroundTile[,] allTiles;
    public GameObject[,] allToppings;
    private FindMatches findMatches;
    void Start()
    {   
        ResetMatchedDict();
        findMatches = FindObjectOfType<FindMatches>();
        // allTiles = new BackgroundTile[width, height];
        allToppings = new GameObject[width, height];
        SetBoard();
    }
    //void Update()
    //{
    //    if (Input.GetKeyDown("space"))
    //    {
    //          SetBoard();
    //   }
    //}
    void SetBoard()
    {
        for (int i = 0; i < width; i++) 
        {
            for (int j = 0; j < height; j++) 
            {
                Vector3 tempPos = new Vector3(i, j + offSet-((int) (offSet/(j+2))), 0);
                GameObject toppingTile = Instantiate(prefab, tempPos, Quaternion.identity) as GameObject;
                toppingTile.transform.parent = this.transform;
                toppingTile.name = "("+ i +","+ j +")";

                int toppingToUse = Random.Range(0,toppings.Length);
                int maxIterations = 0;
                while(MatchesAt(i, j, toppings[toppingToUse]) && maxIterations < 100 ){
                    toppingToUse = Random.Range(0,toppings.Length);
                    maxIterations++;
                }
                maxIterations = 0;

                GameObject topping = Instantiate(toppings[toppingToUse], tempPos, Quaternion.identity);
                topping.GetComponent<Topping>().row = j;
                topping.GetComponent<Topping>().column = i;
                topping.transform.parent = this.transform;
                topping.name = "("+ i +","+ j +")";
                allToppings[i, j] = topping;
            }
        }
    }

    private bool MatchesAt(int column, int row, GameObject piece){
        if( column > 1 )           
        {
            if(allToppings[column - 1, row].tag == piece.tag && allToppings[column - 2, row].tag == piece.tag)
            {
                return true;
            }
        } 
        if( row > 1 )
        {
            if(allToppings[column, row - 1].tag == piece.tag && allToppings[column, row - 2].tag == piece.tag)
            {
                return true;
            }
        }
        return false;
    } 

    private void DestroyMatchesAt(int column, int row){
        if(allToppings[column, row].GetComponent<Topping>().isMatched){
            findMatches.currentMatches.Remove(allToppings[column, row]);
            Destroy(allToppings[column, row]);
            allToppings[column, row] = null;
        }
    }

    public void DestroyMatches(){
        CalculateAndSend();
        for(int i = 0; i < width; i++){
            for(int j = 0; j < height; j++){
                if(allToppings[i, j] != null){
                    DestroyMatchesAt(i, j);
                }
            }
        }
        StartCoroutine(DecreaseRowCo());
    }

    private void CalculateAndSend(){
        for(int i = 0; i < findMatches.currentMatches.Count; i++){
            matchedDict[findMatches.currentMatches[i].tag]+=1;
        }
        for(int i = 0; i < toppingsList.Length; i++){
            if( matchedDict[toppingsList[i]] == 0 ){
                continue;
            }
            else if( matchedDict[toppingsList[i]] == 3 ){
                SendToScore(matchedDict[toppingsList[i]], 3);
            }
            else if( matchedDict[toppingsList[i]] == 4 ){
                SendToScore(matchedDict[toppingsList[i]], 4);
            }
            else if( matchedDict[toppingsList[i]] == 5 ){
                SendToScore(matchedDict[toppingsList[i]], 5);
            }
            else if( matchedDict[toppingsList[i]] == 6 ){
                SendToScore(matchedDict[toppingsList[i]], 3);
                SendToScore(matchedDict[toppingsList[i]], 3);
            }
            else if( matchedDict[toppingsList[i]] == 7 ){
                SendToScore(matchedDict[toppingsList[i]], 4);
                SendToScore(matchedDict[toppingsList[i]], 3);
            }
            else if( matchedDict[toppingsList[i]] == 8 ){
                SendToScore(matchedDict[toppingsList[i]], 4);
                SendToScore(matchedDict[toppingsList[i]], 4);
            }
            else if( matchedDict[toppingsList[i]] == 9 ){
                SendToScore(matchedDict[toppingsList[i]], 5);
                SendToScore(matchedDict[toppingsList[i]], 4);
            }
            else if( matchedDict[toppingsList[i]] == 10 ){
                SendToScore(matchedDict[toppingsList[i]], 5);
                SendToScore(matchedDict[toppingsList[i]], 5);
            }
            else{
                SendToScore(matchedDict[toppingsList[i]], 0);
            }
        }
        ResetMatchedDict();
    }
    private void SendToScore(string topping, int toppingValue){

    }
    private void ResetMatchedDict(){
        matchedDict = new Dictionary<string, int>();
        for(int i = 0; i < toppingsList.Length; i++){
            matchedDict.Add(toppingsList[i], 0);  
        }
    }
    private IEnumerator DecreaseRowCo(){
        int nullCount = 0;
        for(int i = 0; i < width; i++){
            for(int j = 0; j < height; j++){
                if(allToppings[i, j] == null){
                    nullCount++; 
                }
                else if(nullCount > 0)
                {
                    allToppings[i, j].GetComponent<Topping>().row -= nullCount;
                    allToppings[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }
    private void RefillBoard(){
        for(int i = 0; i < width; i++){
            for(int j = 0; j < height; j++){
                if(allToppings[i, j] == null){
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    int toppingToUse = Random.Range(0, toppings.Length);
                    GameObject piece = Instantiate(toppings[toppingToUse], tempPosition, Quaternion.identity);
                    allToppings[i, j] = piece;
                    piece.GetComponent<Topping>().row = j;
                    piece.GetComponent<Topping>().column = i;
                }
            }
        }
    }

    private bool MatchesOnBoard(){
        for(int i = 0; i < width; i++){
            for(int j = 0; j < height; j++){
                if(allToppings[i, j] != null){
                    if(allToppings[i, j].GetComponent<Topping>().isMatched){
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator FillBoardCo(){
        RefillBoard();
        yield return new WaitForSeconds(.3f);
        
        while(MatchesOnBoard()){
            yield return new WaitForSeconds(.1f);
            DestroyMatches();
        }
        yield return new WaitForSeconds(.3f);
        currentState = GameState.move;
    }
}
