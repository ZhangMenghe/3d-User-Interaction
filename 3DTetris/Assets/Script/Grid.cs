using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Grid : MonoBehaviour {

    //Store game score
    public static int gameScore = 0;
    public AudioClip clearAudio;
    public int clearLineNum;
    private AudioSource audioSound;
    //initialize the width, length and height of the game area
    public int w;
    public int l;
    public int h;
    public Transform[,,] grid;// = new Transform[w,h,l];
    public Grid(int width, int length, int height)
    {
        clearLineNum = 0;
        w = width;
        l = length;
        h = height;
        grid = new Transform[w, h, l];
        audioSound = GameObject.Find("Controller").GetComponent<AudioSource>();
    }
    void Start()
    {
    }

    public bool insideBorder(Vector3 pos)
    {
        //audioSound.PlayOneShot(clearAudio);
        bool insideX = (int)pos.x >= 0 && (int)pos.x < w;
        bool insideY = (int)pos.z >= 0 && (int)pos.z < l;
        bool insideZ = (int)pos.y >= 0 && (int)pos.y < (h - 2);
        return (insideX && insideY && insideZ);
    }


    public void decreasePlaneAbove(int level)
    {
        for (int y = level; y < h; ++y)
        {
            for (int x = 0; x < w; ++x)
            {
                for (int z = 0; z < l; ++z)
                {
                    if (grid[x, y, z] != null)
                    {
                        //move one towards the bottom
                        grid[x, y - 1, z] = grid[x, y, z];
                        grid[x, y, z] = null;

                        //update the blocks positon
                        grid[x, y - 1, z].position += new Vector3(0, -1, 0);
                    }
                }
            }
        }
    }
    //delete the whole plane
    public void deletePlane(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            for (int z = 0; z < l; ++z)
            {
                Destroy(grid[x, y, z].gameObject);
                grid[x, y, z] = null;
            }
        }
        clearLineNum++;
        shot other = (shot)GameObject.Find("hand_right").GetComponent(typeof(shot));
        other.AddAmmo();
    }
    //check if full
    public bool isPlaneFull(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            for (int z = 0; z < l; ++z)
            {
                if (grid[x, y, z] == null)
                    return false;
            }
        }
        audioSound.PlayOneShot(clearAudio);
        return true;
    }

    //delete the full level of y, then decrease upper levels
    public void deleteFullPlane()
    {
        for (int y = 0; y < h; ++y)
        {
            if (isPlaneFull(y))
            {
                addToScore(5000);
                deletePlane(y);
                decreasePlaneAbove(y + 1);
                --y;
            }
        }
    }

    public void setScore(int score) {
        gameScore = score;
        updateScore();
    }

    public void addToScore(int score) {
        gameScore += score;
        updateScore();
    }

    public void updateScore() {
        GameObject.Find("ScoreText").GetComponent<TextMesh>().text = "Score: " + gameScore;
    }
}
