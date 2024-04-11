using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam;

    [SerializeField] private float xParallaxEffect;
    [SerializeField] private float yParallaxEffect;

    private float xPosition;
    private float yPosition;
    private Vector3 lastPosition;

    private float lengthXImage;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        lengthXImage = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;
        yPosition = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastPosition == cam.transform.position) return;

        float distanceMoved = cam.transform.position.x * (1 - xParallaxEffect);

        lastPosition = cam.transform.position;
        float distanceX = cam.transform.position.x * xParallaxEffect;
        float distanceY = cam.transform.position.y * yParallaxEffect;

        transform.position = new Vector3(xPosition + distanceX,yPosition+distanceY );

        if(distanceMoved > xPosition + lengthXImage)
        {
            xPosition = xPosition + lengthXImage;
        }
        else if (distanceMoved < xPosition - lengthXImage)
        {
            xPosition = xPosition - lengthXImage;
        }
    }
}
