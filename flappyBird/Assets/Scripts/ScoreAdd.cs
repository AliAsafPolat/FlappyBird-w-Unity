using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Borulardan gecildiginde kuslarin skoru artar.
public class ScoreAdd : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<FB_script>().increaseScore();
    }
}
