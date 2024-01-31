using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class slingshot : MonoBehaviour
{
    public string next_scene;
    [SerializeField] public LineRenderer[] lineRenderers;
    [SerializeField] public Transform[] stripPositions;
    [SerializeField] public Transform center;
    [SerializeField] public Transform idlePosition;
    Vector3 currpos;
    bool is_mouse_down = false;
    public float max_l;

    public GameObject rockprefab;
    float rockPositionOffset;
    Rigidbody2D rock;
    Collider2D rockCollider;
    public int pigs_amount = 0;
    public float force;

    public GameObject textmeshpro;
    TextMeshProUGUI textmp;
    // Start is called before the first frame update
    void Start()
    {
        textmp = textmeshpro.GetComponent<TextMeshProUGUI>();
        textmp.text = pigs_amount.ToString();
        lineRenderers[0].positionCount = 2;
        lineRenderers[1].positionCount = 2;
        lineRenderers[0].SetPosition(0, stripPositions[0].position);
        lineRenderers[1].SetPosition(0, stripPositions[1].position);

        CreateRock();
        ResetStrips();
    }

    void CreateRock()
    {
        rock = Instantiate(rockprefab).GetComponent<Rigidbody2D>();
        rockCollider = rock.GetComponent<Collider2D>();
        rockCollider.enabled = false;

        rock.isKinematic = true;

        ResetStrips();
    }


    private void OnMouseDown()
    {
        is_mouse_down = true;
    }
    private void OnMouseUp()
    {
        is_mouse_down = false;
        Shoot();
        ResetStrips();
    }
    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            SceneManager.LoadScene(next_scene);
        }
        
        if (is_mouse_down)
        {
            Vector3 mousepos = Input.mousePosition;
            mousepos.z = 10;

            mousepos = Camera.main.ScreenToWorldPoint(mousepos);
            currpos = mousepos;
            currpos = center.position + Vector3.ClampMagnitude(currpos
                - center.position, max_l);
            SetStrips(currpos);
            if (rockCollider)
            {
                rockCollider.enabled = true;
            }
        }

    }
    void ResetStrips()
    {
        currpos = idlePosition.position;
        SetStrips(idlePosition.position);
    }
    void SetStrips(Vector3 position)
    {
        lineRenderers[0].SetPosition(1, position);
        lineRenderers[1].SetPosition(1, position);

        if (rock)
        {
            rock.transform.position = position;

        }
    }
    void Shoot()
    {
        if (pigs_amount > 0)
        {
            pigs_amount--;
            textmp.text = pigs_amount.ToString();

            if (rock != null)
            {
                rock.isKinematic = false;
                Vector3 birdForce = (currpos - center.position) * force * -1;
                rock.velocity = birdForce;

                // Set bird and birdCollider to null after applying the force
                rock = null;
                rockCollider = null;
            }

            if (pigs_amount != 0)
            {
                Invoke("CreateRock", 1);
            }

            if (pigs_amount == 0)
            {
                StartCoroutine(WaitAndReloadScene(5f));
            }
        }
       

    }

    IEnumerator WaitAndReloadScene(float waitTime)
{
    // Wait for the specified seconds
    yield return new WaitForSeconds(waitTime);

    // Reload the scene after waiting
    ReloadScene();
}

void ReloadScene()
{
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}

}
