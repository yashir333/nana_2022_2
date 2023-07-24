using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{

    // for holding the materials for the globe
    public Material[] globeMats;
    // for setting what the current globe is so that the base can change, but the globe stays the same
    public int globeMatCounter = 0;
    // for holding the globe prefabs
    public GameObject[] globeBases;
    public int globeBaseCounter = 0;

    // the spawn location for the globes
    public GameObject spawnLoc;

    // the current globe
    public GameObject currGlobe;

    // This function moves base 1 forward. Pass into either the value -1 (decrement) or 1 increment the current globe base
    public void ChangeGlobeBase(int i)
    {
        if(i == 1)
        {
            if(globeBaseCounter < globeBases.Length - 1)
            {
                globeBaseCounter++;
            }
        } else
        {
            if(globeBaseCounter > 0)
            {
                globeBaseCounter--;
            }
        }

        // clear the current object
        foreach(Transform obj in spawnLoc.transform)
        {
            GameObject.Destroy(obj.gameObject);
        }

        // spawn the new one
        GameObject temp = Instantiate(globeBases[globeBaseCounter], spawnLoc.transform);
        temp.transform.parent = spawnLoc.transform;
        currGlobe = temp;
        ChangeGlobeMaterial(0);
        if(globeBaseCounter > 3)
        {
            currGlobe.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        } else
        {
            currGlobe.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }

    // This function changes the material attached to the globe
    public void ChangeGlobeMaterial(int i)
    {
        switch (i)
        {
            case 1:
                if(globeMatCounter < globeMats.Length - 1)
                {
                    globeMatCounter++;
                }
                break;

            case -1:
                if (globeMatCounter > 0)
                {
                    globeMatCounter--;
                }
                break;

            default:
                break;
        }

        // assign the right material to the current globe
        currGlobe.GetComponent<GlobeProperties>().globe.GetComponent<Renderer>().material = globeMats[globeMatCounter];

    }


}
