using System.Collections;
using System.Collections.Generic;
using Extensions;
using PathCreation;
using Sirenix.OdinInspector;
using UnityEngine;

public class follower : MonoBehaviour
{

    [SerializeField] private PathCreator _pathCreator;
    [SerializeField] private Vector3 _up = Vector3.forward;
    private float dist;
    public bool isUpdate = true;
    [SerializeField] private float _speed =1;

    // Update is called once per frame
    void Update()
    {
        if(!isUpdate)return;
        dist += _speed * Time.deltaTime;
        transform.position = _pathCreator.path.GetPointAtDistance(dist);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, _pathCreator.path.GetDirectionAtDistance(dist));
    }

}
