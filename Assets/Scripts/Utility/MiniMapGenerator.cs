using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapGenerator : MonoBehaviour {

    GameObject levelLayout;

	// Use this for initialization
	void Start ()
    { 
        levelLayout = Instantiate(GameObject.Find("LevelLayout"));
        levelLayout.name = "Mimimap";
        levelLayout.transform.SetParent(gameObject.transform);
        RemoveAllComponents();
        levelLayout.transform.localScale = new Vector3(25, 25, 25);
        levelLayout.transform.localPosition = new Vector3(0,0,0);
    }

    void RemoveAllComponents()
    {
        foreach (var comp in levelLayout.GetComponentsInChildren<Component>())
        {
            //Remove the colliders in the Vent Objects
            if(comp.name == "Vent_E(Clone)" || comp.name == "Vent TEST(Clone)")
                foreach (Transform child in comp.gameObject.transform)
                    Destroy(child.gameObject);

            //Do not delete Image or Transform components
            if (comp is Transform || comp is Image)
                continue;

            //Change SpriteRenderers to Image
            else if ((comp is SpriteRenderer))
            {
                SpriteRenderer sr = comp as SpriteRenderer;
                Image img = comp.gameObject.AddComponent<Image>();
                img.sprite = sr.sprite;
                img.color = sr.color;
                Destroy(comp);
            }

            //Destory any other components
            else
                Destroy(comp);
        }
    }
}
