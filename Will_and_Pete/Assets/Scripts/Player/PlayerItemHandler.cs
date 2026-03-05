using System;
using Assets.Scripts.Player;
using Items;
using UnityEngine;

namespace Player
{
    public class PlayerItemHandler : MonoBehaviour
    {
        private PlayerController player;
        [SerializeField] private LayerMask itemLayer;
        [SerializeField] private Transform itemHoldTransform;


        private Item _currentItem;
        private Item _itemInVicinity;

        public void Initialize(PlayerController player)
        {
            this.player = player;
            player.PlayerInput.InteractEvent += OnInteract;
            player.PlayerInput.AbilityOneEvent += TryThrow;
        }

        private void TryThrow()
        {
            if (_currentItem == null)
            {
                return;
            }

            Vector2 dir;
            dir = player.PlayerInput.AimingInput != Vector2.zero ? player.PlayerInput.AimingInput : Vector2.up;
            
            _currentItem.Throw(dir,player.PlayerSettings.ThrowForce);
            _currentItem = null;
        }

        private void OnInteract()
        {
            if (_itemInVicinity != null && _itemInVicinity != _currentItem)
            {
                _currentItem?.Drop();
                _currentItem = null;
                PickUpItem(_itemInVicinity);
                return;
            }
            
            if (_currentItem)
            {
                _currentItem.Drop();
                _currentItem = null;
            }
        }
       
        private void FixedUpdate()
        {
            CheckForItem(out _itemInVicinity);
        }

        private void PickUpItem(Item itemInVicinity)
        {
            if (_currentItem != null)
            {
                _currentItem.Drop();
                _currentItem = null;
            }

            _currentItem = itemInVicinity.Pickup(player);
            _currentItem.transform.parent = itemHoldTransform;
            _currentItem.transform.localPosition = Vector2.zero + _currentItem.CarryOffset;
        }

        private void CheckForItem(out Item item)
        {
            item = null;
            Collider2D check = Physics2D.OverlapCircle(transform.position, 0.7f, itemLayer);
            bool result = check && check.TryGetComponent<Item>(out item);
            if (item == _currentItem)
            {
                item = null;
            }
        }
    }
}