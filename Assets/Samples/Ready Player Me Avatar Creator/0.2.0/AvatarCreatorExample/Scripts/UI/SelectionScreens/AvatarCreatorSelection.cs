﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ReadyPlayerMe.AvatarCreator;
using ReadyPlayerMe.AvatarLoader;
using UnityEngine;
using UnityEngine.UI;

namespace ReadyPlayerMe
{
    public class AvatarCreatorSelection : State, IDisposable
    {
        private const string UPDATING_YOUR_AVATAR_LOADING_TEXT = "Updating your avatar";

        [SerializeField] private AssetTypeUICreator assetTypeUICreator;
        [SerializeField] private AssetButtonCreator assetButtonCreator;
        [SerializeField] private Button saveButton;
        [SerializeField] private AvatarConfig inCreatorConfig;
        [SerializeField] private RuntimeAnimatorController animator;

        private PartnerAssetsManager partnerAssetManager;
        private AvatarManager avatarManager;
        private GameObject currentAvatar;
        private Quaternion lastRotation;
        private CancellationTokenSource ctxSource;

        public override StateType StateType => StateType.Editor;
        public override StateType NextState => StateType.End;

        private void OnEnable()
        {
            saveButton.onClick.AddListener(OnSave);
            Setup();
        }

        private void OnDisable()
        {
            saveButton.onClick.RemoveListener(OnSave);
            Cleanup();
        }

        private async void Setup()
        {
            LoadingManager.EnableLoading();
            ctxSource = new CancellationTokenSource();
            if (!AuthManager.IsSignedIn)
            {
                await AuthManager.LoginAsAnonymous();
            }

            avatarManager = new AvatarManager(
                AvatarCreatorData.AvatarProperties.BodyType,
                AvatarCreatorData.AvatarProperties.Gender,
                inCreatorConfig,
                ctxSource.Token);
            avatarManager.OnError = OnErrorCallback;

            await LoadAssets();
            currentAvatar = await LoadAvatar();

            if (string.IsNullOrEmpty(avatarManager.AvatarId))
                return;

            await LoadAvatarColors();
            LoadingManager.DisableLoading();
        }

        private void Cleanup()
        {
            if (currentAvatar != null)
            {
                Destroy(currentAvatar);
            }
            saveButton.gameObject.SetActive(false);

            Dispose();
            assetTypeUICreator.ResetUI();
        }

        private void OnErrorCallback(string error)
        {
            avatarManager.OnError = null;
            partnerAssetManager.OnError = null;

            ctxSource?.Cancel();
            StateMachine.Back();
            LoadingManager.EnableLoading(error, LoadingManager.LoadingType.Popup, false);
        }

        private async Task LoadAssets()
        {
            var startTime = Time.time;
            partnerAssetManager = new PartnerAssetsManager(
                AvatarCreatorData.AvatarProperties.Partner,
                AvatarCreatorData.AvatarProperties.BodyType,
                AvatarCreatorData.AvatarProperties.Gender,
                ctxSource.Token);

            partnerAssetManager.OnError = OnErrorCallback;

            var assetIconDownloadTasks = await partnerAssetManager.GetAllAssets();

            CreateUI(AvatarCreatorData.AvatarProperties.BodyType, assetIconDownloadTasks);
            partnerAssetManager.DownloadAssetsIcon(assetButtonCreator.SetAssetIcons);
            DebugPanel.AddLogWithDuration("Got all partner assets", Time.time - startTime);
        }

        private async Task<GameObject> LoadAvatar()
        {
            var startTime = Time.time;

            GameObject avatar;

            if (string.IsNullOrEmpty(AvatarCreatorData.AvatarProperties.Id))
            {
                AvatarCreatorData.AvatarProperties.Assets ??= GetDefaultAssets();
                avatar = await avatarManager.Create(AvatarCreatorData.AvatarProperties);
            }
            else
            {
                avatar = await avatarManager.GetAvatar(AvatarCreatorData.AvatarProperties.Id);
            }

            if (avatar == null)
            {
                return null;
            }

            AvatarCreatorData.AvatarProperties.Id = avatarManager.AvatarId;

            ProcessAvatar(avatar);

            DebugPanel.AddLogWithDuration("Avatar loaded", Time.time - startTime);
            return avatar;
        }

        private async Task LoadAvatarColors()
        {
            var startTime = Time.time;
            var avatarAPIRequests = new AvatarAPIRequests();
            ColorPalette[] colors = null;
            try
            {
                colors = await avatarAPIRequests.GetAllAvatarColors(AvatarCreatorData.AvatarProperties.Id);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            if (colors == null)
            {
                return;
            }
            assetButtonCreator.CreateColorUI(colors, UpdateAvatar);
            DebugPanel.AddLogWithDuration("All colors loaded", Time.time - startTime);
        }

        private void CreateUI(BodyType bodyType, Dictionary<string, AssetType> assets)
        {
            assetTypeUICreator.CreateUI(bodyType, AssetTypeHelper.GetAssetTypeList(bodyType));
            assetButtonCreator.CreateAssetButtons(assets, UpdateAvatar);
            assetButtonCreator.CreateClearButton(UpdateAvatar);
            assetButtonCreator.SetSelectedAssets(AvatarCreatorData.AvatarProperties.Assets);
            saveButton.gameObject.SetActive(true);
        }

        private async void OnSave()
        {
            var startTime = Time.time;
            var avatarId = await avatarManager.Save();
            AvatarCreatorData.AvatarProperties.Id = avatarId;
            DebugPanel.AddLogWithDuration("Avatar saved", Time.time - startTime);
            StateMachine.SetState(StateType.End);
        }

        private Dictionary<AssetType, object> GetDefaultAssets()
        {
            if (string.IsNullOrEmpty(AvatarCreatorData.AvatarProperties.Base64Image))
            {
                return AvatarCreatorData.AvatarProperties.Gender == OutfitGender.Feminine
                    ? AvatarPropertiesConstants.FemaleDefaultAssets
                    : AvatarPropertiesConstants.MaleDefaultAssets;
            }

            return new Dictionary<AssetType, object>();
        }

        private async void UpdateAvatar(object assetId, AssetType assetType)
        {
            var startTime = Time.time;

            var payload = new AvatarProperties
            {
                Assets = new Dictionary<AssetType, object>()
            };

            payload.Assets.Add(assetType, assetId);
            lastRotation = currentAvatar.transform.rotation;
            LoadingManager.EnableLoading(UPDATING_YOUR_AVATAR_LOADING_TEXT, LoadingManager.LoadingType.Popup);
            var avatar = await avatarManager.UpdateAsset(assetType, assetId);
            if (avatar == null)
            {
                return;
            }

            ProcessAvatar(avatar);
            currentAvatar = avatar;
            LoadingManager.DisableLoading();
            DebugPanel.AddLogWithDuration("Avatar updated", Time.time - startTime);
        }

        private void ProcessAvatar(GameObject avatar)
        {
            if (AvatarCreatorData.AvatarProperties.BodyType == BodyType.FullBody)
            {
                avatar.GetComponent<Animator>().runtimeAnimatorController = animator;
            }
            avatar.transform.rotation = lastRotation;
            avatar.AddComponent<RotateAvatar>();
        }

        public void Dispose()
        {
            partnerAssetManager?.Dispose();
            avatarManager?.Dispose();
        }
    }
}
