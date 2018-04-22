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

        public string getText() {
            return text;
        }

        public float getDuration() {
            return duration;
        }
    }

    // Use this for initialization
    void Start() {
        delay = 0;
        messages = new List<Notification>();
        text = GetComponent<Text>();
        hide();
    }

    void Update() {
        if ( showingMessage ) {
            if ( Time.time > delay ) {
                if (messages.Count <= 1) {
					messages.Remove(messages[0]);
                    hide();
                } else {
                    messages.Remove(messages[0]);
                    text.text = messages[0].getText();
                    delay = Time.time + messages[0].getDuration();
                }
            }
        }
    }

    public void showMessage(string text, float duration) {
        if ( showingMessage ) {
            messages.Add(new Notification(text, duration));
        } else {
            showingMessage = true;

            GetComponent<Text>().text = text;
            delay = Time.time + duration;
            show();
        }
    }

    void show() {
        GetComponentInParent<Image>().enabled = true;
        text.enabled = true;
        showingMessage = true;
    }

    void hide() {
        GetComponentInParent<Image>().enabled = false;
        text.enabled = false;
        showingMessage = false;
    }
}
