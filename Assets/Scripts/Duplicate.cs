using System.Collections;
using UnityEngine;

/// <summary>
/// Duplicate a grabbable number balloon
/// </summary>
public class Duplicate : MonoBehaviour
{
    #region Fields

    private static Vector3 VECTOR_50 =
        new()
        {
                    x = 50,
                    y = 50,
                    z = 50
        };

    [SerializeField]
    private Transform _handle;
    [SerializeField]
    private float _offsetX = 0f;
    [SerializeField]
    private float _offsetY = 0f;
    [SerializeField]
    private float _offsetZ = 0f;
    [SerializeField]
    private float _delay = 0f;

    private Quaternion _startRotation;
    private Transform _startParent;
    private Vector3 _startPosition;
    private bool _hasBeenDuplicated = false;

    #endregion Fields

    void Start()
    {
        _startPosition = new Vector3
        {
            x = _handle.transform.position.x + _offsetX,
            y = _handle.transform.position.y + _offsetY,
            z = _handle.transform.position.z + _offsetZ
        };
        _startRotation = transform.rotation;
        _startParent = transform.parent;
    }

    public void DoDuplicate()
    {
        StartCoroutine(DuplicateCoroutine());
    }

    /// <summary>
    /// Duplicate the grabbable after _delay seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator DuplicateCoroutine()
    {
        for (int i = 0; i < 2; i++)
        {
            if (i == 1 && !_hasBeenDuplicated)
            {
                _hasBeenDuplicated = true;
                var duplicate = Instantiate(this);
                var trans = duplicate.transform;
                trans.localScale = VECTOR_50;
                trans.position = _startPosition;
                trans.rotation = _startRotation;
                trans.parent = _startParent;
                duplicate.GetComponentInChildren<NumberBalloon>(true).EnableGrab();
            }
            yield return new WaitForSeconds(_delay);
        }
    }

}
