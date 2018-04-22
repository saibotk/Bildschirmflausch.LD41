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
	void Start () {
        delay = 0;
        showingMessage = false;
        messages = new List<Notification>();
        text = GetComponent<Text>();
	}

	void Update() {
        if (showingMessage) {
            if (Time.time > delay) {
                if (messages.Count == 1) {
                    GetComponentInParent<Image>().enabled = false;
                    messages.Remove(messages[0]);
                    showingMessage = false;
                } else {
                    text.text = messages[0].getText();
                    delay = Time.time + messages[0].getDuration();
                }
            }
        }
	}

	public void showMessage(string text, float duration) {
        if (showingMessage) {
            messages.Add(new Notification(text, duration));         
        } else {
			showingMessage = true;

            GetComponent<Text>().text = text;
            delay = Time.time + duration;
            GetComponentInParent<Image>().enabled = true;
        }
    }
}
