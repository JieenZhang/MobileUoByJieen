using ClassicUO;
using ClassicUO.Configuration;
using ClassicUO.Game.Scenes;
using ClassicUO.IO.Resources;
using ClassicUO.Network;
using ClassicUO.Utility.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class MainCamera : MonoBehaviour
    {
        [SerializeField]
        private bool useDynamicAtlas;
        [SerializeField]
        public bool useGraphicsDrawTexture;


        [Header("Controls")]
        [SerializeField]
        private MobileJoystick movementJoystick;

        [SerializeField]
        private Canvas inGameDebugConsoleCanvas;

        [SerializeField]
        private bool scaleGameToFitScreen;
        public bool ScaleGameToFitScreen
        {
            get => scaleGameToFitScreen;
            set
            {
                scaleGameToFitScreen = value;
                ApplyScalingFactor();
                //Force update game viewport render texture
                if (Client.Game.Scene is GameScene gameScene)
                {
                    gameScene.GetViewPort();
                }
            }
        }

        private int lastScreenWidth;
        private int lastScreenHeight;

        private void Awake()
        {
            ConsoleRedirect.Redirect();
        }

        // Start is called before the first frame update
        void Start()
        {
            StartGame();
        }

        // Update is called once per frame
        void Update()
        {
            if (Client.Game == null)
                return;

            if (lastScreenWidth != Screen.width || lastScreenHeight != Screen.height)
            {
                lastScreenWidth = Screen.width;
                lastScreenHeight = Screen.height;
                //Force update ScaleGameToFitScreen
                ScaleGameToFitScreen = scaleGameToFitScreen;
            }

            float deltaTime = UnityEngine.Time.deltaTime;
            //Is this necessary? Wouldn't it slow down the game even further when it dips below 20 FPS?
            if (deltaTime > 0.050f)
            {
                deltaTime = 0.050f;
            }

            Client.Game.Tick(deltaTime);
        }

        private void OnDisable()
        {
            AnimationsLoader._instance?.Dispose();
            AnimDataLoader._instance?.Dispose();
            ArtLoader._instance?.Dispose();
            MapLoader._instance?.Dispose();
            ClilocLoader._instance?.Dispose();
            GumpsLoader._instance?.Dispose();
            FontsLoader._instance?.Dispose();
            HuesLoader._instance?.Dispose();
            TileDataLoader._instance?.Dispose();
            MultiLoader._instance?.Dispose();
            SkillsLoader._instance?.Dispose();
            TexmapsLoader._instance?.Dispose();
            SpeechesLoader._instance?.Dispose();
            LightsLoader._instance?.Dispose();
            SoundsLoader._instance?.Dispose();
            MultiMapLoader._instance?.Dispose();
            ProfessionLoader._instance?.Dispose();
        }

        private void OnPostRender()
        {
            if (Client.Game == null)
                return;

            GL.LoadPixelMatrix(0, Screen.width, Screen.height, 0);

            Client.Game.Batcher.UseDynamicAtlas = useDynamicAtlas;
            Client.Game.Batcher.UseGraphicsDrawTexture = useGraphicsDrawTexture;
            Client.Game.DrawUnity(UnityEngine.Time.deltaTime);
            Client.Game.Batcher.Reset();

        }

        public void StartGame()
        {
            string siteName = "uooutlands";
            CUOEnviroment.ExecutablePath = Path.Combine(Application.persistentDataPath, siteName);

            if (!Directory.Exists(CUOEnviroment.ExecutablePath))
            {
                Directory.CreateDirectory(CUOEnviroment.ExecutablePath);
            }

            Log.Start(LogTypes.All);
            //var settingsFilePath = Settings.GetSettingsFilepath();
            //if (File.Exists(settingsFilePath))
            //{
            //    Log.Debug("settingsFilePath true");
            //    Settings.GlobalSettings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(settingsFilePath));
            //}
            //else
            //{
            //    Log.Debug("settingsFilePath false");
            //    Settings.GlobalSettings = JsonConvert.DeserializeObject<Settings>(Resources.Load<TextAsset>("settings").text);
            //}
            Settings.GlobalSettings.LastServerNum = 1;
            Settings.GlobalSettings.ClilocFile = "Cliloc.enu";
            Settings.GlobalSettings.FPS = 60;

            Settings.GlobalSettings.IP = "play.uooutlands.com";
            Settings.GlobalSettings.Port = 2593;

            //errorText.text = CUOEnviroment.ExecutablePath;


            //Reset static encryption type variable
            EncryptionHelper.Type = ENCRYPTION_TYPE.NONE;
            Settings.GlobalSettings.Encryption = 0; //(byte)(config.UseEncryption ? 1 : 0);

            //Empty the plugins array because no plugins are working at the moment
            Settings.GlobalSettings.Plugins = new string[0];

            //If connecting to UO Outlands, set shard type to 2 for outlands
            Settings.GlobalSettings.ShardType = 2;// config.UoServerUrl.ToLower().Contains("uooutlands") ? 2 : 0;

            //Try to detect old client version to set ShardType to 1, for using StatusGumpOld. Otherwise, it's possible
            //to get null-refs in StatusGumpModern.
            //if (ClientVersionHelper.IsClientVersionValid(config.ClientVersion, out var clientVersion))
            //{
            //    if (clientVersion < ClientVersion.CV_308Z)
            //    {
            //        Settings.GlobalSettings.ShardType = 1;
            //    }
            //}

            Settings.GlobalSettings.ClientVersion = "7.0.15.1";

            CUOEnviroment.IsOutlands = Settings.GlobalSettings.ShardType == 2;

            Settings.GlobalSettings.UltimaOnlineDirectory = CUOEnviroment.ExecutablePath;

            //This flag is tied to whether the GameCursor gets drawn, in a convoluted way
            //On mobile platform, set this flag to true to prevent the GameCursor from being drawn
            Settings.GlobalSettings.RunMouseInASeparateThread = Application.isMobilePlatform;

            //Some mobile specific overrides need to be done on the Profile but they can only be done once the Profile has been loaded
            ProfileManager.ProfileLoaded += OnProfileLoaded;
            Log.Trace("Start AudioSource");
            MediaPlayer.AudioSourceOneShot = gameObject.AddComponent<AudioSource>();


            try
            {
                Client.Run();


                //Client.Game.sceneChanged += OnSceneChanged;
                //Client.Game.Exiting += OnGameExiting;
                ApplyScalingFactor();
            }
            catch (Exception e)
            {
                //OnError?.Invoke(e.ToString());
                Log.Debug(e.ToString());
            }
        }

        private void OnProfileLoaded()
        {
            //Disable XBR as MobileUO does not yet support that effect
            ProfileManager.Current.UseXBR = false;
            //Disable auto move on mobile platform
            ProfileManager.Current.DisableAutoMove = Application.isMobilePlatform;
            //Prevent stack split gump from appearing on mobile
            //ProfileManager.Current.HoldShiftToSplitStack = Application.isMobilePlatform;
            //Scale items inside containers by default on mobile (won't have any effect if container scale isn't changed)
            ProfileManager.Current.ScaleItemsInsideContainers = Application.isMobilePlatform;
        }

        private void ApplyScalingFactor()
        {
            var scale = 1f;

            if (Client.Game == null)
            {
                return;
            }

            var isGameScene = Client.Game.Scene is GameScene;

            if (ScaleGameToFitScreen)
            {
                var loginScale = Mathf.Min(Screen.width / 640f, Screen.height / 480f);
                var gameScale = Mathf.Max(1, loginScale * 0.75f);
                scale = isGameScene ? gameScale : loginScale;
            }

            //if (UserPreferences.CustomScaleSize != UserPreferences.ScaleSizes.Default && isGameScene)
            //{
            //    scale *= (int)UserPreferences.CustomScaleSize / 100f;
            //}

            ((UnityGameWindow)Client.Game.Window).Scale = scale;
            Client.Game.Batcher.scale = scale;
            Client.Game.scale = scale;
        }
    }
}
