using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Duplicate : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private Transform Handle;
    [SerializeField]
    private float offsetX = 0f;
    [SerializeField]
    private float offsetY = 0f;
    [SerializeField]
    private float offsetZ = 0f;
    [SerializeField]
    private float delay = 0f;

    #endregion Fields

    private Quaternion startRotation;
    private Transform startParent;
    private Vector3 startPosition;

    private bool _hasBeenDuplicated = false;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = new Vector3(Handle.transform.position.x + offsetX,
                                    Handle.transform.position.y + offsetY,
                                    Handle.transform.position.z + offsetZ);
        startRotation = transform.rotation;
        startParent = transform.parent;
        Debug.Log($"Zahlenwelten [Duplicate]: x: {startPosition.x}, y: {startPosition.y}, z: {startPosition.z}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoDuplicate()
    {
        StartCoroutine(DuplicateCoroutine());
    }
    private IEnumerator DuplicateCoroutine()
    {
        for (int i = 0; i < 2; i++)
        {
            if (i == 1 && !_hasBeenDuplicated)
            {
                _hasBeenDuplicated = true;
                var duplicate = Instantiate(this);
                duplicate.transform.localScale = new Vector3(50, 50, 50);
                duplicate.transform.position = startPosition;
                duplicate.transform.rotation = startRotation;
                duplicate.transform.parent = startParent;
                duplicate.GetComponentInChildren<NumberBalloon>(true)?.EnableGrab();
                Debug.Log($"Zahlenwelten [Duplicate]: x: {duplicate.transform.position.x}, y: {duplicate.transform.position.y}, z: {duplicate.transform.position.z}");
            }
            yield return new WaitForSeconds(delay);
        }
    }

}
