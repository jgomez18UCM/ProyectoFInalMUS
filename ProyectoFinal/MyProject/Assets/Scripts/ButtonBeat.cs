using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using FMODUnity;
using System.Runtime.InteropServices;
using System;

/// <summary>
/// Todas las siguientes variables son relativos a FMOD
/// </summary>
// Variables para recoger datos de las canciones

namespace Combat {
    public class ButtonBeat : MonoBehaviour
    {
        public static ButtonBeat instance;

        [StructLayout(LayoutKind.Sequential)]
        class TrackInfo
        {
            public int CurrentMusicBar = 0;
            public FMOD.StringWrapper LastMarker = new FMOD.StringWrapper();
        }

        FMOD.Studio.EVENT_CALLBACK beatCallback;
        [SerializeField] StudioEventEmitter song;
        FMOD.Studio.EventInstance songInstance;
        TrackInfo info;
        GCHandle timelineHandle;


        [SerializeField] RectTransform buttonTransform;
        [SerializeField] float pulseSize = 1.15f;
        [SerializeField] float returnSpeed = 5f;
        Vector3 startSize;

        bool hover = false;

        public void changeHover(bool inside)
        {
            hover = inside;

            if (hover)
                songInstance.start();
            else
                songInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

        public bool getHover() { return hover; }

        void OnDestroy()
        {
            songInstance.setUserData(IntPtr.Zero);
            songInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            songInstance.release();
            timelineHandle.Free();

        }

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            startSize = buttonTransform.localScale;

            beatCallback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);

            songInstance = song.EventInstance;

            info = new TrackInfo();

            timelineHandle = new GCHandle();
            timelineHandle = GCHandle.Alloc(info, GCHandleType.Pinned);

            songInstance.setUserData(GCHandle.ToIntPtr(timelineHandle));
            songInstance.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
            songInstance.setVolume(GameManager.instance.getVolume());
            float aux; songInstance.getVolume(out aux);
            Debug.Log(aux);
        }

        public void pulse() { buttonTransform.localScale = startSize * pulseSize; }


        // Update is called once per frame
        void Update()
        {
            buttonTransform.localScale = Vector3.Lerp(transform.localScale, startSize, Time.deltaTime * returnSpeed);
        }


        [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
        static FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
        {
            Debug.Log("Ritmo");
            // Creamos una nueva instancia a partir del puntero
            FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);

            // Sacamos toda la informacion de la instancia
            IntPtr timelineInfoPtr;
            FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);

            // Control de errores, por si no conseguimos los datos
            if (result != FMOD.RESULT.OK)
            {
                Debug.LogError("Timeline Callback error: " + result);
            }
            else if (timelineInfoPtr != IntPtr.Zero)
            {
                // Guardamos los datos del objeto y detalles de los marcadores
                GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
                TrackInfo timelineInfo = (TrackInfo)timelineHandle.Target;

                // Dependiendo del tipo de evento que pille
                switch (type)
                {
                    case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                        {
                            var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                            timelineInfo.CurrentMusicBar = parameter.bar;
                            if (ButtonBeat.instance.getHover())
                                ButtonBeat.instance.pulse();
                            break;
                        }
                    case FMOD.Studio.EVENT_CALLBACK_TYPE.DESTROYED:
                        {
                            // Liberamos memoria al terminar
                            timelineHandle.Free();
                            break;
                        }
                }
            }
            return FMOD.RESULT.OK;
        }
    }
}
