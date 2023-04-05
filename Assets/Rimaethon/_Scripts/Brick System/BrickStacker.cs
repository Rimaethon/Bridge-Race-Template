using System.Collections.Generic;
using Rimaethon._Scripts.Core;
using Rimaethon._Scripts.ObjectManagers;
using UnityEngine;

namespace Rimaethon._Scripts.Brick_System
{
    public class BrickStacker : MonoBehaviour
    {
        #region Fields

        private ITypeDeterminer.ColorEnum _characterColor;
        private int _brickCount;
        private GameObject _brickHolder;
        private List<GameObject> _bricksOnCharacter;
        private GameObject _collidedStair;
        private MpbController _stairsMpbController;
        private IObjectPool _objectPool;

        #endregion

        #region Unity Methods

        private void Start()
        {   
            _objectPool = FindObjectOfType<ObjectPool>(); // Initialize object pool

            _characterColor = GetComponent<ITypeDeterminer>().ColorType;
            _brickHolder = transform.GetChild(1).gameObject;
            _bricksOnCharacter = new List<GameObject>();
            
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Brick"))
            {
                CollectBrick(other.gameObject);
            }
            else if (other.gameObject.CompareTag("Stair"))
            {
                PutBrickToStair(other.gameObject);
            }
        }

        #endregion

        #region Custom Methods


        
        private void CollectBrick(GameObject brick)
        {
            var brickColor = brick.GetComponent<ITypeDeterminer>().ColorType;
            if (_characterColor == brickColor)
            {
                _objectPool.RemoveFromPooledBricks(brickColor,brick,BrickStatus.PooledBrickStatus.Active);
                brick.transform.position = _brickHolder.transform.position + new Vector3(0, 0.105f * _bricksOnCharacter.Count, 0);
                brick.transform.parent = _brickHolder.transform;
                brick.transform.localRotation = Quaternion.Euler(0, 0, 0);
                brick.GetComponent<Collider>().enabled = false;
                _bricksOnCharacter.Add(brick);
                
                
                
            }
        }

        private void PutBrickToStair(GameObject stair)
        {
            _stairsMpbController = stair.GetComponent<MpbController>();
            var stairColor = _stairsMpbController.ColorType;
            
            if (_bricksOnCharacter.Count == 0 && _characterColor != stairColor)
            {
                Debug.Log("since"+_characterColor+" is not equal to  "+stairColor);
                gameObject.GetComponent<CharacterController>().stepOffset = 0.15f;
                return;
            }
            gameObject.GetComponent<CharacterController>().stepOffset = 0.35f;
            if (_characterColor != stairColor)
            {
                _stairsMpbController.SetObjectsColor(_characterColor);
                stair.GetComponent<MeshRenderer>().enabled = true;
                var lastBrick = _bricksOnCharacter[_bricksOnCharacter.Count - 1];
                lastBrick.transform.parent = null;
                _objectPool.ReturnBrickToPool(lastBrick);
                _bricksOnCharacter.RemoveAt(_bricksOnCharacter.Count - 1);
            }

           
        }

        #endregion
    }
}
