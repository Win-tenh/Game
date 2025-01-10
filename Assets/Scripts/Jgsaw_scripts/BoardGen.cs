using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGen : MonoBehaviour
{
    private int chapterSelected;
    private string imageFilename;
    Sprite mBaseSpriteOpaque;
    Sprite mBaseSpriteTransparent;

    GameObject mGOOpaque;
    GameObject mGOTransparent;

    public float ghostTransparency = 0.0001f;

    // jigsaw tiles creation
    public int numTileX { get; private set; }
    public int numTileY { get; private set; }

    Tile[,] mTiles = null;
    GameObject[,] mTileGOs = null;

    public Transform parentForTiles = null;

    // access to menu
    public MenuJigsaw menu = null;
    public List<Rect> regions = new List<Rect>();
    private List<Coroutine> activeCroutines = new List<Coroutine>();

    private bool isTimerRunning = false; // Trạng thái timer

    Sprite LoadBaseTexture()
    {
        Texture2D tex = SpritesUtils.LoadTexture(imageFilename);
        if (!tex.isReadable)
        {
            Debug.Log("Texture is not readable");
            return null;
        }

        if (tex.width % Tile.tileSize != 0 || tex.height % Tile.tileSize != 0)
        {
            Debug.Log(tex.width + "_" + tex.height);
            Debug.Log(" Size is multiple of " + Tile.tileSize);
            return null;
        }

        // Add padding on all side
        Texture2D newTex = new Texture2D(
            tex.width + Tile.padding * 2,
            tex.height + Tile.padding * 2,
            TextureFormat.ARGB32,
            false);

        for (int i = 0; i < tex.width; i++)
            for (int j = 0; j < tex.height; j++)
                newTex.SetPixel(i, j, Color.white);
        // copy colors
        for (int x = 0; x < tex.width; ++x)
            for (int y = 0; y < tex.height; ++y)
            {
                Color color = tex.GetPixel(x, y);
                color.a = 1.0f;
                newTex.SetPixel(x + Tile.padding, y + Tile.padding, color);
            }
        newTex.Apply();

        Sprite sprite = SpritesUtils.CreateSpriteFromTexture2D(
            newTex,
            0,
            0,
            newTex.width,
            newTex.height);
        return sprite;

    }    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chapterSelected = PlayerPrefs.GetInt("ChapterSelected", 1);
        imageFilename = GameApp.Instance.GetJigsawImgName(chapterSelected-1);

        mBaseSpriteOpaque = LoadBaseTexture();
        mGOOpaque = new GameObject();
        mGOOpaque.name = imageFilename + "_Opaque";
        mGOOpaque.AddComponent<SpriteRenderer>().sprite = mBaseSpriteOpaque;
        mGOOpaque.GetComponent<SpriteRenderer>().sortingLayerName = "Opaque";

        mBaseSpriteTransparent = CreateTransparentView(mBaseSpriteOpaque.texture);
        mGOTransparent = new GameObject();
        mGOTransparent.name = imageFilename + "_Transparent";
        mGOTransparent.AddComponent<SpriteRenderer>().sprite = mBaseSpriteTransparent;
        mGOTransparent.GetComponent<SpriteRenderer>().sortingLayerName= "Transparent";

        mGOOpaque.gameObject.SetActive(false);
       
        SetCameraPosition();

        //CreateJigSawTiles();
        StartCoroutine(Coroutine_CreateJigSawTiles());
    }

    Sprite CreateTransparentView(Texture2D tex)
    {
        Texture2D newTex = new Texture2D(
            tex.width,
            tex.height,
            TextureFormat.ARGB32,
            false);

        for (int i = 0; i < newTex.width; i++)
            for (int j = 0; j < newTex.height; j++)
            {
                Color c = tex.GetPixel(i, j);

                if (i > Tile.padding && 
                    i < (newTex.width - Tile.padding) &&
                    j > Tile.padding && 
                    j < (newTex.height - Tile.padding))
                {
                    c.a = ghostTransparency;
                }
                newTex.SetPixel(i, j, c);
            }
        newTex.Apply();

        Sprite sprite = SpritesUtils.CreateSpriteFromTexture2D(
            newTex,
            0,
            0,
            newTex.width,
            newTex.height);
        return sprite;
    }

    void SetCameraPosition()
    {
        Camera.main.transform.position = new Vector3(
            mBaseSpriteOpaque.texture.width / 2,
            mBaseSpriteOpaque.texture.height / 2, 
            -10.0f);

        //Camera.main.orthographicSize = mBaseSpriteOpaque.texture.width / 2;
        int smaller_value = Mathf.Min(
            mBaseSpriteOpaque.texture.width,
            mBaseSpriteOpaque.texture.height);
        Camera.main.orthographicSize = smaller_value * 0.8f;

    }

    public static GameObject CreateGOFromTile(Tile tile)
    {
        GameObject obj = new GameObject();

        obj.name = 
            "TileGameObe_" + 
            tile.xIndex.ToString() + 
            "_" + 
            tile.yIndex.ToString();

        obj.transform.position = new Vector3(
            tile.xIndex * Tile.tileSize,
            tile.yIndex * Tile.tileSize,
            0.0f
            );

        SpriteRenderer spriteRen = obj.AddComponent<SpriteRenderer>();
        spriteRen.sprite = SpritesUtils.CreateSpriteFromTexture2D(
            tile.finalCut,
            0,
            0,
            Tile.padding * 2 + Tile.tileSize,
            Tile.padding * 2 + Tile.tileSize);

        BoxCollider2D box = obj.AddComponent<BoxCollider2D>();

        TileMovement tileMovement = obj.AddComponent<TileMovement>();
        tileMovement.tile = tile;

        return obj;
    }

    IEnumerator Coroutine_CreateJigSawTiles()
    {
        Texture2D baseTexture = mBaseSpriteOpaque.texture;
        numTileX = baseTexture.width / Tile.tileSize;
        numTileY = baseTexture.height / Tile.tileSize;

        mTiles = new Tile[numTileX, numTileY];
        mTileGOs = new GameObject[numTileX, numTileY];

        for (int i = 0; i < numTileX; i++)
            for (int j = 0; j < numTileY; j++)
            {
                mTiles[i, j] = CreateTile(i, j, baseTexture);
                mTileGOs[i, j] = CreateGOFromTile(mTiles[i, j]);
                if (parentForTiles != null)
                {
                    mTileGOs[i, j].transform.SetParent(parentForTiles);
                }
                yield return null;
            }

        // enable the bottom panel
        // set the delegate to button play on click
        menu.SetEnableBotPanel(true);
        menu.btnPlayOnClick = ShuffleTiles;
    }

    Tile CreateTile(int i, int j, Texture2D baseTexture)
    {
        Tile tile = new Tile(baseTexture);
        tile.xIndex = i;
        tile.yIndex = j;

        // left side tile
        if (i == 0)
        {
            tile.SetCurveType(Tile.Direction.LEFT, Tile.PosNegType.NONE);
        } else
        {
            // has left direction opposite curve type
            Tile leftTile = mTiles[i - 1, j];
            Tile.PosNegType rightOp = leftTile.GetCurveType(Tile.Direction.RIGHT);
            tile.SetCurveType(Tile.Direction.LEFT, rightOp == Tile.PosNegType.NEG ?
                Tile.PosNegType.POS: Tile.PosNegType.NEG);
        }

        // bot side tile
        if (j == 0)
        {
            tile.SetCurveType(Tile.Direction.DOWN, Tile.PosNegType.NONE);
        }
        else
        {
            // has left direction opposite curve type
            Tile downTile = mTiles[i, j - 1];
            Tile.PosNegType upOp = downTile.GetCurveType(Tile.Direction.UP);
            tile.SetCurveType(Tile.Direction.DOWN, upOp == Tile.PosNegType.NEG ?
                Tile.PosNegType.POS : Tile.PosNegType.NEG);
        }


        // right side tile
        if (i == numTileX - 1)
        {
            tile.SetCurveType(Tile.Direction.RIGHT, Tile.PosNegType.NONE);
        }
        else
        {
            float toss = Random.Range(0f, 1f);
            if (toss < 0.5f)
                tile.SetCurveType(Tile.Direction.RIGHT, Tile.PosNegType.POS);
            else
                tile.SetCurveType(Tile.Direction.RIGHT, Tile.PosNegType.NEG);
        }

        // up side tile
        if (j == numTileY - 1)
        {
            tile.SetCurveType(Tile.Direction.UP, Tile.PosNegType.NONE);
        } else
        {
            float toss = Random.Range(0f, 1f);
            if (toss < 0.5f)
                tile.SetCurveType(Tile.Direction.UP, Tile.PosNegType.POS);
            else
                tile.SetCurveType(Tile.Direction.UP, Tile.PosNegType.NEG);
        }
        tile.Apply();
        return tile;
    }    

    #region Shuffling realted codes

    private IEnumerator Coroutine_MoveOverSeconds(GameObject objToMove, Vector3 end, float seconds)
    {
        float elaspedTime = 0f;
        Vector3 stratingPosition = objToMove.transform.position;
        while (elaspedTime <seconds)
        {
            objToMove.transform.position = Vector3.Lerp(
                stratingPosition, end, (elaspedTime / seconds));
            elaspedTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
        objToMove.transform.position = end;
    }

    void Shuffle(GameObject obj)
    {
        if (regions.Count == 0)
        {
            regions.Add(new Rect(
                -300.0f,
                -100.0f,
                50.0f,
                numTileY * Tile.tileSize));

            regions.Add(new Rect(
                (numTileX + 1) * Tile.tileSize,
                -100.0f,
                50.0f,
                numTileY * Tile.tileSize));
        }

        int regionIndex = Random.Range(0, regions.Count);
        float x = Random.Range(
            regions[regionIndex].xMin, 
            regions[regionIndex].xMax);
        float y = Random.Range(
            regions[regionIndex].yMin, 
            regions[regionIndex].yMax);

        Vector3 pos = new Vector3(x, y, 0.0f);
        Coroutine moveCoroutine = StartCoroutine(Coroutine_MoveOverSeconds(obj, pos, 1.0f));
        activeCroutines.Add(moveCoroutine);
    }

    IEnumerator Coroutine_Shuffle()
    {
        for (int i = 0; i < numTileX; ++i)
            for (int j = 0; j < numTileY; ++j)
            {
                Shuffle(mTileGOs[i, j]);
                yield return null;
            }

        foreach (var item in activeCroutines)
        {
            if (item != null)
                yield return item;
        }

        OnFinishedShuffling();
    }

    public void ShuffleTiles()
    {
        menu.SetEnableBotPanel(false);
        StartCoroutine(Coroutine_Shuffle());
    }

    void OnFinishedShuffling()
    {
        activeCroutines.Clear();

        StartCoroutine(Coroutine_CallAfterDelay(
            () => menu.SetEnableTopPanel(true), 1.0f));
        GameApp.Instance.TileMovementEnabled = true;

        StartTimer();

        for (int i = 0; i < numTileX; ++i)
            for (int j = 0; j < numTileY; ++j)
            {
                TileMovement tm = mTileGOs[i, j].GetComponent<TileMovement>();
                tm.onTileInPlace += OnTileInPlace;
                SpriteRenderer spriteRenderer = tm.gameObject.GetComponent<SpriteRenderer>();
                Tile.tilesSorting.BringToTop(spriteRenderer);

            }

        menu.SetTotalTiles(numTileX * numTileY);
    }

    IEnumerator Coroutine_CallAfterDelay(System.Action func, float delay)
    {
        yield return new WaitForSeconds(delay);
        func();
    }

    public void StartTimer()
    {
        if (!isTimerRunning) // Đảm bảo không tạo nhiều coroutine cùng lúc
        {
            isTimerRunning = true;
            StartCoroutine(Coroutine_Timer());
        }
    }

    IEnumerator Coroutine_Timer()
    {
        yield return new WaitForSeconds(1.0f);
        while (true)
        {
            if (isTimerRunning)
            {
                yield return new WaitForSeconds(1.0f);
                GameApp.Instance.SecondsSinceStart += 1;

                menu.SetTimeInSeconds(GameApp.Instance.SecondsSinceStart); // Cập nhật giao diện
            } else
                yield return null; // Tạm dừng và chờ trạng thái được tiếp tục
        } 
    }

    public void PauseTimer()
    {
        isTimerRunning = false; // Dừng timer
        menu.SetEnablePausePanel(true); // Hiển thị panel pause
    }

    public void ResumeTimer()
    {
        isTimerRunning = true; // Tiếp tục timer
        menu.SetEnablePausePanel(false); // Ẩn panel pause
    }

    public void StopTimer()
    {
        isTimerRunning = false; // Dừng timer
        StopCoroutine(Coroutine_Timer()); // Dừng coroutine timer
    }    

    public void ResetGame()
    {
        StopTimer();
        GameApp.Instance.TotalTilesInCorrectPosition = 0;
        GameApp.Instance.SecondsSinceStart = 0; // Reset giá trị thời gian

        Tile.tilesSorting.CLear();
        menu.OnClickPLayAgain();
    }

    #endregion


    public void ShowOpaqueImage()
    {
        mGOOpaque.SetActive(true);
    }

    public void HideOpaqueImage()
    {
        mGOOpaque.SetActive(false);
    }

    void OnTileInPlace(TileMovement tm)
    {
        GameApp.Instance.TotalTilesInCorrectPosition += 1;

        tm.enabled = false;
        Destroy(tm);

        SpriteRenderer spriteRenderer =
            tm.gameObject.GetComponent<SpriteRenderer>();
        Tile.tilesSorting.Remove(spriteRenderer);

        spriteRenderer.sortingLayerName = "TileInPlace";
        if (GameApp.Instance.TotalTilesInCorrectPosition ==
            mTileGOs.Length)
        {
            //Debug.Log("Game complete");
            menu.SetEnableTopPanel(false);
            menu.SetEnableGameComplete(true);

            // reset value
            StopTimer();
            GameApp.Instance.TotalTilesInCorrectPosition = 0;
            GameApp.Instance.SecondsSinceStart = 0;

        }
        menu.SetTilesInPlace(GameApp.Instance.TotalTilesInCorrectPosition); 
    }
}
