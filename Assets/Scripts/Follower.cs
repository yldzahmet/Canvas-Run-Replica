using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Follower : MonoBehaviour
{
    private MaterialPropertyBlock mPropertyBlock;
    private Renderer renderer;
    private int colorPropertyId;
    public static int gradientLength = 40;
    internal int gradientColorIndex = 0;


    internal PoolController poolController;
    internal GameObject head;   // Gameobject to following
    internal Rigidbody rigidBody;
    internal bool isFollowing = true;   // Is this object currently following head
    static float spreadMultipler = 100;
    internal bool isGoingThrough;

    private void Awake()
    {
        poolController = GameObject.Find("Controller").GetComponent<PoolController>();
        rigidBody = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();
        colorPropertyId = Shader.PropertyToID("_Color");
        mPropertyBlock = new MaterialPropertyBlock();
    }

    // Hide object and make ready to use again
    public void ReturnToPool()
    {
        if (head.CompareTag("Driver"))
        {
            // If this object following Driver object
            head.GetComponent<DriversManager>().isPullingBall = false;
        }
        isFollowing = false;
        poolController.InsertToPool(gameObject);
        transform.localPosition = new Vector3(0, 0, 0);
        gameObject.SetActive(false);
    }

    //  Calling from update every frame
    public void FollowHead()
    {
        transform.position = Vector3.Lerp(transform.position, head.transform.position, 25 * Time.deltaTime);
    }

    public void GoThrough()
    {
        // move the head object forward at a constant rate
        rigidBody.MovePosition(transform.position + Vector3.forward * 5 * Time.deltaTime);
    }

    public Vector3 GetRandomLimitedDirection(float multipler)
    {
        Vector3 vec = new Vector3(
                Random.Range(-0.4f, 0.4f),
                Random.Range(0.4f, 1f),
                Random.Range(1.4f, 1.7f)) * multipler;
        return vec;
    }

    public void SetColor(int index, Color color1, Color color2)
    {
        index %= gradientLength;
        renderer.GetPropertyBlock(mPropertyBlock);
        mPropertyBlock.SetColor(colorPropertyId, Color.Lerp(color1, color2, (float)index / gradientLength));
        renderer.SetPropertyBlock(mPropertyBlock);
        gradientColorIndex = index;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            isFollowing = false;
            GetComponent<Rigidbody>().AddForce(GetRandomLimitedDirection(spreadMultipler), ForceMode.Impulse);
            StartCoroutine(GoThroughDelayed());
        }
        if(other.CompareTag("Multipler"))
        {
            isGoingThrough = false;
        }
    }

    IEnumerator GoThroughDelayed()
    {
        yield return new WaitForSeconds(1f);
        isGoingThrough = true;
    }

    // Update is called once per frame
    void Update()
    {   
        // If pressed play button
        if (Manager.startDriving){
            if (isFollowing)
            {
                FollowHead();
            }
            else if (isGoingThrough)
            {
                GoThrough();
            }
        }

        // If our head object is not active in hierarchy then go to pool
        if (!head.activeInHierarchy)
        {
            ReturnToPool();
        }
    }
}

