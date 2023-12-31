using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TLab.InputField
{
    #region SYNC_UTIL

    public class SyncUtility : MonoBehaviour
    {
        static protected SyncUtility m_Instance;

        static public SyncUtility instance
        {
            get
            {
                if (m_Instance == null)
                {
                    GameObject o = new GameObject("SyncHandler");
                    DontDestroyOnLoad(o);
                    m_Instance = o.AddComponent<SyncUtility>();
                }

                return m_Instance;
            }
        }

        public void OnDisable()
        {
            if (m_Instance)
            {
                Destroy(m_Instance.gameObject);
            }
        }

        public static Coroutine StartStaticCoroutine(IEnumerator coroutine)
        {
            return instance.StartCoroutine(coroutine);
        }

        public static IEnumerator AfterFrame(UnityEvent callback, int delay)
        {
            for (int i = 0; i < delay; i++)
            {
                yield return null;
            }

            callback.Invoke();
        }

        public static IEnumerator AfterFrame(UnityAction callback, int delay)
        {
            for (int i = 0; i < delay; i++)
            {
                yield return null;
            }

            callback.Invoke();
        }

        public static IEnumerator AfterSecound(UnityEvent callback, float delay)
        {
            yield return new WaitForSeconds(delay);
            callback.Invoke();
        }

        public static IEnumerator AfterSecound(UnityAction callback, float delay)
        {
            yield return new WaitForSeconds(delay);
            callback.Invoke();
        }
    }

    #endregion SYNC_UTIL

    #region AUDIO_UTIL

    public class AudioUtility
    {
        public static void ShotAudio(AudioSource audioSource, AudioClip audioClip, float delay)
        {
            if (audioSource != null && audioClip != null)
            {
                SyncUtility.StartStaticCoroutine(
                    SyncUtility.AfterSecound(() => { audioSource.PlayOneShot(audioClip, 1.0f); }, delay));
            }
        }
    }

    #endregion AUDIO_UTIL

    [RequireComponent(typeof(AudioSource))]
    public class TLabVKeyborad : MonoBehaviour
    {
        [Header("Key Audio")]
        [SerializeField] private AudioSource m_audioSource;
        [SerializeField] private AudioClip m_keyStroke;

        [Header("Key BOX")]
        [SerializeField] private GameObject m_keyBOX;
        [SerializeField] private GameObject m_romajiBOX;
        [SerializeField] private GameObject m_symbolBOX;
        [SerializeField] private GameObject m_operatorBOX;

        [Header("Transform Anchor")]
        [SerializeField] private Transform m_anchor;

        [Header("Hide Keyborad Callback")]
        [SerializeField] private UnityEvent<TLabVKeyborad, bool> m_onHide;

        [SerializeField, HideInInspector]
        private TLabInputFieldBase m_inputFieldBase;

        private System.Action m_updateKeybord;
        private bool m_isMobile = false;
        private float m_backSpaceKeyTime = 0.0f;
        private List<string> m_inputBuffer = new List<string>();

        private const string BACKSPACE = "BACKSPACE";
        private const string ENTER = "ENTER";
        private const string SHIFT = "SHIFT";
        private const string SPACE = "SPACE";
        private const string TAB = "TAB";
        private const string SYMBOL = "SYMBOL";

        private const float IMMEDIATELY = 0f;
        private const float BACKSPACE_DELAY = 0.1f;

        public bool isMobile => m_isMobile;

        public TLabInputFieldBase inputFieldBase => m_inputFieldBase;

        public void SwitchInputField(TLabInputFieldBase inputFieldBase) => m_inputFieldBase = inputFieldBase;

        // Keyboradを子階層に持つTransformを動かす
        // TODO: RectTransform対応
        public void SetTransform(Vector3 position, Vector3 target, Vector3 worldUp)
        {
            m_anchor.position = position;

            m_anchor.LookAt(target, worldUp);
        }

        public void HideKeyborad(bool active, bool callbackSync = false)
        {
            m_keyBOX.SetActive(!active);

            // callback

            if (callbackSync)
            {
                SyncUtility.AfterFrame(delegate { m_onHide.Invoke(this, active); }, 1);
            }
            else
            {
                m_onHide.Invoke(this, active);
            }
        }

        #region PLATFORM_UTIL

#if !UNITY_EDITOR && UNITY_WEBGL
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern bool IsMobile();
#endif

        private bool CheckIfMobile()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            isMobile = IsMobile();
#elif UNITY_ANDROID
            m_isMobile = true;
#else
            m_isMobile = false;
#endif
            return m_isMobile;
        }

        #endregion PLATFORM_UTIL

        void Awake()
        {
            // awakeからonHideを呼び出すとき，また目的のインスタンスが生成されていない可能性がある．
            // そのため，onHideを1フレーム待ってから実行する

            if (!CheckIfMobile())
            {
                HideKeyborad(true, callbackSync: true);
            }
        }

        void Start()
        {
            if (m_anchor == null)
            {
                m_anchor = this.transform;
            }

            // For desktops, set up the system assuming a keyboard is used.
            if (m_isMobile)
            {
                bool romajiBOXActive = m_romajiBOX.activeSelf;
                bool symbolBOXActive = m_symbolBOX.activeSelf;
                bool operatorBoxActive = m_operatorBOX.activeSelf;

                m_romajiBOX.SetActive(true);
                m_symbolBOX.SetActive(true);
                m_operatorBOX.SetActive(true);

                foreach (TLabKey key in TLabKey.Keys(m_keyBOX))
                {
                    key.SetKeyInputBuffer(m_inputBuffer);
                }

                m_romajiBOX.SetActive(romajiBOXActive);
                m_symbolBOX.SetActive(symbolBOXActive);
                m_operatorBOX.SetActive(operatorBoxActive);
            }

            if (m_isMobile)
            {
                m_updateKeybord = delegate
                {
                    foreach (string input in m_inputBuffer)
                    {
                        switch (input)
                        {
                            case BACKSPACE:
                                m_inputFieldBase?.OnBackSpacePressed();
                                AudioUtility.ShotAudio(m_audioSource, m_keyStroke, IMMEDIATELY);
                                break;
                            case ENTER:
                                m_inputFieldBase?.OnEnterPressed();
                                AudioUtility.ShotAudio(m_audioSource, m_keyStroke, IMMEDIATELY);
                                break;
                            case SHIFT:
                                foreach (TLabKey key in TLabKey.Keys(m_keyBOX))
                                {
                                    key.ShiftPressed();
                                }
                                m_inputFieldBase?.OnShiftPressed();
                                AudioUtility.ShotAudio(m_audioSource, m_keyStroke, IMMEDIATELY);
                                break;
                            case SPACE:
                                m_inputFieldBase?.OnSpacePressed();
                                AudioUtility.ShotAudio(m_audioSource, m_keyStroke, IMMEDIATELY);
                                break;
                            case TAB:
                                m_inputFieldBase?.OnTabPressed();
                                AudioUtility.ShotAudio(m_audioSource, m_keyStroke, IMMEDIATELY);
                                break;
                            case SYMBOL:
                                bool active = m_romajiBOX.activeSelf;
                                m_romajiBOX.SetActive(!active);
                                m_symbolBOX.SetActive(active);
                                m_inputFieldBase?.OnSymbolPressed();
                                AudioUtility.ShotAudio(m_audioSource, m_keyStroke, IMMEDIATELY);
                                break;
                            default:
                                m_inputFieldBase?.OnKeyPressed(input);
                                AudioUtility.ShotAudio(m_audioSource, m_keyStroke, IMMEDIATELY);
                                break;
                        }
                    }

                    m_inputBuffer.Clear();
                };
            }
            else
            {
                m_updateKeybord = delegate
                {
                    m_backSpaceKeyTime += Time.deltaTime;

                    if (Input.anyKey)
                    {
                        string inputString = Input.inputString;

                        if (Input.GetKeyDown(KeyCode.Return))
                        {
                            m_inputFieldBase?.OnEnterPressed();
                        }
                        else if (Input.GetKeyDown(KeyCode.Tab))
                        {
                            m_inputFieldBase?.OnTabPressed();
                        }
                        else if (Input.GetKeyDown(KeyCode.Space))
                        {
                            m_inputFieldBase?.OnSpacePressed();
                        }
                        else if (Input.GetKey(KeyCode.Backspace) && m_backSpaceKeyTime > BACKSPACE_DELAY)
                        {
                            m_inputFieldBase?.OnBackSpacePressed();
                            m_backSpaceKeyTime = 0.0f;
                        }
                        else if (inputString != "" && inputString != "")
                        {
                            m_inputFieldBase?.OnKeyPressed(inputString);
                        }
                        else if (Input.GetMouseButtonDown(1))
                        {
                            m_inputFieldBase?.OnKeyPressed(GUIUtility.systemCopyBuffer);
                        }
                    }
                };
            }
        }

        void Update()
        {
            m_updateKeybord.Invoke();
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            m_audioSource = GetComponent<AudioSource>();
            UnityEditor.EditorUtility.SetDirty(this);
        }

        private void CheckTLabKeyExist()
        {
            var buttons = m_keyBOX.GetComponentsInChildren<UnityEngine.UI.Button>();
            foreach (UnityEngine.UI.Button button in buttons)
            {
                var key = button.GetComponent<TLabKey>();
                if (key == null)
                {
                    button.gameObject.AddComponent<TLabKey>();
                }
            }
        }

        public void InitializeTLabKey()
        {
            CheckTLabKeyExist();

            foreach (TLabKey key in TLabKey.Keys(m_keyBOX))
            {
                key.Initialize();
                UnityEditor.EditorUtility.SetDirty(key);
            }
        }
#endif
    }
}