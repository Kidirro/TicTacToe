using System.Collections.Generic;
using Emotes.Interfaces;
using Network.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Emotes
{
    public class EmoteManager:MonoBehaviour, IEmoteService
    {
        [Header("Properties"), SerializeField]
        private Animator _emoteAnimator;
        [SerializeField]
        private Image _emoteImage;

        private Dictionary<int, Sprite> _loadedSprites = new();
        private const string RESOURCE_PATH = "Emotes/Emote_{0}";

        #region Dependency

        private IEmotesEventNetworkService _emotesEventNetworkService;
        
        [Inject]
        private void Construct(IEmotesEventNetworkService emotesEventNetworkService)
        {
            _emotesEventNetworkService = emotesEventNetworkService;
        }

        #endregion
 

        public void ShowEmote(int id)
        {
            _emoteAnimator.gameObject.SetActive(false);
            Sprite currentSprite;
            if (!_loadedSprites.TryGetValue(id, out currentSprite))
            {
                currentSprite = Resources.Load<Sprite>(string.Format(RESOURCE_PATH, id));
                Debug.Log("LOADED RESOURCES!!!");
                _loadedSprites[id] = currentSprite;
            }

            _emoteImage.sprite = currentSprite;
            _emoteAnimator.gameObject.SetActive(true);
        }

        public void SendEmote(int id)
        {
            _emotesEventNetworkService.RaiseEventShowEmote(id);
            ShowEmote(id);
        }
    }
}