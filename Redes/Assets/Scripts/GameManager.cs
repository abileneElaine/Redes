using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform player1Hand;
    public Transform player2Hand;
    public Transform boardParent;

    private int currentPlayer = 1;
    private int leftEnd = -1;
    private int rightEnd = -1;

    private List<DominoPiece> piecesOnBoard = new List<DominoPiece>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        DominoPiece[] todas = GameObject.FindObjectsOfType<DominoPiece>(true);
        Debug.Log("Total de peças encontradas: " + todas.Length);

        List<DominoPiece> bag = new List<DominoPiece>(todas);
        Shuffle(bag);

        for (int i = 0; i < bag.Count; i++)
        {
            var piece = bag[i];
            piece.gameObject.SetActive(true); // ativa a peça

            if (i < 7)
                piece.transform.SetParent(player1Hand);
            else if (i < 14)
                piece.transform.SetParent(player2Hand);
            else
                piece.gameObject.SetActive(false); // reserva
        }

        Debug.Log("Começa o jogador 1");
    }



    public void DistributePieces()
    {
        DominoPiece[] todas = GameObject.FindObjectsOfType<DominoPiece>(true);
        List<DominoPiece> bag = new List<DominoPiece>(todas);

        Shuffle(bag);

        for (int i = 0; i < bag.Count; i++)
        {
            var piece = bag[i];
            piece.gameObject.SetActive(true);
            piece.transform.localScale = Vector3.one;

            if (i < 7)
                piece.transform.SetParent(player1Hand);
            else if (i < 14)
                piece.transform.SetParent(player2Hand);
            else
                piece.gameObject.SetActive(false); // reserva
        }
    }

    void Shuffle(List<DominoPiece> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(i, list.Count);
            var temp = list[i];
            list[i] = list[rnd];
            list[rnd] = temp;
        }
    }

    public bool CanPlayerMove(DominoPiece piece)
    {
        return (currentPlayer == 1 && piece.transform.parent == player1Hand) ||
               (currentPlayer == 2 && piece.transform.parent == player2Hand);
    }

    public bool TryPlacePiece(DominoPiece piece, Vector3 position)
    {
        if (piecesOnBoard.Count == 0)
        {
            leftEnd = piece.leftValue;
            rightEnd = piece.rightValue;
            PlacePiece(piece, position);
            return true;
        }

        if (piece.leftValue == leftEnd)
        {
            leftEnd = piece.rightValue;
            PlacePiece(piece, position);
            return true;
        }
        else if (piece.rightValue == leftEnd)
        {
            leftEnd = piece.leftValue;
            piece.transform.Rotate(0, 0, 180);
            PlacePiece(piece, position);
            return true;
        }
        else if (piece.leftValue == rightEnd)
        {
            rightEnd = piece.rightValue;
            PlacePiece(piece, position);
            return true;
        }
        else if (piece.rightValue == rightEnd)
        {
            rightEnd = piece.leftValue;
            piece.transform.Rotate(0, 0, 180);
            PlacePiece(piece, position);
            return true;
        }

        return false;
    }

    void PlacePiece(DominoPiece piece, Vector3 position)
    {
        piece.transform.position = position;
        piece.transform.SetParent(boardParent);
        piecesOnBoard.Add(piece);
        SwitchTurn();
    }

    void SwitchTurn()
    {
        currentPlayer = (currentPlayer == 1) ? 2 : 1;
        Debug.Log("Agora é o turno do jogador " + currentPlayer);
    }
}
