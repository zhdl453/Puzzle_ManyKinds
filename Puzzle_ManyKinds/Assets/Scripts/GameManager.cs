using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Game Elements")]
    [Range(2, 6)] //밑의 난이도를 2~6사이로 조절하는 바 생김
    [SerializeField] private int difficulty = 4;
    [SerializeField] private Transform gameHolder;
    [SerializeField] private Transform piecePrefab;


    [Header("UI Elements")]
    [SerializeField] private List<Texture2D> imageTextures;
    [SerializeField] private Transform levelSelectPanel;
    [SerializeField] private Image levelSelectPrefab;
    [SerializeField] private GameObject playAgainButton;
    private List<Transform> pieces;
    private Vector2Int dimensions;
    private float width;
    private float height;
    private Transform draggingPiece = null; //nothing selected yet
    private Vector3 offset;
    private int piecesCorrect;

    void Start()
    {
        //Create the UI
        foreach (Texture2D texture in imageTextures)
        {
            Image image = Instantiate(levelSelectPrefab, levelSelectPanel); //levelSelectPanel를 부모객체로 놓고, 그 밑에 프리팹 생성
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            //Assign Button action
            image.GetComponent<Button>().onClick.AddListener(delegate { StartGame(texture); }); //OnClick에다가 이벤트 추가 시키는법
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                //Everything is moveable, so we don't need to check it's a Piece.
                draggingPiece = hit.transform;
                offset = draggingPiece.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                offset += Vector3.back;
            }
        }
        //when we release the mouse button stop dragging.
        if (draggingPiece && Input.GetMouseButtonUp(0))
        {
            SnapAndDisableIfCorrect();
            draggingPiece.position += Vector3.forward;
            draggingPiece = null;
        }

        //Set the dragged piece position to the position of the mouse.
        if (draggingPiece)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //newPosition.z = draggingPiece.position.z; //이 세팅을 안하면 카메라의 z축값이 들어가 보이지 않을것이다.
            newPosition += offset;
            draggingPiece.position = newPosition;
        }
    }
    public void SnapAndDisableIfCorrect()
    {
        //각 퍼즐 조각이 가장 가까운 퍼즐 위치에 스냅될 수 있도록 로직 바꿔야함.
        int closestIndex = -1;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < pieces.Count; i++)
        {
            int col = i % dimensions.x;
            int row = i / dimensions.x;
            Vector2 targetPostion = new((-width * dimensions.x / 2) + (width * col) + (width / 2),
                                    (-height * dimensions.y / 2) + (height * row) + (height / 2));
            float distance = Vector2.Distance(draggingPiece.localPosition, targetPostion);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }
        if (closestDistance < (width / 2))
        {
            int col = closestIndex % dimensions.x;
            int row = closestIndex / dimensions.x;

            Vector2 targetPosition = new((-width * dimensions.x / 2) + (width * col) + (width / 2),
                                    (-height * dimensions.y / 2) + (height * row) + (height / 2));
            draggingPiece.localPosition = targetPosition;

            // 올바른 위치에 놓였는지 확인.
            if ((Vector2)draggingPiece.localPosition == targetPosition)
            {
                piecesCorrect++;
                // 퍼즐 조각이 올바른 위치에 놓이면, 더 이상 움직이지 못하게 비활성화.
                //draggingPiece.GetComponent<Collider2D>().enabled = false;
            }
        }
        // 모든 퍼즐 조각이 올바르게 맞춰졌는지 확인.
        if (piecesCorrect == pieces.Count)
        {
            Debug.Log("Puzzle Complete!");
        }
    }


    // public void SnapAndDisableIfCorrect()
    // {
    //     //각 퍼즐 조각이 가장 가까운 퍼즐 위치에 스냅될 수 있도록 로직 바꿔야함.
    //     int pieceIndex = pieces.IndexOf(draggingPiece);

    //     //The coordinates of the piece in the puzzle.
    //     int col = pieceIndex % dimensions.x;
    //     int row = pieceIndex / dimensions.x;

    //     //The target position in the non-scaled coordinates.
    //     Vector2 targetPostion = new((-width * dimensions.x / 2) + (width * col) + (width / 2),
    //                                 (-height * dimensions.y / 2) + (height * row) + (height / 2));
    //     //Check if we're in the correct location
    //     if (Vector2.Distance(draggingPiece.localPosition, targetPostion) < (width / 2))
    //     {
    //         //snap to our destination.
    //         draggingPiece.localPosition = targetPostion;

    //         //Disable the collider so we can't click on the object anymore.
    //         //draggingPiece.GetComponent<BoxCollider2D>().enabled = false;

    //         //Increase the number of correct pieces, and check for puzzle completion.
    //         piecesCorrect++;
    //         if (piecesCorrect == pieces.Count)
    //         {
    //             playAgainButton.SetActive(true);
    //         }
    //     }
    // }

    public void StartGame(Texture2D jigsawTexture)
    {
        //Hide The UI
        levelSelectPanel.gameObject.SetActive(false);
        //We store a list of the transform for each jigsaw piece so we can track them later.
        pieces = new List<Transform>();
        //Calculate the size of each jigsaw piece, based on a difficulty setting.
        dimensions = GetDimensions(jigsawTexture, difficulty);
        //Create the pieces of the correct size with the correct texture.
        CreateJigsawPieces(jigsawTexture);
        //Place the pieces randomly into the visible area.
        Scatter();
        //Update the border to fit the chosen puzzle.
        UpdateBorder();
        //As we're starting the puzzle there will be no correct pieces.
        piecesCorrect = 0;
    }

    Vector2Int GetDimensions(Texture2D jigsawTexture, int difficulty)
    {
        Vector2Int dimensions = Vector2Int.zero; //1.디멘션를 선언하여 가로 세로 크기를 저장할 공간을 만듦
        if (jigsawTexture.width < jigsawTexture.height)//2.가로세로 크기비교해서 작은쪽 길이에 사용자가 설정한 난이도 적용
        {
            dimensions.x = difficulty;
            dimensions.y = (difficulty * jigsawTexture.height) / jigsawTexture.width;
            //3.퍼즐조각이 가능한 정사각형에 가깝게 만드는 계산
        }
        else
        {
            dimensions.x = (difficulty * jigsawTexture.width) / jigsawTexture.height;
            dimensions.y = difficulty;
        }
        return dimensions;
    }

    void CreateJigsawPieces(Texture2D jigsawTexture)
    {
        //Calculate piece sizes based on the dimensions.
        height = 1f / dimensions.y; //퍼즐조각의 높이 = 1 / dimensions.y=퍼즐의 세로 방향의 조각수
                                    //예를 들어 dimensions.y가 4라면 height는 0.25임
                                    //원본 이미지의 가로 세로 비율
        float aspect = (float)jigsawTexture.width / jigsawTexture.height;
        width = aspect / dimensions.x; //이렇게 해야 원본 이미지의 가로세로 비율 유지되면서 퍼즐조각의 너비 결정

        for (int row = 0; row < dimensions.y; row++) //행
        {
            for (int col = 0; col < dimensions.x; col++) //열
            {
                //Create the piece in the right location of the right size.
                Transform piece = Instantiate(piecePrefab, gameHolder);
                piece.localPosition = new Vector3
                (
                    (-width * dimensions.x / 2) + (width * col) + (width / 2), //if total width was 2, we'd start at -1 and go to +1
                    (-height * dimensions.x / 2) + (height * row) + (height / 2), //if total width was 2, we'd start at -1 and go to +1
                    -1
                );
                piece.localScale = new Vector3(width, height, 1f);

                //we don't have to name them, nut always useful for debugging.
                piece.name = $"Piece {(row * dimensions.x) + col}";
                pieces.Add(piece);

                //Assign the correct part of the texture for this jigsaw piece
                //we need our width and height both to be normalised between 0 and 1 for the UV.
                float width1 = 1f / dimensions.x;
                float height1 = 1f / dimensions.y;
                //UV coord order is anti-clockwise: (0,0),(1,0),(0,1),(1,1)
                Vector2[] uv = new Vector2[4];
                uv[0] = new Vector2(width1 * col, height1 * row);
                uv[1] = new Vector2(width1 * (col + 1), height1 * row);
                uv[2] = new Vector2(width1 * col, height1 * (row + 1));
                uv[3] = new Vector2(width1 * (col + 1), height1 * (row + 1));
                //Assign our new UVs to the mesh.
                Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                mesh.uv = uv; //이 부분은 가져온 메시의 UV 맵을 설정한다.
                              //UPdate the texture on the piece
                piece.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", jigsawTexture);
            }
        }
    }

    private void Scatter()
    {
        //Calculate the visible orthographic size of the screen.
        float orthoHeight = Camera.main.orthographicSize;
        float screenAspect = (float)Screen.width / Screen.height;
        float orthoWidth = (screenAspect * orthoHeight);

        //Ensure pieces are away from the edges.
        //퍼즐조각의 너비와 높이 계산
        float pieceWidth = width * gameHolder.localScale.x;
        float pieceHeight = height * gameHolder.localScale.y;

        //화면의 너비와 높이에서 퍼즐 조각의 너비와 높이를 빼줍니다.
        //이렇게 하면 퍼즐 조각이 화면 바깥으로 나가지 않도록 범위를 제한하게 됩니다.
        orthoHeight -= pieceHeight;
        orthoWidth -= pieceWidth;

        //Place each piece randomly in the visible area.
        foreach (Transform piece in pieces)
        {
            float x = Random.Range(-orthoWidth, orthoWidth);
            float y = Random.Range(-orthoHeight, orthoHeight);
            piece.position = new Vector3(x, y, -1);
        }
    }
    private void UpdateBorder()
    {
        LineRenderer lineRenderer = gameHolder.GetComponent<LineRenderer>();

        //Calculate helf sizes to simplify the code.
        float halfWidth = (width * dimensions.x) / 2f;
        float halfHeight = (height * dimensions.y) / 2f;

        //we want the border to be hehind the pieces.
        float borderZ = 0f;

        //Set border vertices, starting top left, going clockwise.
        lineRenderer.SetPosition(0, new Vector3(-halfWidth, halfHeight, borderZ));
        lineRenderer.SetPosition(1, new Vector3(halfWidth, halfHeight, borderZ));
        lineRenderer.SetPosition(2, new Vector3(halfWidth, -halfHeight, borderZ));
        lineRenderer.SetPosition(3, new Vector3(-halfWidth, -halfHeight, borderZ));

        //Set the thickness of the border line.
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        //Show the border line.
        lineRenderer.enabled = true;
    }
    public void RestartGame()
    {
        //Destroy all the puzzle pieces.
        foreach (Transform piece in pieces)
        {
            Destroy(piece.gameObject);
        }
        pieces.Clear();
        //Hide the outline.
        gameHolder.GetComponent<LineRenderer>().enabled = false;
        //Show the level select UI.
        playAgainButton.SetActive(false);
        levelSelectPanel.gameObject.SetActive(true);
    }
}
