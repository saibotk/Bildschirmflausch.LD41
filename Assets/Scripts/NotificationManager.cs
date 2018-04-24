using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour {


    List<Notification> messages;
    bool showingMessage;
    float delay;
    Text text;

    class Notification {
        string text;
        float duration;

        public Notification(string text, float duration) {
            this.text = text;
            this.duration = duration;
        }

        public string GetText() {
            return text;
        }

        public float GetDuration() {
            return duration;
        }
    }

    public NotificationManager() {
        messages = new List<Notification>();
        delay = 0;
    }

    // Use this for initialization
    void Start() {
        text = GetComponent<Text>();
        Hide();
    }

    void Update() {
        if ( showingMessage ) {
            if ( Time.time > delay ) {
                if (messages.Count == 1) {
					messages.Remove(messages[0]);
                    Hide();
                } else if (messages.Count > 1) {
                    messages.Remove(messages[0]);
                    text.text = messages[0].GetText();
                    delay = Time.time + messages[0].GetDuration();
                }
            }
        }
    }

    public void ShowMessage(string text, float duration) {
        messages.Add(new Notification(text, duration));

        if (!showingMessage) {
            showingMessage = true;
            GetComponent<Text>().text = text;
            delay = Time.time + duration;
            Show();
        }
    }

    void Show() {
        GetComponentInParent<Image>().enabled = true;
        text.enabled = true;
        showingMessage = true;
    }

    void Hide() {
        GetComponentInParent<Image>().enabled = false;
        text.enabled = false;
        showingMessage = false;
    }
}
