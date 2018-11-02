using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextScript : MonoBehaviour
{

    // Use this for initialization
    public UnityEngine.UI.InputField messageInput;

    public UnityEngine.UI.InputField bundleIDInput;
    public UnityEngine.UI.Text MessageText;

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");


            // string arguments = intent.Call<string>("getDataString");
            // Debug.Log("arguments: " + arguments);

            bool hasExtra = intent.Call<bool>("hasExtra", "message");

            if (hasExtra)
            {
                // AndroidJavaClass extras = intent.Call<AndroidJavaClass>("getExtras");
                // string argument = extras.Call<string>("getString", "from");

                string message = intent.Call<string>("getStringExtra", "message");
                Debug.Log("message: " + message);

                if (message != null && message.Length > 0)
                {
                    MessageText.text = message;
                }
            }
        }
    }

    void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnMouseDown);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnMouseDown()
    {
        // GetComponent<UnityEngine.UI.Text>().color = new Color(0, 1, 0, 1);

        bool fail = false;

        string bundleId = bundleIDInput.text; // your target bundle id
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");

        AndroidJavaObject launchIntent = null;
        try
        {
            launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleId);
        }
        catch (System.Exception e)
        {
            fail = true;
        }

        if (fail)
        { //open app in store
            Application.OpenURL("https://google.com");
        }
        else //open the app
        {
            launchIntent.Call<AndroidJavaObject>("putExtra", "message", messageInput.text);

            ca.Call("startActivity", launchIntent);
        }

        up.Dispose();
        ca.Dispose();
        packageManager.Dispose();
        launchIntent.Dispose();
    }

    // void OnPointerDown()
    // {
    //     GetComponent<TextMesh>().color = new Color(0, 1, 0, 0);
    //     Debug.Log("onPointerDown");
    // }
}
