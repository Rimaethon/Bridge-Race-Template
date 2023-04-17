using System;
using System.Collections.Generic;
using Rimaethon._Scripts.Core;
using Rimaethon._Scripts.Core.Enums;
using Rimaethon._Scripts.Core.Interfaces;
using Rimaethon._Scripts.Managers;
using UnityEngine;

namespace Rimaethon._Scripts.ObjectManagers
{
    public class ObjectPool : MonoBehaviour,IObjectPool
    {

        
        [SerializeField] private int poolSize = 20;
        [SerializeField] private GameObject brickPrefab;
        private List<ColorEnum> _brickTypeList;
        private PlatformStates _platformStates;

        private void OnEnable()
        {

        }

        private void Awake()
        {

            InstantiateObjects(poolSize);
            _platformStates = PlatformStates.StartingPlatform;
        }
        

        private void InstantiateObjects(int objectPoolSize)
        {
            
            _brickTypeList =SceneDataHolder.CharactersTypesOnLevels[PlatformStates.StartingPlatform];

            foreach (ColorEnum color in _brickTypeList)
            {
                for (int i = 0; i < objectPoolSize; i++)
                {
                    GameObject brickObject = Instantiate(brickPrefab);
                    brickObject.GetComponent<MpbController>().ColorType = color;
                    brickObject.SetActive(false);
                    Debug.Log("I created a brick with " + color + "color");
                    AddToPooledBricks(color, brickObject, BrickStatus.PooledBrickStatus.NotActive);
                }
            }
            Debug.Log("yes ı finished instantiation but ı dont broadcast fucker");
            EventBroadcaster.Broadcast<PlatformStates>(EventManager.Instance,GameStates.OnObjectsInstantiated,PlatformStates.StartingPlatform);
            
        }

        

        public GameObject GetBrickFromPool(ColorEnum colorType)
        {
            if (SceneDataHolder.PooledBrickDictionary[BrickStatus.PooledBrickStatus.NotActive][colorType].Count == 0)
            {
                
                return null;
            }

            GameObject pooledBrick = SceneDataHolder.PooledBrickDictionary[BrickStatus.PooledBrickStatus.NotActive][colorType][0];
            RemoveFromPooledBricks(colorType,pooledBrick,BrickStatus.PooledBrickStatus.NotActive);
            AddToPooledBricks(colorType, pooledBrick, BrickStatus.PooledBrickStatus.Active);
            Debug.Log("I made"+colorType+" brick "+pooledBrick.transform.position+" in this position");
            return pooledBrick;
        }

        public void ReturnBrickToPool(GameObject brick)
        {
            brick.SetActive(false);
            ColorEnum color = brick.GetComponent<MpbController>().ColorType;
            AddToPooledBricks(color, brick, BrickStatus.PooledBrickStatus.NotActive);
        }
        
        public void AddToPooledBricks(ColorEnum color, GameObject brick, BrickStatus.PooledBrickStatus brickStatus)
        {
            if (!SceneDataHolder.PooledBrickDictionary[brickStatus].ContainsKey(color))
            {
                SceneDataHolder.PooledBrickDictionary[brickStatus].Add(color, new List<GameObject>());
            }

        
            SceneDataHolder.PooledBrickDictionary[brickStatus][color].Add(brick);
        }

        public void RemoveFromPooledBricks(ColorEnum color, GameObject brick,
            BrickStatus.PooledBrickStatus brickStatus)
        {
            if (!SceneDataHolder.PooledBrickDictionary[brickStatus].ContainsKey(color))
            {
                SceneDataHolder.PooledBrickDictionary[brickStatus].Add(color, new List<GameObject>());
            }
            SceneDataHolder.PooledBrickDictionary[brickStatus][color].Remove(brick);
            Debug.Log("I removed"+brick+" with " +color);
        }
    }
}



