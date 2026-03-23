
using UnityEngine;
using UnityEngine.UI;
using TMPro;
 // Added namespace

    public class CharacterSkinItemUI : MonoBehaviour
    {
        public TextMeshProUGUI characterNameText;
        public TextMeshProUGUI priceText;
        public Button purchaseButton;
        public Button selectButton;
        public RawImage previewRenderImage;
        public Model360Viewer modelViewer; // Reference to the viewer script

        private CharacterSkin skin;
        private GameObject previewInstance;

        public void Setup(CharacterSkin characterSkin)
        {
            this.skin = characterSkin;
            characterNameText.text = skin.characterName;

            if (CharacterSkinManager.Instance.IsSkinUnlocked(skin))
            {
                ShowSelectButton();
            }
            else
            {
                ShowPurchaseButton();
            }
            
            SetupPreview();
        }

        private void ShowPurchaseButton()
        {
            purchaseButton.gameObject.SetActive(true);
            selectButton.gameObject.SetActive(false);
            priceText.text = skin.price.ToString() + " " + skin.unlockType.ToString();

            purchaseButton.onClick.RemoveAllListeners();
            purchaseButton.onClick.AddListener(OnPurchaseButtonClicked);
        }

        private void ShowSelectButton()
        {
            purchaseButton.gameObject.SetActive(false);
            selectButton.gameObject.SetActive(true);
            priceText.text = "Unlocked";

            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(OnSelectButtonClicked);
        }

        private void OnPurchaseButtonClicked()
        {
            if (CharacterSkinManager.Instance.UnlockSkin(skin))
            {
                ShowSelectButton();
            }
        }

        private void OnSelectButtonClicked()
        {
            CharacterSkinManager.Instance.SetSelectedSkin(skin);
        }
        
        private void SetupPreview()
        {
             if (previewInstance != null) Destroy(previewInstance);
             previewInstance = Instantiate(skin.previewPrefab);

             if (modelViewer != null)
             {
                modelViewer.target = previewInstance.transform;
             }
             
             // The rest of the setup for using a RenderTexture with a separate camera
             // would be done in the Unity Editor.
        }

        private void OnDestroy()
        {
            if(previewInstance != null) Destroy(previewInstance);
        }
    }

